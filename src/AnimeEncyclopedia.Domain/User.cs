namespace AnimeEncyclopedia.Domain;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!; // хранится хеш (BCrypt)
    public string Role { get; set; } = "User"; // "User" или "Admin"
    public List<RefreshToken> RefreshTokens { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}