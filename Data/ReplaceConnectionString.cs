namespace ToDoList_RestAPI.Data
{
    public static class ReplaceConnectionString
    {
        public static string BuildConnectionString(string rawConnectionString)
        {
            if(string.IsNullOrEmpty(rawConnectionString))
                throw new ArgumentException("The Connection String Can't Be Null");

            return rawConnectionString
                .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "")
                .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "")
                .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
                .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");
        }
    }
}