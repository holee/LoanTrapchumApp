using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Extensions.Linqs
{
    public static class LinqExtensions
    {
        public static bool Between<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }
    }

}
