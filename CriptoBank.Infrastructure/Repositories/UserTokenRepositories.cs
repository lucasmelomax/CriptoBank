using System;
using CriptoBank.Application.Repositories.Token;
using CriptoBank.Domain.Models;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Repositories;

public class UserTokenRepositories : IUserTokenRepositories
{
    private readonly CriptoDbContext _context;

    public UserTokenRepositories(CriptoDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken ct)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task AddAsync(
        User user,
        CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
    }
}