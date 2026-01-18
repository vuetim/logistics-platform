using BCrypt.Net;
using LogisticsPlatform.Application.Interfaces.Services.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
}
