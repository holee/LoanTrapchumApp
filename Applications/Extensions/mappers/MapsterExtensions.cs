using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Extensions.mappers
{
    public static class MapsterExtensions
    {
        public static IEnumerable<T> Maps<T>(this object obj) where T : class
        {
            return obj.Adapt<IEnumerable<T>>(); 
        }

        public static List<T> MapList<T>(this object obj) where T : class
        {
            return obj.Adapt<List<T>>();
        }


    }
}
