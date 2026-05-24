using CMIS.Data;
using CMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMIS.Services;

public class AuthService
{
    private readonly ApplicationDbContext _db;

    public AuthService(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Authenticates a user by email/username and password.
    /// Returns the Account with Profile and Role if successful, null otherwise.
    /// </summary>
    public async Task<Account?> LoginAsync(string emailOrUsername, string password)
    {
        if (string.IsNullOrWhiteSpace(emailOrUsername) || string.IsNullOrWhiteSpace(password))
            return null;

        var account = await _db.Accounts
            .Include(a => a.Profile)
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a =>
                (a.Email == emailOrUsername || a.Username == emailOrUsername)
                && a.Status == "Active");

        if (account is null)
            return null;

        // Verify password hash using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);

        return isValid ? account : null;
    }

    /// <summary>
    /// Hashes a plain-text password using BCrypt (for registration/seeding).
    /// </summary>
    public static string HashPassword(string plainPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword);
    }
}
