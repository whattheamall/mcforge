﻿/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Starter
{
    class Program
    {
        // Attempt to read the location from the DLL, if available.
        public static string DLLLocation
        {
            get
            {
                return "http://www.mcforge.net/MCForge_.dll";
            }
        }

        static void Main(string[] args)
        {
            int tries = 0;
    retry:
            if (tries > 4)
            {
                Console.WriteLine("I'm afraid I can't download the file for some reason!");
                Console.WriteLine("Go to " + DLLLocation + " yourself and download it, please");
                Console.WriteLine("Place it inside my folder, near me, and restart me.");
                Console.WriteLine("If you have any issues, get the files from the www.mcforge.net download page and try again.");
                Console.WriteLine("Press any key to close me...");
                Console.ReadLine();
                goto exit;
            }

            if (File.Exists("MCForge_.dll"))
            {
                openServer(args);
            }
            else
            {
                tries++;
                Console.WriteLine("This is try number " + tries);
                Console.WriteLine("You do not have the required DLL!");
                Console.WriteLine("I'll download it for you. Just wait.");
                Console.WriteLine("Downloading from " + DLLLocation);

                WebClient Client = new WebClient();
                Client.DownloadFile(DLLLocation, "MCForge_.dll");
                Client.Dispose();

                Console.WriteLine("Finished downloading! Let's try this again, shall we.");
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(".");
                }
                Console.WriteLine("Go!");
                Console.WriteLine();

                goto retry;
            }
exit:   Console.WriteLine("Bye!");
        }

        static void openServer(string[] args)
        {
            MCForge_.Gui.Program.Main(args);
        }
    }
}
