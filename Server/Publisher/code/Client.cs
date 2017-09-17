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

        //public void Watching()
        //{
        //    Socket connect = null;
        //    while (true)
        //    {
        //        try
        //        {
        //            connect = socketWatch.Accept();
        //        }
        //        catch(Exception e)
        //        {
        //            throw e;
        //        }

        //        IPAddress clientIP = (connect.RemoteEndPoint as IPEndPoint).Address;
        //        int clientPort = (connect.RemoteEndPoint as IPEndPoint).Port;

        //        string sendMsg = "连接服务端成功！\r\n" + "本地IP:" + clientIP + "，本地端口" + clientPort.ToString();
        //        byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
        //        connect.Send(arrSendMsg);

        //    }
        //}

        //private void SendMsg(string sendMsg,string name)
        //{
        //    //将string转换为byte 
        //    byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
        //    //向服务端返回确认信息
        //    dictionary[name].Send(arrClientSendMsg);
        //}
    }
}
