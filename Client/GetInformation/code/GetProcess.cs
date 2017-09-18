using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInformation.code
{
    public delegate void ShowProcessHandler(Process[] processes);
    public class GetProcess
    {
        public event ShowProcessHandler ShowPro;
        public void GetPro()
        {
            Process[] processes;
            processes = Process.GetProcesses();
            ShowPro?.Invoke(processes);
        }
    }
}
