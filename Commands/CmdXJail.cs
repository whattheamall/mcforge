﻿namespace MCForge
{
    using System;
    using System.IO;
    public class CmdXJail : Command
    {
        public override string name { get { return "xjail"; } }
        public override string shortcut { get { return "xj"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xjail <player> - Mutes <player>, freezes <player> and sends <player> to the XJail map (shortcut = /xj)");
            Player.SendMessage(p, "If <player> is already jailed, <player> will be spawned, unfrozen and unmuted");
            Player.SendMessage(p, "/xjail set - Sets the map to be used for xjail to your current map and sets jail to current location");
        }
        public override void Use(Player p, string message)
        {
            string dir = "extra/jail/";
            string jailMapFile = dir + "xjail.map.xjail";
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            if (!File.Exists(jailMapFile))
            {
                StreamWriter SW = new StreamWriter(jailMapFile);
                SW.WriteLine(Server.mainLevel.name);
                SW.Close();
            }
            if (message == "") { Help(p); return; }
            else
            {
                StreamReader SR = new StreamReader(jailMapFile);
                string xjailMap = SR.ReadLine();
                SR.Close();
                Command jail = Command.all.Find("jail");
                if (message == "set")
                {
                    if (!p.level.name.Contains("cMuseum"))
                    {
                        jail.Use(p, "create");
                        StreamWriter SW = new StreamWriter(jailMapFile);
                        SW.WriteLine(p.level.name);
                        SW.Close();
                        Player.SendMessage(p, "The xjail map was set from '" + xjailMap + "' to '" + p.level.name + "'");
                        return;
                    }
                    else { Player.SendMessage(p, "You are in a museum!"); return; }
                }
                else
                {
                    Player player = Player.Find(message);
                    if (player != null)
                    {
                        Command move = Command.all.Find("move");
                        Command spawn = Command.all.Find("spawn");
                        Command freeze = Command.all.Find("freeze");
                        Command mute = Command.all.Find("mute");
                        string playerFile = dir + player.name + "_temp.xjail";
                        if (!File.Exists(playerFile))
                        {
                            StreamWriter writeFile = new StreamWriter(playerFile);
                            writeFile.WriteLine(player.level.name);
                            writeFile.Close();
                            if (!player.muted) { mute.Use(p, message); }
                            if (!player.frozen) { freeze.Use(p, message); }
                            move.Use(p, message + " " + xjailMap);
                            while (player.Loading)
                            {
                            }
                            if (!player.jailed) { jail.Use(p, message); }
                            Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was XJailed!");
                            return;
                        }
                        else
                        {
                            StreamReader readFile = new StreamReader(playerFile);
                            string playerMap = readFile.ReadLine();
                            readFile.Close();
                            File.Delete(playerFile);
                            move.Use(p, message + " " + playerMap);
                            while (player.Loading)
                            {
                            }
                            mute.Use(p, message);
                            jail.Use(p, message);
                            freeze.Use(p, message);
                            Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was released from XJail!");
                            return;
                        }
                    }
                    else { Player.SendMessage(p, "Player not found"); return; }
                }
            }
        }
    }
}