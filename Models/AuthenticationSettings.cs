namespace my_app_backend.Models
{
    public class AuthenticationSettings
    {
        public static string OptionName = "AuthenticationSettings";
        public string SecurityKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
