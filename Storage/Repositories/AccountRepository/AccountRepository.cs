using Chat_Backend.Models.Backend;
using Chat_Backend.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace Chat_Backend.Repositories.AccountRepository;

/// <summary>
/// 
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Account item)
    {
        await _context.Accounts.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IList<Account>> GetAllAsync() =>
        await _context.Accounts.ToListAsync();

    public async Task<Account> GetAsync(Guid id) =>
        (await _context.Accounts.FindAsync(id))!;

    public async Task UpdateAsync(Guid id, Account item)
    {
        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount != null)
        {
            _context.Entry(existingAccount).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }
    }
}