namespace Applications.Helpers
{
    public static class DictionaryExtension
    {
        public static Dictionary<string, string[]> AddValue(this Dictionary<string, string[]> dict, string key, string[] value)
        {
            dict.Add(key, value);
            return dict;
        }
    }

}
