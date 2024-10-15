using Chat_Backend.Models.Backend;
using Chat_Backend.Models.Options;
using Chat_Backend.Repositories.AccountRepository;
using Chat_Backend.Services.AccountService;
using Storage.Models.Backend;

namespace Services.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly JwtOptions _jwtOptions;

    public AccountService
    (
        IAccountRepository accountRepository,
        JwtOptions jwtOptions
    )
    {
        _accountRepository = accountRepository;
        _jwtOptions = jwtOptions;
    }

    public async Task<Account?> GetAccountByLoginAsync(string login)
    {
        var accounts = await _accountRepository.GetAllAsync();
        var account = accounts.FirstOrDefault(a => a.Login == login);
        return account;
    }

    public async Task<Tokens> RefreshTokens(Account account)
    {
        var tokens = _jwtOptions.GetJwtTokens(account.Login);
        account.Tokens = tokens;
        await UpdateAsync(account.Id, account);
        return account.Tokens;
    }

    public async Task<Error> RegistrationValidation(string control)
    {
        var accounts = await _accountRepository.GetAllAsync();
        Error validationError = new();
        if (accounts.Any(x => x.Login == control))
        {
            validationError = new Error
            {
                Type = "Login",
                Message = "This login is already in use."
            };
        }
        if (accounts.Any(x => x.Email == control))
        {
            validationError = new Error
            {
                Type = "Email",
                Message = "This email is already registered."
            };
        }
        return validationError;
    }

    public async Task<IList<Account>> GetAllAsync() =>
        await _accountRepository.GetAllAsync();

    public async Task<Account> GetAsync(Guid id) =>
        await _accountRepository.GetAsync(id);

    public async Task DeleteAsync(Guid id) =>
        await _accountRepository.DeleteAsync(id);

    public async Task UpdateAsync(Guid id, Account item) =>
        await _accountRepository.UpdateAsync(id, item);

    public async Task CreateAsync(Account item) =>
        await _accountRepository.CreateAsync(item);
}