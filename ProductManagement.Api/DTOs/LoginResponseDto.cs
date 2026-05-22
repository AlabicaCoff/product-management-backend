namespace ProductManagement.Api.DTOs
{
    public class LoginResponseDto
    {
        public bool IsLoginSuccess { get; set; }
        public string? FailureMessage { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
