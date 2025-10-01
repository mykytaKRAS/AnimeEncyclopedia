namespace AnimeEncyclopedia.API.DTOs;

public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string UsernameOrEmail, string Password);
public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);
public record RefreshRequestDto(string RefreshToken);
public record RevokeRequestDto(string RefreshToken);