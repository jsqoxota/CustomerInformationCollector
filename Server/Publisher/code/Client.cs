using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher.code
{
    public delegate void SetMesHandler(string message);
    public class Client
    {
        Socket serverSocket = null;
        public event SetMesHandler SetM;
        Thread receiveThread;

        Dictionary<string, Socket> dictionary = new Dictionary<string, Socket>();

        public void ClientAdd(IPAddress address,int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(address, port));//绑定ip地址端口号
            serverSocket.Listen(10);//等待连接队列 数量
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                string remoteEndPoint = clientSocket.RemoteEndPoint.ToString();
                dictionary.Add(remoteEndPoint, clientSocket);
                clientSocket.Send(Encoding.ASCII.GetBytes("send message"));
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private  void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                { 
                    byte[] result = new byte[100];
                    int length = myClientSocket.Receive(result);
                    SetM?.Invoke(Encoding.UTF8.GetString(result));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
