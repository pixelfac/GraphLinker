using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Models;
using Sercices;

namespace Script
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TwitterService service = new TwitterService("AAAAAAAAAAAAAAAAAAAAABRoWQEAAAAAJZR7mPFouya5TJXoSFlJRINHnlU%3De9j9t43DeCEjns9zl5PvW2VFEte9CLmBwPwsqifY6G7eZZFQcv");

            FollowingResponse response = await service.GetFollowingById("12");

            Console.WriteLine(response.meta.ResultCount);
            Console.WriteLine(response.data.Count());
        }
    }
}
