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
        public static IEnumerable<T> AdaptMany<T>(this object obj) where T : class
        {
            return obj.Adapt<IEnumerable<T>>(); 
        }

    }
}
