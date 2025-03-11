namespace AngularStandaloneDemo.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
       
        public string Username { get; set; } = string.Empty;
        public int JobTitleId { get; set; } // Added JobTitleId for role-based routing
    }
}
