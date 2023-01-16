using System;
using System.Collections.Generic;
using System.Linq;

namespace DotA.API.Helpers
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new Random();
        public static List<T> Sample<T>(this List<T> list, int count)
        {
            return list.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        /// <summary>
        /// splits int number into N parts
        /// Example: 29 -> 16 + 5 + 1 + 5 + 2
        /// </summary>
        /// <param name="number"></param>
        /// <param name="parts">count of parts to split the number</param>
        /// <param name="min">the minimum value of a part</param>
        /// <returns></returns>
        public static List<int> SplitNumber(this int number, int parts, int min)
        {
            var leftOfNumber = number;
            var result = new List<int>();
            for (var i = 0; i < parts - 1; i++)
            {
                var part = _random.Next(Math.Min(min, leftOfNumber), Math.Max(0, leftOfNumber - min));
                leftOfNumber -= part;
                result.Add(part);
            }
            
            result.Add(leftOfNumber);
            return result;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(_ => Guid.NewGuid()).ToList();
        }
    } 
}