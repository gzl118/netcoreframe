using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SANS.Common
{
    public class StringHelper
    {
        public static List<string> ExtractStringBetweenBeginAndEnd(string input, string begin, string end)
        {
            List<string> vs = new List<string>();
            Regex regex = new Regex(string.Format(@"(?<={0}).*?(?={1})", begin, end));
            foreach (Match match in regex.Matches(input))
            {
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    vs.Add(match.Value);
                }
            }
            return vs;
        }
        public static string ExtractStringBetweenBeginAndEndByFirst(string input, string begin, string end)
        {
            Regex regex = new Regex(string.Format(@"(?<={0}).*?(?={1})", begin, end));
            foreach (Match match in regex.Matches(input))
            {
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    return match.Value;
                }
            }
            return null;
        }
    }
}
