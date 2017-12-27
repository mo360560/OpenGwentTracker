using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Test.Classes
{
    class LogParser
    {

        public const int ACTION_TYPE_START_POS = 57;
        private const String LOG_PATTERN = "GwentClient-*.log*";

        private String log_folder;
        private FileInfo latest_log;

        public LogParser()
        {
            log_folder =  String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Low\CDProjektRED\Gwent\");
            latest_log = GetLatestLogFile();

            
        }


        public FileInfo GetLatestLogFile()
        {
            var dirInfo = new DirectoryInfo(log_folder);

            var latest_log = (from f in dirInfo.GetFiles(LOG_PATTERN) orderby f.LastWriteTime descending select f).First();

            return latest_log;
        }

        public void Parse()
        {


            string[] lines = System.IO.File.ReadAllLines(String.Concat(log_folder,latest_log.ToString()));

            foreach (string line in lines)
            {
                if (line.Contains("ActionManager") && !line.Contains("HighlightAction") && !line.Contains("SetPlayerStatusAction"))
                {
                    String actionType = "";

                    int i = ACTION_TYPE_START_POS;
                    while (line[i] != ' ')
                    {
                        i = i + 1;
                    }

                    actionType = line.Substring(ACTION_TYPE_START_POS, i - ACTION_TYPE_START_POS);

                    switch (actionType)
                    {
                        case "SetPlayerDeckAction":
                            Console.WriteLine(line);
                            break;
                        case "SetupPlayerAction":
                            Console.WriteLine(line);
                            break;

                        default:
                            break;

                    }
                }
            }


        }

    }
}
