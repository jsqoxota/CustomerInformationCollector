using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        GetInformation.code.GetProcess getPro = new GetInformation.code.GetProcess();


        public MainForm()
        {
            InitializeComponent();
            receive.SetM += new Receive.code.SetMesHandler(SetLable);
            getPro.ShowPro += new GetInformation.code.ShowProcessHandler(ShowProcess);
        }

        public void SetLable(string msg)
        {
            label1.Text = msg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            receive.RecInit(IPAddress.Parse("127.0.0.1"), 8085);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            getPro.GetPro();
        }

        private void ShowProcess(Process[] processes)
        {
            this.listView1.Columns.Add("Name",100);
            this.listView1.Columns.Add("PID",100);
            this.listView1.Columns.Add("MemorySize", 100);
            foreach (Process process in processes)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = process.ProcessName;
                listViewItem.SubItems.Add(process.Id.ToString());
                listViewItem.SubItems.Add((process.PrivateMemorySize64/1024).ToString()+"K");
                listView1.Items.Add(listViewItem);
            }
        }
    }
}
