namespace ASP.NETAuthITStep.Auth.Options
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";
        
        public string SigningKey { get; set; }
        public int TokenLifetime { get; set; }
    }
}