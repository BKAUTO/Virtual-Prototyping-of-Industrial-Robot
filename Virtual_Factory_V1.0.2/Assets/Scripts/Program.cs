using System;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Threading;
using Chat;
using Handler;

namespace Pro
{
    public class Program : MonoBehaviour
    {
        //设置连接端口
        const int portNo = 8088;
        //初始化
        void Start()
        {
            Thread myThread = new Thread(ListenClientConnect);//开启协程
            myThread.Start();
        }

        void Update()
        {

        }

        private void ListenClientConnect()
        {
            //初始化服务器IP
            IPAddress localAdd = IPAddress.Parse("192.168.2.125");
            //创建TCP侦听器
            TcpListener listener = new TcpListener(localAdd, portNo);
            listener.Start();
            //显示服务器启动信息
            //oldstr = String.Concat("正在启动服务器!");
            //textshow.text = oldstr;
            //("Server is starting...\n");
            //循环接受客户端的连接请求
            while (true)
            {
                ChatClient user = new ChatClient(listener.AcceptTcpClient());
                //显示连接客户端的IP与端口
                if (user._clientIP.Contains("160"))
                {
                    ChatClient.Broadcast("机械臂加入服务器\n");
                }
                else
                    ChatClient.Broadcast(user._clientIP + "加入服务器\n");
            }
        }
    }
}