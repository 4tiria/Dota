using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotA.API.Models.DTO;

namespace DotA.API.Helpers
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            return string.Join("", 
                value.Split(' ')
                .Select(x => x[0].ToString().ToUpper() + x[1..]));
        }
        
        public static string ToCamelCase(this CamelCaseNameJs value)
        {
            return value.Name.ToCamelCase();
        }


        public static string FromCamelCase(this string value)
        {
            var list = new List<string>();
            var queue = new StringBuilder();
            foreach (var c in value)
            {
                if (char.IsUpper(c))
                {
                    if (queue.Length > 0)
                    {
                        list.Add(queue.ToString());
                        queue.Clear();
                    }
                }
                
                queue.Append(c);
            }
            
            list.Add(queue.ToString());
            var result = string.Join(" ",list);
            return result;
        }
        
                
        public static string FromCamelCase(this CamelCaseNameJs value)
        {
            return value.Name.FromCamelCase();
        }
    }
}