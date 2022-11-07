using Application.ApplicationUsers.DTOs;

namespace Application.ApplicationUsers.Models;

public class SignInResponse
{
    public string Jwt { get; set; }
    
    public UserDto User { get; set; }
}