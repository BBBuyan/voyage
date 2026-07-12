namespace VoyageManager.Application.Abstractions;

public interface IWorkerPasswordHasher
{
    string HashPassword(string password);

    bool VerifyPassword(string hashedPassword, string password);
}
