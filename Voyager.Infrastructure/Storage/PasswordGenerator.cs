using System.Security.Cryptography;
using Voyager.Application.Abstractions;

namespace Voyager.Infrastructure.Storage;

public class PasswordGenerator : IPasswordGenerator
{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string Symbols = "!@#$%^&*()-_=+[]{};:,.<>?";
    private const int PasswordLength = 16;

    public string GeneratePassword()
    {
        string allChars = Uppercase + Lowercase + Digits + Symbols;
        return RandomNumberGenerator.GetString(allChars, PasswordLength);
    }
}
