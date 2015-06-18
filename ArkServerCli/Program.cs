using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using ArkServerCore;

namespace ArkServerCli
{
    class Program
    {
        private static bool ExitRequested { get; set; }
        private readonly static ArkServerCore.Server ArkServer = new ArkServerCore.Server();
        static void Main(string[] args)
        {
            Console.WriteLine("ArkServeCli: type Help for command list");
            while (!ExitRequested)
            {
                string command =Console.ReadLine();
                ParseCommand(command);
            }
        }

        private static void ParseCommand(string command)
        {
            if (command.ToLower() == "help") Help();
            if (command.ToLower() == "settings") Settings();
            if (command.ToLower() == "update") Update();
            if (command.ToLower() == "start") Start();
            if (command.ToLower() == "stop") Stop();
            if (command.ToLower() == "quit") Quit();
        }

        private static void Update()
        {
            Console.WriteLine("please wait for server Update to complete");
            ArkServer.Update();
            while (!ArkServer.SteamProcess.HasExited)
            {
                Console.Write("|");
                Thread.Sleep(2000);
            }
            Console.WriteLine("Complete");
        }

        private static void Start()
        {
            if (ArkServer.ServerProcess != null) Console.WriteLine("server is already active");
            else
            {
                ArkServer.Start();
                Console.WriteLine("starting server: " + ArkServer.Settings.SessionName + " on port " + ArkServer.Settings.QueryPort);
            }         
        }

        private static void Stop()
        {
            ArkServer.Close();   
        }

        private static void Quit()
        {
            ArkServer.SaveSettings();
            ArkServer.Close();
            Environment.Exit(0);
        }

        private static void Help()
        {
            Console.WriteLine("Valid commands:");
            Console.WriteLine("Settings - used to alter the server settings");
            Console.Write("Update - used to install and update the server");
            Console.WriteLine("Start - used to start the server");
            Console.WriteLine("Stop - used to stop the server");
            Console.WriteLine("Quit - used to shut down ArkServerCli");
        }

        private static void Settings()
        {
            Console.WriteLine("you will be asked a series of settings questions, this is saved for the future.");
            Console.WriteLine("Server Name:");
            ArkServer.Settings.SessionName = Console.ReadLine();
            ArkServer.Settings.QueryPort = ConsoleValueToInt("Port:");
            Console.WriteLine("Server Password: (leave blank if you want no password)");
            ArkServer.Settings.ServerPassword = Console.ReadLine();
            Console.WriteLine("Admin Password:");
            ArkServer.Settings.ServerAdminPassword = Console.ReadLine();
            ArkServer.Settings.MaxPlayers = ConsoleValueToInt("Max Players: (127 maximum)");
            ArkServer.Settings.Difficulty = ConsoleValueToDecimal("Difficulty: (decimal between 0 and 1 only. 0 is normal difficulty)");
            ArkServer.Settings.MaxStructuresInRange = ConsoleValueToInt("Max Structures in Range: (1300 seems to be default)");
            Console.WriteLine("boolean settings. answer with True or False.");
            ArkServer.Settings.ServerPVE = ConsoleValueToBool("Pve Server:");
            ArkServer.Settings.ServerHardcore = ConsoleValueToBool("Hardcore:");
            ArkServer.Settings.ServerCrosshair = ConsoleValueToBool("Crosshairs:");
            ArkServer.Settings.ServerForceNoHud = ConsoleValueToBool("Force No Hud:");
            ArkServer.Settings.GlobalVoiceChat = ConsoleValueToBool("Global Voice Chat:");
            ArkServer.Settings.NoTributeDownloads = ConsoleValueToBool("No Tribute Downloads:");
            ArkServer.Settings.AllowThirdPersonPlayer = ConsoleValueToBool("Allow Third Person:");
            ArkServer.Settings.AllowsNotifyPlayerLeft = ConsoleValueToBool("Notify Player Left:");
            ArkServer.Settings.DontAlwaysNotifyPlayerJoined = ConsoleValueToBool("Dont Always Notify Player Joined:");
            ArkServer.Settings.MapPlayerLocation = ConsoleValueToBool("Enable Player Location on Maps:");
            ArkServer.Settings.bDisableStructureDecayPve = ConsoleValueToBool("disable Structure Decay for PVE");
            ArkServer.Settings.bAllowFlyerCarryPve = ConsoleValueToBool("Allow Flyer Carry in PVE");
            ArkServer.Settings.EnablePvpGama = ConsoleValueToBool("Enable Gama in PVP");
            Console.WriteLine("Settings finished.");
        }

        private static int ConsoleValueToInt(string description)
        {
            int tempint = new int();
            bool validInput = new bool();    
            while (!validInput)
            {
                Console.WriteLine(description);
                try
                {                
                    tempint = Convert.ToInt32(Console.ReadLine());
                    validInput = true;
                }
                catch
                {
                    Console.WriteLine("Invalid Input, try again:");
                }
            }
            return tempint;
        }

        private static decimal ConsoleValueToDecimal(string description)
        {
            decimal tempDecimal = new decimal();
            bool validInput = new bool();
            while (!validInput)
            {
                Console.WriteLine(description);
                try
                {
                    tempDecimal = decimal.Parse(Console.ReadLine());
                    validInput = true;
                }
                catch
                {
                    Console.WriteLine("Invalid Input, try again:");
                }
            }
            return tempDecimal;
        }

        private static bool ConsoleValueToBool(string description)
        {
            bool tempBool = new bool();
            bool validInput = new bool();
            while (!validInput)
            {
                Console.WriteLine(description);
                try
                {
                    tempBool = bool.Parse(Console.ReadLine());
                    validInput = true;
                }
                catch
                {
                    Console.WriteLine("Invalid Input, try again: make sure to type true or false.");
                }
            }
            return tempBool;
        }
    }
}
