namespace AngularStandaloneDemo.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
       
        public string Username { get; set; } = string.Empty;
        public int JobTitleID { get; set; } // Added JobTitleId for role-based routing

        public string FirstName { get; internal set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int UserID { get; set; }
      
    }
}
