using AutoMapper;
using Chat_Backend.Models.Backend;
using Chat_Backend.Models.Frontend;
using Chat_Backend.Models.Options;
using Chat_Backend.Services.AccountService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    private readonly CryptoOptions _cryptoOptions;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAccountService accountService,
        IMapper mapper,
        CryptoOptions cryptoOptions,
        JwtOptions jwtOptions,
        ILogger<AuthController> logger
    )
    {
        _accountService = accountService;
        _mapper = mapper;
        _cryptoOptions = cryptoOptions;
        _jwtOptions = jwtOptions;
        _logger = logger;
    }
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Test successful");
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] AccountAuthModel accountAuthModel)
    {
        try
        {
            var salt = _cryptoOptions.GenerateSalt();
            var hashPassword = _cryptoOptions.GenerateHashPassword(accountAuthModel.Password, salt);
            var tokens = _jwtOptions.GetJwtTokens(accountAuthModel.Login);
            var account = new Account(accountAuthModel, Convert.ToBase64String(salt), Convert.ToBase64String(hashPassword), tokens);
            await _accountService.CreateAsync(account);
            var accountModel = _mapper.Map<AccountModel>(account);
            _logger.LogInformation("User registered successfully: {Login}", accountAuthModel.Login);
            return Ok(accountModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration: {Login}", accountAuthModel.Login);
            return StatusCode(500, "Internal server error"); // Спросить что лучше вернуть?
        }
    }

    [HttpPut("Auth")]
    public async Task<IActionResult> Login([FromBody] AccountAuthModel accountAuthModel)
    {
        try
        {
            var account = await _accountService.GetAccountByLoginAsync(accountAuthModel.Login);
            if (account == null)
            {
                _logger.LogError("Authorization failed: User with {Login} does not exist", accountAuthModel.Login);
                return Unauthorized();
            }
            var hashPassword = _cryptoOptions.GenerateHashPassword(accountAuthModel.Password, Convert.FromBase64String(account.Salt));
            if (Convert.ToBase64String(hashPassword) == account.HashPassword)
            {
                var tokens = _jwtOptions.GetJwtTokens(account.Login);
                account.Tokens = tokens;
                await _accountService.UpdateAsync(account.Id, account);
                var loginedAccount = _mapper.Map<AccountModel>(account);
                _logger.LogInformation("Authorization for user with login: {Login} successfully", accountAuthModel.Login);
                return Ok(loginedAccount);
            }
            else
            {
                _logger.LogError("Authorization failed: Incorrect password for user {Login}", accountAuthModel.Login);
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user authorization: {Login}", accountAuthModel.Login);
            return StatusCode(500, "Internal server error"); // Спросить что лучше вернуть?
        }
    }

    [HttpPut("RefreshTokens/{refreshToken}")]
    public async Task<IActionResult> RefreshTokens(string refreshToken)
    {
        try
        {
            var login = _jwtOptions.GetLoginFromJwtToken(refreshToken);
            var account = await _accountService.GetAccountByLoginAsync(login);
            if (account?.Tokens.RefreshToken == refreshToken)
            {
                var tokens = await _accountService.RefreshTokens(account);
                _logger.LogInformation("Tokens for user with login: {Login} updated successfully", account.Login);
                return Ok(tokens);
            }
            else
            {
                _logger.LogError("Error occurred during tokens refreshing");
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during tokens refreshing:");
            return StatusCode(500, "Internal server error"); // Спросить что лучше вернуть?
        }
    }
}