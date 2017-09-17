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

namespace ServerUI.code
{
    public partial class MainForm : Form
    {
        Publisher.code.Client client = new Publisher.code.Client();
        public MainForm()
        {
            InitializeComponent();
            client.SetM += new Publisher.code.SetMesHandler(SetLable);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.ClientAdd(IPAddress.Parse("127.0.0.1"),8085);
        }

        public void SetLable(string msg)
        {
            label1.Text = msg;
        }
    }
}
