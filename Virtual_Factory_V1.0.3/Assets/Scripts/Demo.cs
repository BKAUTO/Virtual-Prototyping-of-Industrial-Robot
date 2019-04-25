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
        public static string recvStr = "";
        public static byte[] recvBytes = new byte[1024];//机械臂数据内存缓冲区,1k
        public static byte[] recvBytes2 = new byte[1024];//其他设备内存缓冲区
        public static int bytes1;//接收机械臂数据
        public static int bytes2;//接收其他数据
        public static float[] variable = new float[6];
        private const int port = 8088;
        //private static string host = "127.0.0.1";
        private static Socket s;
        public static Socket temp;
        public static bool flag = false, flag2 = false, flag3 = false;
        public Text chatText;
        public ScrollRect scrollRect;
        public static string displayText;
        public static Socket[] lst = new Socket[10];
        private static int armSocketCount = 0;//机械臂创建两个socket客户端，一个用来发送关节角度给上位机，一个用来接收控制信号

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


            /*float[] variable = new float[6];
            decimal[] temp1 = new decimal[6];
            string[] temp = {"100000","100000","100000","100000","100000","100000"};
            int j = 0;
            int k = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    j++;
                    k = 0;
                }
                else
                {
                    char[] strr = temp[j].ToCharArray();
                    strr[k] = str[i];
                    k++;
                    temp[j] = new string(strr);
                }
            }
            for (int i = 0; i <= j; i++)
            {
                temp1[i] = Convert.ToDecimal(temp[i]);
            }
            for (int i = 0; i <= j; i++)
            {
                variable[i] = Convert.ToSingle(temp1[i]);
            }*/
            return vari;
        }

        private void ClientConnectListen()
        {
            while (true)
            {
                temp = s.Accept();
                if (temp.RemoteEndPoint.ToString().Contains("192.168.2.160"))//机械臂连接上
                {
                   if(armSocketCount == 0)
                    {
                        lst[0] = temp;
                        flag = true;
                        armSocketCount += 1;
                    }
                   else if(armSocketCount == 1)
                    {
                        lst[2] = temp;
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
                //string sendStr = "state\r";
                //byte[] bs = Encoding.UTF8.GetBytes(sendStr);
                //temp.Send(bs, bs.Length, 0);//发送信息给客户端

                //bytes = temp.Receive(recvBytes, recvBytes.Length, 0);
                //recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);
                //TextPrint(recvStr+"\n");
                /*if(temp.Poll(10, SelectMode.SelectRead) == true)
                {
                    temp.Close();
                    displayText = "\n" + "<color=red>" + "等待机械臂连接......\n" + "</color>";
                    flag2 = true;
                    flag = false;
                }*/
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

        /*void OnGUI()
        {
            if (GUI.Button(new Rect(240, 10, 120, 20), "Arm_Connect"))
            {
                string sendStr = "jnt_pos\r";
                byte[] bs = Encoding.UTF8.GetBytes(sendStr);
                temp.Send(bs, bs.Length, 0);//返回信息给客户端

                bytes = temp.Receive(recvBytes, recvBytes.Length, 0);
                recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);
                variable = GetVariable(recvStr);//
                print("server get message: " + recvStr + "\n");

                for(int i=0; i<6; i++)
                {
                    print(variable[i]+"\n");
                }
            }
        }*/

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

                    bytes1 = lst[0].Receive(recvBytes, recvBytes.Length, 0);
                    recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes1);
                    variable = GetVariable(recvStr);
                    //print("server get message: " + recvStr + "\n");
                    
                    //for (int i = 0; i < 6; i++)
                    //{
                    //print(variable[i] + "\n");
                    //}
                }
                catch
                {
                    Debug.Log("4444");
                    displayText = "\n" + "<color=red>" + "已断开，等待机械臂连接......\n" + "</color>";
                    flag2 = true;
                    flag = false;
                    armSocketCount = armSocketCount - 1;//armSocketCount = 0;
                }
                /*if (temp.Poll(-1, SelectMode.SelectRead))
                {
                    int nRead = temp.Receive(recvBytes, recvBytes.Length, 0);
                    if (nRead == 0)
                    {
                        displayText = "\n" + "<color=red>" + "等待机械臂连接......\n" + "</color>";
                        flag2 = true;
                    }
                }*/

                /*if(temp.Poll(10, SelectMode.SelectRead)==true)
                {
                    displayText = "\n" + "<color=red>" + "等待机械臂连接......\n" + "</color>";
                    flag2 = true;
                    flag = false;
                }*/
            }
            /*if (flag3 == true)
            {
                try
                {
                    //string sendStr = "";
                    //byte[] bs = Encoding.UTF8.GetBytes(sendStr);
                    //lst[1].Send(bs, bs.Length, 0);//发送信息给客户端
                    //bytes2 = lst[1].Receive(recvBytes, recvBytes.Length, 0);
                    //recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes2);
                    //print("server get message: " + recvStr + "\n");

                    //for (int i = 0; i < 6; i++)
                    //{
                    //print(variable[i] + "\n");
                    //}
                }
                catch
                {
                    displayText = "\n" + "<color=red>" + temp.RemoteEndPoint.ToString() + "已断开\n" + "</color>";
                    flag = false;
                }
            }*/
        }
    }
}