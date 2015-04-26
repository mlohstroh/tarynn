using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public static class Extensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long SinceEpoch(this DateTime t)
        {
            var span = t.ToUniversalTime() - UnixEpoch;
            return Convert.ToInt64(span.TotalSeconds);
        }
    }
}
