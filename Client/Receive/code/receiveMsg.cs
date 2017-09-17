using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Receive.code
{
    public delegate void SetMesHandler(string message);

    public class ReceiveMsg
    {
        Thread threadClient = null;
        Socket socketClient = null;
        public event SetMesHandler SetM;

        public void RecInit(IPAddress adress, int port)
        {
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(adress, port);

            try
            {
                socketClient.Connect(point);
            }
            catch (Exception e)
            {
                throw e;
            }

            threadClient = new Thread(RecM)
            {
                IsBackground = true
            };
            threadClient.Start();
        }

        public void RecM()
        {
            while (true)
            {
                try
                {
                    //容器:暂存来自服务端的信息
                    byte[] msg = new byte[100];

                    //存入信息+获得信息长度
                    int length = socketClient.Receive(msg);

                    //msg转换成字符串
                    string strRevMsg = Encoding.UTF8.GetString(msg, 0, length);

                    //比较获得的指令，并回复
                    if ("send message".Equals(strRevMsg))
                    {
                        SetM?.Invoke(strRevMsg);
                        ClientSendMsg("OK");
                        return;
                    }
                    else
                    {
                        SetM?.Invoke(strRevMsg);
                        ClientSendMsg("error");
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void ClientSendMsg(string sendMsg)
        {
            //将string转换为byte 
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //向服务端返回确认信息
            socketClient.Send(arrClientSendMsg);
        }

    }
}

