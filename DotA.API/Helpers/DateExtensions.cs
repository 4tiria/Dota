using System;

namespace DotA.API.Helpers
{
    public static class DateExtensions
    {
        public static long ToLong(this DateTime dateTime)
        {
            var time = TimeSpan.FromTicks(DateTime.UnixEpoch.Ticks);
            return (dateTime - time).Ticks / 10000;
        }

        public static DateTime ToDateTime(this long universalMilliseconds)
        {
            var time = TimeSpan.FromMilliseconds(universalMilliseconds);
            return DateTime.UnixEpoch + time;
        }
    }
}