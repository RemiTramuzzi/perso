using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Tools.Misc
{
    public class PropertyEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        public Func<T, object>[] Properties { get; set; }

        public PropertyEqualityComparer(params Func<T, object>[] properties)
        {
            Properties = properties;
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return Properties.All(p => p(x).Equals(p(y)));
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}
