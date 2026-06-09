namespace VoyageManager.Application.Abstractions;

public interface IVoyagePasswordHasher
{
    string HashPassword(string password);

    bool VerifyPassword(string hashedPassword, string password);
}
