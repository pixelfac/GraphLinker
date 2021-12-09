using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

using Models;
using Sercices;

namespace Script
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TwitterService service = new TwitterService("AAAAAAAAAAAAAAAAAAAAABRoWQEAAAAAJZR7mPFouya5TJXoSFlJRINHnlU%3De9j9t43DeCEjns9zl5PvW2VFEte9CLmBwPwsqifY6G7eZZFQcv");

            //TwitterUser tommy = await service.GetUserByHandle("tommyinnit");
            //FollowingResponse response = await service.GetFollowingByUser(tommy);

            //if (tommy.PublicMetrics.Following != response.data.Count())
            //    Console.WriteLine("Error: Following Count is not equal to data size");

            //FileIOService.ToCSV(tommy, response.data);

            var lines = 0;
            foreach (var file in Directory.GetFiles("output"))
            {
                lines += (await File.ReadAllLinesAsync(file)).Count();
            }
            Console.WriteLine(lines);

            /*foreach (var creator in await File.ReadAllLinesAsync("creators.txt"))
            {
                if (File.Exists($"output/{creator}.csv"))
                    continue;
                
                TwitterUser user = await service.GetUserByHandle(creator);
                FollowingResponse response = await service.GetFollowingByUser(user);

                if (user.PublicMetrics.Following != response.data.Count())
                   Console.WriteLine("Error: Following Count is not equal to data size");

                foreach (var depth in response.data.Where(x => x.Verified is true))
                {
                    if (File.Exists($"output/{depth.Username}.csv"))
                        continue;
                    
                    TwitterUser newUser = await service.GetUserByHandle(depth.Username);
                    FollowingResponse res = await service.GetFollowingByUser(newUser);

                    if (user.PublicMetrics.Following != response.data.Count())
                        Console.WriteLine("Error: Following Count is not equal to data size");

                    FileIOService.ToCSV(newUser, res.data);
                }

                FileIOService.ToCSV(user, response.data);
            }*/
        }
    }
}
