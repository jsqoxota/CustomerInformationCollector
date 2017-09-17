using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUI.code
{
    public partial class MainForm : Form
    {

        Receive.code.ReceiveMsg receive = new Receive.code.ReceiveMsg();



        public MainForm()
        {
            InitializeComponent();
            receive.SetM += new Receive.code.SetMesHandler(SetLable);
        }

        public void SetLable(string msg)
        {
            label1.Text = msg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            receive.RecInit(IPAddress.Parse("127.0.0.1"), 8085);
        }
    }
}
