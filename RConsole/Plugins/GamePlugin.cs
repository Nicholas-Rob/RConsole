using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.IO;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using System.Xml;
using AngleSharp.Dom;
using Newtonsoft.Json;
using Helper;
using System.Diagnostics;

namespace RConsole.Plugins
{
    class GamePlugin : CommandBase
    {
        public const string STEAM_LOCATION = "C:\\Program^ Files^ ^(x86^)\\Steam\\steam.exe";
        public const string STEAM_ID = "450gOfBread";
        public const string LOCAL_STEAM = "steam_games.txt";
        

        public static Dictionary<string, string> steamGames = new Dictionary<string, string>();
        public static Dictionary<string, string> emulatorGames = new Dictionary<string, string>();

        public static bool steamDictChanged = false;

        [OnLoad]
        public static void OnLoad()
        {
            if (File.Exists(LOCAL_STEAM))
            {
                steamGames = ReadLocalSteamFile();
            }
            
            try
            {

                 LoadGamesListSteamWeb(STEAM_ID);

            }catch(Exception e) { }
            
        }

        [OnUnload]
        public static void OnUnload()
        {
            if (steamGames.Count > 0 && steamDictChanged)
            {
                SaveLocalSteamFile();
            }
        }

        [Command("play")]
        public static bool PlayCommand(string[] args)
        {

            PlayHandler(args);

            

            return true;
        }

        [Command("games")]
        public static bool GamesCommand(string[] args)
        {
            if(args[0] == null)
            {
                return false;
            }

            switch (args[0]){
                case "list":
                    PrintSteamList();
                    break;

                default:
                    return false;
            }

            return true;
        }



        private static bool PlayHandler(string[] args)
        {

            // args = [name of steam game]
            // TODO: args = [name of steam game]; or args = -[console emulator code] [name of game]

            if (args[0][0] != '-')
            {
                LaunchSteamGame(args);
            }

            return true;
        }


        private static void LaunchSteamGame(string[] args)
        {
            string name = args.ArrayString();

            if (steamGames.ContainsKey(name))
            {
                
                
                Process proc = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C " + STEAM_LOCATION + " -applaunch " + steamGames[name];

                proc.StartInfo = startInfo;
                proc.Start();
            }
        }
        

        private static void PrintSteamList()
        {
            Console.WriteLine("Steam Games: ");
            foreach(string name in steamGames.Keys)
            {
                Console.WriteLine(name);
            }
        }

        private static async void LoadGamesListSteamWeb(string steamid)
        {

            string uri = "https://steamcommunity.com/id/"+steamid+"/games?xml=1";

            HttpClient client = new HttpClient();

            HttpResponseMessage message = await client.GetAsync(uri);


            Stream resp = await message.Content.ReadAsStreamAsync();


            HtmlParser parser = new HtmlParser();

            IHtmlDocument doc = parser.ParseDocument(resp);

            AddGamesFromWebList(ExtractGames(doc));

        }


        private static Dictionary<string,string> ExtractGames(IHtmlDocument doc)
        {
            // key = Game Name, value = Game's App ID
            Dictionary<string, string> gamesList = new Dictionary<string, string>();

            IHtmlElement body = doc.Body;
            
            INodeList nodes = body.ChildNodes;
            
            foreach(INode node in nodes)
            {
                if (node.NodeName == "GAMESLIST") {
                    foreach (INode cnode in node.ChildNodes)
                    {
                        if (cnode.NodeName == "GAMES")
                        {
                            foreach (INode game in cnode.ChildNodes)
                            {
                                string appid = "";
                                string appname = "";

                                foreach (INode info in game.ChildNodes)
                                {
                                    
                                    
                                    switch (info.NodeName)
                                    {
                                        case "APPID":
                                            appid = info.TextContent;
                                            break;

                                        case "NAME":
                                            
                                            string comment = info.FirstChild.NodeValue;
                                            appname = comment.Substring(7, comment.Length - 9).ToLower();
                                            
                                            break;


                                        default:
                                            break;

                                    }

                                    
                                }

                                if (appid != "")
                                {
                                    gamesList.Add(appname, appid);
                                }
                            }
                        }
                    }
                }
            }

            return gamesList;
        
        }


        private static void AddGamesFromWebList(Dictionary<string, string> games)
        {
            foreach(string game in games.Keys)
            {
                string appid = games[game];

                if (!steamGames.ContainsValue(appid))
                {
                    steamGames.Add(game, appid);

                    

                    steamDictChanged = true;
                }
            }
        }


        private static Dictionary<string, string> ReadLocalSteamFile()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(LOCAL_STEAM));
        }

        private static void SaveLocalSteamFile()
        {
            string json = JsonConvert.SerializeObject(steamGames);
            File.WriteAllText(LOCAL_STEAM, json);
        }
    }
}
