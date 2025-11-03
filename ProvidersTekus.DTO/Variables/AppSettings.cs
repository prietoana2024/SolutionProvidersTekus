namespace ProvidersTekus.DTO.Variables
{
    public static class AppSettings
    {
        public static bool IsProduction { get; set; }

        public static string DB_CONNECTION { get {
                if (IsProduction)
                    return "DbConnection";
                else
                    return "DbConnectionDev";
            } }
    }
}
