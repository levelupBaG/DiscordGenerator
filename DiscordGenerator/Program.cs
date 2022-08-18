// Made by AvianHere | https://github.com/AvianHere
using System;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Threading;

// Made by AvianHere | https://github.com/AvianHere

// This program Is distributed for educational purposes only, 
// I am not responsible for what you do with this software in any way possible.
// If you skid this please leave credits to the original owner.

namespace DiscordGenerator
{
    class Program
    {
        private static readonly HttpClient httpclient = new HttpClient();
        private static readonly WebClient webclient = new WebClient();
        private static readonly string giftcheck = "https://discord.com/api/v6/entitlements/gift-codes/NITROCODE?with_application=false&with_subscription_plan=true";
        static readonly string giftlink = "https://discord.gift/";
        static string webhook;
        static int validamount = 0;
        static bool bypassratelimit = false;
        static bool displayinvalid = false;
        static bool webhookvalid = false;
        static bool pingeveryone = false;
        private static readonly Color nitrolink = Color.CornflowerBlue;
        private static readonly Color purple = Color.Purple;
        private static readonly Color invalid = Color.Red;
        private static readonly Color valid = Color.Green;
        static void Main()
        {
            Console.Title = "Discord Nitro Generator  |  Made by github.com/AvianHere";
            Console.WriteLine(@"                                    ___          _                __ __                ", purple);
            Console.WriteLine(@"                                   / _ | _  __  (_) ___ _  ___   / // / ___   ____ ___ ", purple);
            Console.WriteLine(@"                                  / __ || |/ / / / / _ `/ / _ \ / _  / / -_) / __// -_)", purple);
            Console.WriteLine(@"                                 /_/ |_||___/ /_/  \_,_/ /_//_//_//_/  \__/ /_/   \__/ ", purple);
            Console.Write($"\nShow invalid gifts (yes/no) => ", purple);
            if (string.Compare(Console.ReadLine(), "yes", true) == 0)
                displayinvalid = true;
            Console.Write($"Bypass rate limits (yes/no) => ", purple);
            if (string.Compare(Console.ReadLine(), "yes", true) == 0)
                bypassratelimit = true;
            Console.Write($"Set webhook (optional) => ", purple);
            webhook = Console.ReadLine();
            if (!string.IsNullOrEmpty(webhook))
            {
                try {
                    if (!httpclient.GetAsync(webhook).Result.IsSuccessStatusCode)
                    {
                        Console.Write("Invalid webhook.\n", invalid);
                        Console.Write("Webhook example: https://discord.com/api/webhooks/740959217493206913/KAOJP3jopjpdsaAasASpxasAssaXHWTzREGAzhioaIOZHHOI3hoc \n", invalid);
                        webhook = "";
                    }
                    else
                    {
                        webhookvalid = true;
                        Console.Write($"Ping @everyone when nitro sniped? (yes/no) => ", purple);
                        if (string.Compare(Console.ReadLine(), "yes", true) == 0)
                            pingeveryone = true;
                    }
                }
                catch 
                { 
                    Console.Write("Invalid webhook.\n", invalid); 
                    Console.Write("Webhook example: https://discord.com/api/webhooks/740959217493206913/KAOJP3jopjpoapSOPOasopAXHWTzREGAzhioaIOZHHOI3hoc \n", invalid);
                    webhook = ""; 
                }
            }
            httpclient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!displayinvalid) { 
                Console.Write(DateTime.Now.ToString("HH:mm:ss"), purple); 
                Console.Write(" Sniper started.\n", valid); 
            }
            while (true) {
                var nitro = Nitro();
                var status = httpclient.GetAsync(giftcheck.Replace("NITROCODE", nitro)).Result;
                if (status.IsSuccessStatusCode)
                {
                    Console.Write(DateTime.Now.ToString("HH:mm:ss"), purple);
                    Console.Write(" [VALID] -> ", valid);
                    Console.Write(giftlink + nitro + "\n", nitrolink);
                    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory() + "nitro.txt"), giftlink + nitro);
                    ++validamount;
                    Console.Title = $"Discord Nitro Sniper  |  Valid: {validamount}  |  Made by github.com/AvianHere";
                    if (webhookvalid)
                        Post(giftlink + nitro);
                    continue;
                }
                else if (bypassratelimit && status.ReasonPhrase == "Too Many Requests")
                {
                    Console.Write(DateTime.Now.ToString("HH:mm:ss"), purple);
                    Console.Write(" [RATELIMITED] -> Sleeping for 60 seconds\n", invalid);
                    Thread.Sleep(60000);
                    continue;
                }
                else if (displayinvalid)
                {
                    Console.Write(DateTime.Now.ToString("HH:mm:ss"), purple);
                    Console.Write(" [INVALID] -> ", invalid);
                    Console.Write(giftlink + nitro + "\n", nitrolink);
                }
            }
        }
        static string Nitro()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++) { stringChars[i] = chars[random.Next(chars.Length)]; }
            var nitro = new String(stringChars);
            return nitro;
        }
        static void Post(string nitro)
        {
            webclient.UploadValues(webhook, new NameValueCollection
            {
                {"username", "github.com/AvianHere - DiscordSniper"},
                {"avatar_url", "https://avatars.githubusercontent.com/u/111364000?v=4"},
                {"content", pingeveryone? $"@everyone\n{nitro}" : $"{nitro}"}
            });
        }
    }
}
