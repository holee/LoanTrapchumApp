namespace Application.Helpers
{
    public static class QueryStringHelper
    {
        private static string AddQueryString(string basePath, Dictionary<string, object?> queryParams)
        {
            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return $"{basePath}?{queryString}";
        }

        public static string AddQueryString(this Dictionary<string, object?> queryParams, string basePath = "/")
        {
            return AddQueryString(basePath, queryParams);
        }
    }
}
