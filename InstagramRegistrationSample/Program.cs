/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ My Telegram Account: https://t.me/ramtinak ]
 * 
 * 
 * This sample is a part of InstagramApiSharp's private library.
 * 
 * 
 * IRANIAN DEVELOPERS
 */

using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Enums;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;

/*
 * ////////////////////////////// WARNING /////////////////////////////////
 * 1. You need InstagramApiSharp v1.6.0 for running this sample!
 * 2. This registration feature only works on .NETFramework 4.6.1 or newer or .NETStandard 2.0 or newer.
 * 3. Note that I didn't implement v180 API in the InstagramApiSharp's library, so it's only works for registration!!!!!
 *    You have to modify it a little bit by yourself to support it for LoginAsync and other stuff!
 * 4. Registering account via Phone is just like this one, so I didn't add it! 
 *    but if you need it, you can purchase the library, it's already has the sample for phone registration!
 * 
 */

namespace InstagramRegistrationSample
{
    class Program
    {
        private static IInstaApi InstaApi;

#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Task.Run(MainAsync).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
        
        }



        static async Task Delay(double seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}
