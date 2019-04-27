/*Demo.cs备份*/
using System;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    class Demo : MonoBehaviour
    {
        public static string recvStr = "";//接收机械臂关节数据
        public static string recvStr2 = "";//机械臂控制信号
        public static byte[] recvBytes = new byte[1024];//机械臂关节数据内存缓冲区,1k
        public static byte[] recvBytes2 = new byte[1024];//其他设备内存缓冲区
        public static byte[] recvBytes3 = new byte[1024];//机械臂控制信号内存缓冲区
        public static int bytes1;//接收机械臂数据
        public static int bytes2;//接收其他数据
        public static int bytes3;//接收机械臂控制信号
        public static float[] variable = new float[6];
        private const int port = 8088;
        //private static string host = "127.0.0.1";
        private static Socket s;
        public static Socket temp;
        public static bool flag = false, flag2 = false, flag3 = false;//flag用来指示机械臂关节数据传输socket连接情况，flag2控制前端提示信息显示，flag3指示外设（电爪）连接情况
        public static bool flag4 = false;//flag4指示机械臂控制信号socket连接情况
        public Text chatText;
        public ScrollRect scrollRect;
        public static string displayText;
        public static Socket[] lst = new Socket[10];
        private static int armSocketCount = 0;//机械臂创建两个socket客户端，一个用来发送关节角度给上位机，一个用来接收控制信号
        public static double[] pose = new double[6] {295.157246157,-239.277773036,380.116285186,0.0,0.0,0.0};

        public void TextPrint(string addText)
        {
            chatText.text += addText;
            Canvas.ForceUpdateCanvases();       //关键代码
            scrollRect.verticalNormalizedPosition = 0f;  //关键代码
            Canvas.ForceUpdateCanvases();   //关键代码
        }

        public float[] GetVariable(string str)
        {
            string[] strr = str.Split(',');
            float[] vari = new float[6];
            for (int i = 0; i < 6; i++)
                vari[i] = Convert.ToSingle(strr[i]);
            return vari;
        }

        private void ClientConnectListen()
        {
            while (true)
            {
                temp = s.Accept();
                if (temp.RemoteEndPoint.ToString().Contains("192.168.2.160"))//机械臂连接上
                {
                   if(armSocketCount == 0)//机械臂数据传输socket
                    {
                        lst[0] = temp;
                        flag = true;
                        armSocketCount += 1;
                        Debug.Log("关节数据socket连接成功");
                    }
                   else if(armSocketCount == 1)//机械臂关节控制信号socket
                    {
                        lst[2] = temp;
                        flag4 = true;
                        Debug.Log("远程控制socket连接成功");
                    }
                }
                else if(flag == false)//没有连接机械臂而先连接了其他设备
                {
                    Debug.Log("2222");
                    displayText = "\n" + "<color=red>" + "请先连接机械臂,程序出错请重启" + "</color>" + "\n";
                    flag2 = true;
                    temp.Close();
                }
                else//其他设备连接上（电爪）
                {
                    Debug.Log("3333");
                    flag3 = true;
                    lst[3]=temp;
                }
                displayText = "\n" + "<color=black>" + "客户端 " + "</color>" + "<color=green>" + temp.RemoteEndPoint.ToString() + "</color>" + "<color=black>" + "已连接\n" + "</color>";
                flag2 = true;
            }
        }

        void Start()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            TextPrint("<color=black>" + "您当前的IP地址:" + "</color>" + "<color=blue>" + AddressIP + "</color>" + "\n");
            Debug.Log("Test");
            IPAddress ip = IPAddress.Parse(AddressIP);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(ipe);
            s.Listen(10);
            TextPrint("\n" + "<color=red>" + "正在等待机械臂连接......\n" + "</color>");
            Thread thread = new Thread(ClientConnectListen);
            thread.Start();
        }

      

        void Update()
        {
            if(flag2 == true)//仅能在主线程里调用UI组件，flag2用来显示信息，设置displayText后需将其设为true
            {
                TextPrint(displayText);
                flag2 = false;
            }
            if (flag == true)
            {
                try
                {
                    Debug.Log("5555");
                    string sendStr = "jnt_pos\r";
                    byte[] bs = Encoding.UTF8.GetBytes(sendStr);
                    lst[0].Send(bs, bs.Length, 0);//发送信息给客户端

                    bytes1 = lst[0].Receive(recvBytes, recvBytes.Length, 0);//接收关节数据
                    recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes1);
                    variable = GetVariable(recvStr);
                }
                catch
                {

                    Debug.Log("4444");
                    displayText = "\n" + "<color=red>" + "已断开，等待机械臂连接......\n" + "</color>";
                    flag2 = true;
                    flag = false;
                    armSocketCount = armSocketCount - 1;//armSocketCount = 0;
                }
            }
            if(flag4 == true)
            {
                try
                {
                    Debug.Log("开始接收控制信号");
                    bytes3 = lst[2].Receive(recvBytes3, recvBytes3.Length, 0);
                    recvStr2 = Encoding.UTF8.GetString(recvBytes3, 0, bytes3);
                    Debug.Log("接收到："+recvStr2);
                    /*if(recvStr2[0]=='1')
                    {
                        byte[] bs = Encoding.UTF8.GetBytes(pose[0]+ "\r");
                        lst[2].Send(bs, bs.Length, 0);//发送第一个角度给客户端
                        Debug.Log(pose[0]);

                        bs = Encoding.UTF8.GetBytes(pose[1] + "\r");
                        lst[2].Send(bs, bs.Length, 0);//第二个
                        Debug.Log(pose[1]);

                        bs = Encoding.UTF8.GetBytes(pose[2] + "\r");
                        lst[2].Send(bs, bs.Length, 0);//第三个
                        Debug.Log(pose[2]);

                        bs = Encoding.UTF8.GetBytes(pose[3] + "\r");
                        lst[2].Send(bs, bs.Length, 0);//第四个
                        Debug.Log(pose[3]);

                        bs = Encoding.UTF8.GetBytes(pose[4] + "\r");
                        lst[2].Send(bs, bs.Length, 0);//第五个
                        Debug.Log(pose[4]);

                        bs = Encoding.UTF8.GetBytes(pose[5] + "\r");
                        lst[2].Send(bs, bs.Length, 0);//第六个
                        Debug.Log(pose[5]);
                    }*/
                }
                catch
                {
                    displayText = "\n" + "<color=red>" + "joint运动控制信号传输失败......\n" + "</color>";
                    flag2 = true;
                }
            }
        }
    }
}