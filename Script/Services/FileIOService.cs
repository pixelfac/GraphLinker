using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Models;

namespace Sercices
{
    class FileIOService
    {
        private static string MakeCSVLine(TwitterUser user)
        {
            var builder = new StringBuilder();
            
            builder.Append(user.Id);
            builder.Append(",");
            builder.Append(user.Username);
            builder.Append(",");
            //builder.Append(user.Name);
            //builder.Append(",");
            builder.Append(user.Verified);
            builder.Append(",");

            builder.Append(user.PublicMetrics.Followers);
            builder.Append(",");
            builder.Append(user.PublicMetrics.Following);
            //builder.Append(",");
            //builder.Append(user.PublicMetrics.Tweets);
            //builder.Append(",");
            //builder.Append(user.PublicMetrics.Listed);

            return builder.ToString();
        }

        public static void ToCSV(TwitterUser rootUser, IEnumerable<TwitterUser> users)
        {
            List<string> lines = new List<string>();

            lines.Add(MakeCSVLine(rootUser));
            
            foreach (var user in users)
            {
                lines.Add(MakeCSVLine(user));
            }
            
            File.WriteAllLines($"output/{rootUser.Username}.csv", lines);
        }
    }
}