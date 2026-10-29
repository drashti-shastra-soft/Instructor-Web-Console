using System;
using System.Globalization;
using System.Linq;

namespace InstructorWebClient.Infrastructure.Utils
{
    public static class StaticMethods
    {
        public static int ConvertToMinutes(string timeStr)
        {
            var parts = timeStr.Split(':');
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            return hours * 60 + minutes;
        }

        public static string FormatTimeDuration(TimeSpan duration)
        {
            int hours = duration.Hours + duration.Days * 24; // Include days if needed
            int minutes = (int)Math.Round(duration.Minutes + duration.Seconds / 60.0);

            return $"{hours} Hour{(hours > 1 ? "s" : string.Empty)} and {minutes} Minute{(minutes > 1 ? "s" : string.Empty)}";
        }

        public static string ConcatString(string specialChar, params string[] args)
        {
            string outputString = string.Empty;
            if (args != null && args.Length > 0)
            {
                outputString = string.Join(specialChar, args.Where(r => r.IsNotNullOrEmpty()));
            }
            return outputString;
        }

        public static TimeSpan ParseHour(string time)
        {
            if (time.IsNotNullOrEmpty())
            {
                return TimeSpan.ParseExact(time, @"hh\:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
    }
}
