using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Test.Classes
{
    class LogParser
    {

        public const int ACTION_TYPE_START_POS = 57;
        private const String LOG_PATTERN = "GwentClient-*.log*";

        private String log_folder;

        private long last_log_length;

        public LogParser()
        {
            log_folder =  String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Low\CDProjektRED\Gwent\");
           
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

                // Assign the result back to the worker, to be
                // consumed by the form

                Parse(new_lines);

            }
        }

        public void Parse(String to_parse)
        {


            string[] lines = to_parse.Split(new[] { '\r', '\n' });

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
                            Debug.WriteLine(line);
                            break;
                        case "SetupPlayerAction":
                            Debug.WriteLine(line);
                            break;

                        default:
                            break;

                    }
                }
            }


        }

    }
}
