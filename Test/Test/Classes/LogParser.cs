using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Text.RegularExpressions;

namespace Test.Classes
{
    class LogParser
    {

        public const int ACTION_TYPE_START_POS = 57;
        private const String LOG_PATTERN = "GwentClient-*.log*";

        private String log_folder;

        private long last_log_length;

        private PlayerWindow user, opponent, player_one, player_two;

        public LogParser(PlayerWindow user, PlayerWindow opponent)
        {
            this.user = user; this.opponent = opponent;

            this.log_folder =  String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Low\CDProjektRED\Gwent\");
           
            last_log_length = 0;
            Timer timer = new Timer(1000);
            timer.Elapsed += CheckLogChange;
            timer.Start();


        }


        public FileInfo GetLatestLogFile()
        {
            var dirInfo = new DirectoryInfo(log_folder);

            var latest_log = (from f in dirInfo.GetFiles(LOG_PATTERN) orderby f.LastWriteTime descending select f).First();

            return latest_log;
        }

        private void CheckLogChange(Object source, ElapsedEventArgs e)
        {
            string new_lines = string.Empty;
            FileInfo log = GetLatestLogFile();


            StreamReader reader = new StreamReader(new FileStream(log.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

            // Just skip if log file hasn't changed
            if (!(last_log_length == log.Length))
            {
                using (reader)
                {
                    // Set the position to the last log size and read
                    // all the content added
                    reader.BaseStream.Position = last_log_length;
                    new_lines = reader.ReadToEnd();
                }

                // Keep track of the previuos log length
                last_log_length = log.Length;

                //Send new lines to parser

                Parse(new_lines);

            }
        }

        public String GetUserName()
        {
            return "ataalik";
        }

        public void Parse(String to_parse)
        {

            //Split the input by lines
            string[] lines = to_parse.Split(new[] { '\r', '\n' });

            foreach (string line in lines)
            {
                if (line.Contains("ActionManager"))
                {
                    Console.WriteLine("lol");

                    String actionType = "";

                    int i = ACTION_TYPE_START_POS;
                    while (line[i] != ' ')
                    {
                        i = i + 1;
                    }

                    //Get action Type. There might be a better way to do this
                    actionType = line.Substring(ACTION_TYPE_START_POS, i - ACTION_TYPE_START_POS);

                    if (actionType.Equals("SetPlayerDeckAction")){

                        //This is where we get our decks contents. This line for the enemy only contains their leader.
                        Console.WriteLine("SetPlayerDeckAction");

                        Regex no_regex = new Regex("Player: (?<no>P[1-2])\\[");

                        Regex leader_regex = new Regex(" Leader: \\[TemplateID: (?<leader>[0-9]*),");
                        Regex non_leader_card = new Regex("(?<!Leader: )\\[TemplateID: (?<id>[0-9]*),");

                        Match match = no_regex.Match(line);
                        if (match.Success)
                            Console.WriteLine("Player no {0}",match.Groups["no"].Value);

                        match = leader_regex.Match(line);
                        if (match.Success)
                        {
                            Console.WriteLine("leader id {0}", match.Groups["leader"].Value);
                        }

                        MatchCollection matches = non_leader_card.Matches(line);

                        foreach (Match card in matches)
                        {
                            Console.WriteLine("card ID {0}", card.Groups["id"].Value);
                        }


                    }
                    else if (actionType.Equals("SetupPlayerAction")){

                        
                        Console.WriteLine("SetupPlayerAction");

                        Match match;
                        PlayerInfo playerToAdd;
                        String no = String.Empty;
                        String name = String.Empty;
                        byte level = 0;
                        byte rank = 0;
                        int mmr = 0;

                        //General Player Info
                        Regex no_regex = new Regex("Player: P(?<no>[1-2]),"); //Player no as in P{1} or P{2}
                        Regex name_regex = new Regex("Name: (?<name>[a-zA-Z0-9 ]*),");//Players GoG name
                        Regex level_regex = new Regex("Level: (?<level>[0-9]*),");//Players experience level
                        Regex rank_regex = new Regex("Rank: (?<rank>[0-9]*),");//Players rank (0 indexed)
                        Regex avatar_regex = new Regex("Avatar: (?<avatar>[0-9]*),");//Players avatar id
                        Regex border_regex = new Regex("Border: (?<border>[0-9]*),");//Players border id 
                        Regex title_regex = new Regex("Title: (?<title>[0-9]*),");//Players title id 

                        //TODO: MMR is supposed to be here but it is not consistently written to log. It can be acquired with an API call 


                        match = no_regex.Match(line);
                        if (match.Success)
                            no = match.Groups["no"].Value;

                        match = name_regex.Match(line);
                        if (match.Success)
                            name = match.Groups["name"].Value;
                        match = level_regex.Match(line);
                        if (match.Success)
                            level = Byte.Parse(match.Groups["level"].Value);
                        match = rank_regex.Match(line);
                        if (match.Success)
                            rank = Byte.Parse(match.Groups["rank"].Value);
                        match = avatar_regex.Match(line);
                        if (match.Success)
                            Console.WriteLine("Avatar ID {0}", match.Groups["avatar"].Value);
                        match = border_regex.Match(line);
                        if (match.Success)
                            Console.WriteLine("Border ID {0}", match.Groups["border"].Value);
                        match = title_regex.Match(line);
                        if (match.Success)
                            Console.WriteLine("Title ID {0}", match.Groups["title"].Value);

                        //If the players name is equal to users name
                        if (name.Equals(GetUserName()))
                        {
                            playerToAdd = new PlayerInfo(name, level, rank, mmr, Enums.PlayerType.BLUE);
                            user.SetPlayer(playerToAdd);

                            //Assign P1-2 variables
                            if (no.Equals("1"))
                            {
                                player_one = user;
                                player_two = opponent;
                            }
                            else
                            {
                                player_one = opponent;
                                player_two = user;
                            }
                        }else
                        {
                            playerToAdd = new PlayerInfo(name, level, rank, mmr, Enums.PlayerType.RED);
                            if (no.Equals("1"))
                            {
                                player_one = opponent;
                                player_two = user;

                            }
                            else
                            {
                                player_one = user;
                                player_two = opponent;
                            }
                        }
                    }
                    else if (actionType.Equals("SpawnCardsAction"))
                    {


                        Regex spawn_card_regex = new Regex(@"\[CardID: (?<cardid>[0-9]*), Definition: \[TemplateID: (?<templateid>[0-9]*), [a-zA-Z: \],]*Position: \[PlayerId: (?<no>P[1-2]), Location: (?<location>[a-zA-Z]*), Index: (?<index>[-0-9]*)\]. SpawnLocation: (?<spawnloc>[a-zA-Z]*), SpawnType: (?<type>[a-zA-Z]*), Spawner: (?<spawner>([0-9]|.*))");

                        Console.WriteLine("SpawnCardsAction");
                    
                        MatchCollection matches = spawn_card_regex.Matches(line);
                        foreach(Match match in matches)
                        {
                            //If spawner is 0 this is the initial spawning of the decks. This is where we can get enemy deck size.
                            if (match.Groups["spawner"].Value.Equals("0"))
                            {
                                Console.WriteLine("+1 to {0}", match.Groups["no"]);
                            }
                        }
                    }
                }
            }


        }

    }
}
