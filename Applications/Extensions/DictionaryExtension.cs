using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Extensions
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
