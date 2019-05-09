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
        public static byte[] recvBytes2 = new byte[1024];//机械臂控制信号内存缓冲区
        public static byte[] recvBytes3 = new byte[1024];//其他设备内存缓冲区
        public static int bytes1;//接收机械臂数据
        public static int bytes2;//接收机械臂控制信号
        public static int bytes3;//接收其他数据
        public static float[] variable = new float[6];
        private const int port1 = 8088;
        private const int port2 = 8087;
        //private static string host = "127.0.0.1";
        private static Socket s1,s2;
        public static Socket temp1,temp2;
        public static bool flag = false, flag2 = false, flag3 = false;//flag用来指示机械臂关节数据传输socket连接情况，flag2控制前端提示信息显示，flag3指示外设（电爪）连接情况
        public static bool flag4 = false;//flag4指示机械臂控制信号socket连接情况
        public static bool flag5 = false;//flag5指示是否可以发送希望位姿给机械臂
        public Text chatText;
        public ScrollRect scrollRect;
        public static string displayText;
        public static Socket[] lst = new Socket[10];
        private static int armSocketCount = 0;//机械臂创建两个socket客户端，一个用来发送关节角度给上位机，一个用来接收控制信号及发送控制数据，使用两个端口
        public static double[] pose = new double[7] {295.157246157,-239.277773036,380.116285186,-0.01,0.7,0.05,0.6};

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

        private void ClientConnectListen1()
        {
            while (true)
            {
                temp1 = s1.Accept();
                if (temp1.RemoteEndPoint.ToString().Contains("192.168.2.160"))//机械臂连接上
                {
                   if(armSocketCount == 0)//机械臂数据传输socket
                    {
                        lst[0] = temp1;
                        flag = true;
                        armSocketCount += 1;
                        Debug.Log("关节数据socket连接成功");
                    }
                }
                else if(flag == false)//没有连接机械臂而先连接了其他设备
                {
                    Debug.Log("2222");
                    displayText = "\n" + "<color=red>" + "请先连接机械臂关节数据通道,程序出错请重启" + "</color>" + "\n";
                    flag2 = true;
                    temp1.Close();
                }
                else//其他设备连接上（电爪）
                {
                    Debug.Log("3333");
                    flag3 = true;
                    lst[3]=temp1;
                }
                displayText = "\n" + "<color=black>" + "客户端 " + "</color>" + "<color=green>" + temp1.RemoteEndPoint.ToString() + "</color>" + "<color=black>" + "已连接\n" + "</color>";
                flag2 = true;
            }
        }

        private void ClientConnectListen2()
        {
            while (true)
            {
                temp2 = s2.Accept();
                if (temp2.RemoteEndPoint.ToString().Contains("192.168.2.160"))//机械臂连接上
                {
                    if(armSocketCount == 1)//机械臂关节控制信号socket
                     {
                         lst[2] = temp2;
                         flag4 = true;
                         Debug.Log("远程控制socket连接成功");
                     }
                    if(armSocketCount == 0)
                    {
                        displayText = "\n" + "<color=red>" + "请先连接机械臂关节数据通道" + "</color>" + "\n";
                        flag2 = true;
                        temp2.Close();
                    }
                }
                displayText = "\n" + "<color=black>" + "客户端 " + "</color>" + "<color=green>" + temp2.RemoteEndPoint.ToString() + "</color>" + "<color=black>" + "已连接\n" + "</color>";
                flag2 = true;
            }
        }

        private void ControlDataHandler()//线程方法，用来接收机械臂传来的控制信号
        {
            while (true)
            {
                if(flag4 == true)
                {
                    bytes2 = 0;
                    bytes2 = lst[2].Receive(recvBytes2, recvBytes2.Length, 0);
                    flag5 = true;//理论上接收到1时才能使flag5 = true即使期望位姿发送至机械臂，但未能实现，
                    //虽不影响远程控制的使用但程序逻辑有问题，希望后续开发者修改
                    Debug.Log("接收到控制信号");
                    recvStr2 = Encoding.UTF8.GetString(recvBytes2, 0, bytes2);
                    Debug.Log("接收到：" + recvStr2);
                }
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

            IPAddress ip1 = IPAddress.Parse(AddressIP);
            IPEndPoint ipe1 = new IPEndPoint(ip1, port1);

            IPAddress ip2 = IPAddress.Parse(AddressIP);
            IPEndPoint ipe2 = new IPEndPoint(ip2, port2);

            s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s1.Bind(ipe1);
            s1.Listen(10);
            TextPrint("\n" + "<color=red>" + "正在等待关节数据通道连接......\n" + "</color>");
            Thread thread1 = new Thread(ClientConnectListen1);
            thread1.Start();

            s2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s2.Bind(ipe2);
            s2.Listen(10);
            TextPrint("\n" + "<color=red>" + "正在等待远程控制通道连接......\n" + "</color>");
            Thread thread2 = new Thread(ClientConnectListen2);
            thread2.Start();

            Thread thread3 = new Thread(ControlDataHandler);//该线程接收机械臂发来的控制信号，指示上位机可以发送位姿
            thread3.Start();
        }

      
        void Update()
        {  
            if (flag2 == true)//仅能在主线程里调用UI组件，flag2用来显示信息，设置displayText后需将其设为true
            {
                TextPrint(displayText);
                flag2 = false;
            }
            if (flag == true)
            {
                try
                {
                    //Debug.Log("5555");
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
                    if(flag5==true)
                    {
                        string pos1 = Convert.ToString(pose[0]);
                        string pos2 = Convert.ToString(pose[1]);
                        string pos3 = Convert.ToString(pose[2]);
                        string pos4 = Convert.ToString(pose[3]);
                        string pos5 = Convert.ToString(pose[4]);
                        string pos6 = Convert.ToString(pose[5]);
                        string pos7 = Convert.ToString(pose[6]);
                        string sendPos = pos1 + "," + pos2 + "," + pos3 + "," + pos4 + "," + pos5 + "," + pos6 + "," + pos7 + "\r";
                        byte[] pp = Encoding.UTF8.GetBytes(sendPos);
                        lst[2].Send(pp, pp.Length, 0);//发送位姿信息给客户端
                        Debug.Log(pose[0]);
                        Debug.Log(pose[1]);
                        Debug.Log(pose[2]);
                        Debug.Log(pose[3]);
                        Debug.Log(pose[4]);
                        Debug.Log(pose[5]);
                        Debug.Log(pose[6]);
                        flag5 = false;
                    }
                    else
                    {
                        Debug.Log("不在发送位姿时间内");
                    }
                }
                catch
                {
                    displayText = "\n" + "<color=red>" + "joint运动控制信号传输失败......\n" + "</color>";
                    flag2 = true;
                    flag4 = false;
                }
            }
        }
    }
}