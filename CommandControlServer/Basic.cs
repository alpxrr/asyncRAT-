//-- implements the Basic Tab --

using System;

using System.Text;

using System.Threading; //to run commands concurrently
using System.Net; //for IPEndPoint
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace CommandControlServer
{
    internal class Basic
    {
        static TcpListener tcpListener;
        static Socket socketForClient;
        static NetworkStream networkStream;
        static StreamWriter streamWriter;
        static StreamReader streamReader;
        static StringBuilder strInput;
        static Thread th_StartListen, th_RunServer;

        public static Form1 frm = null;
        public static ToolStripStatusLabel tsl = null;
        public static TextBox tb1 = null;
        public static TextBox tb2 = null;

        //Commands in enumeration format:
        private enum command
        {
            HELP = 1,
            MESSAGE = 2,
            BEEP = 3,
            PLAYSOUND = 4,
            SHUTDOWNCLIENT = 5
        }

        public static void initBasic(Form1 frmIn,ToolStripStatusLabel tslIn, TextBox tb1In, TextBox tb2In)
        {
            frm = frmIn;
            tsl = tslIn;
            tb1 = tb1In;
            tb2 = tb2In;
            tb2.Focus();
        }

        public static void startThreads()
        {
           
            th_StartListen = new Thread(new ThreadStart(StartListen));
            th_StartListen.Start();
        }

        private static void StartListen()
        {
            tcpListener = new TcpListener(System.Net.IPAddress.Any, 6666);
            tcpListener.Start();
            tsl.Text = "Listening on port 6666 ...";
            for (; ; )
            {
                socketForClient = tcpListener.AcceptSocket();
                IPEndPoint ipend = (IPEndPoint)socketForClient.RemoteEndPoint;
                tsl.Text = "Connection from " + IPAddress.Parse(ipend.Address.ToString());
                th_RunServer = new Thread(new ThreadStart(RunServer));
                th_RunServer.Start();
            }
        }

        private static void RunServer()
        {
            networkStream = new NetworkStream(socketForClient);
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
            strInput = new StringBuilder();

            
            while (true)
            {
                try
                {
                    strInput.Append(streamReader.ReadLine());
                    strInput.Append("\r\n");
                    tb1.AppendText(strInput.ToString());
                    strInput.Remove(0, strInput.Length);
                }
                catch (Exception)
                {
                    
                }
                Application.DoEvents();
                
                
            }
        }

        private static void Cleanup()
        {
            try
            {
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                socketForClient.Close();
            }
            catch (Exception) { }
            tsl.Text = "Connection Lost";
        }

        public static void tb2_keydown(KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    strInput.Append(tb2.Text.ToString());
                    streamWriter.WriteLine(strInput);
                    streamWriter.Flush();
                    strInput.Remove(0, strInput.Length);
                    if (tb2.Text == "exit") Cleanup();
                    if (tb2.Text == "terminate") Cleanup();
                    if (tb2.Text == "cls") tb1.Text = "";
                    tb2.Text = "";
                }
            }
            catch (Exception err) { }
        }

        public static void btn1_click()
        {
            streamWriter.WriteLine("" + (int)command.MESSAGE); 
            streamWriter.Flush();
        }

        public static void btn2_click()
        {
            streamWriter.WriteLine("" + (int)command.BEEP);
            streamWriter.Flush();
        }

        public static void btn3_click()
        {
            streamWriter.WriteLine("" + (int)command.PLAYSOUND);
            streamWriter.Flush();
        }

        public static void btn4_click()
        {
            streamWriter.WriteLine("" + (int)command.SHUTDOWNCLIENT);
            streamWriter.Flush();
            tsl.Text = "Client has been shut down";
        }

        public static void frm_closing()
        {
            Cleanup();
            System.Environment.Exit(0);
        }
    }
}
