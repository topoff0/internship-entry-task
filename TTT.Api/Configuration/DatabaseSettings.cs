namespace TTT.Api.Configuration
{
    public class DatabaseSettings
    {
        public string PostgresUser { get; set; } = default!;
        public string PostgresPassword { get; set; } = default!;
        public string PostgresDatabase { get; set; } = default!;
        public string PostgresPort { get; set; } = default!;
    }
}