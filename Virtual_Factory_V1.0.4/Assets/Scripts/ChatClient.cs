using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Net;
using System.Threading;
using UnityEngine.UI;
using System.Text;

namespace Chat
{
    public class ChatClient : MonoBehaviour
    {
        public static Hashtable ALLClients = new Hashtable();//客户列表
        public TcpClient _client; //客户端实体
        public string _clientIP;   //客户端IP
        private string _clientNick; //客户端昵称
        private byte[] data;    //消息数据
        public static float[] variable = new float[6];
        private bool ReceiveNick = true;

        public ChatClient(TcpClient client)//构造方法
        {
            this._client = client;
            this._clientIP = client.Client.RemoteEndPoint.ToString();
            ALLClients.Add(this._clientIP, this);//将当前客户端实例添加进客户端列表
            data = new byte[this._client.ReceiveBufferSize];
            client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);//从服务端读取数据
        }

        //从客户端获取消息
        public void ReceiveMessage(IAsyncResult ar)
        {
            //Broadcast("Test");
            int bytesRead;
            try
            {
                lock (this._client.GetStream())
                {
                    bytesRead = this._client.GetStream().EndRead(ar);
                }
                if (bytesRead < 1 && !this._clientIP.Contains("160"))
                {
                    ALLClients.Remove(this._clientIP);
                    Broadcast(this._clientIP + "已经离开服务器");//已经离开服务器
                    return;
                }
                else
                {
                    string messageReceived = Encoding.UTF8.GetString(data, 0, bytesRead);
                    if (ReceiveNick)
                    {
                        if (this._clientIP.Contains("160"))
                            Broadcast("机械臂已连入服务器");
                        else
                        {
                            this._clientNick = messageReceived;
                            Broadcast(this._clientNick + "已经进入服务器");//已经进入服务器
                        }
                        //this.sendMessage("hello");
                        ReceiveNick = false;
                    }
                    else
                    {
                        if (this._clientIP.Contains("160"))
                        {
                            if(((int)messageReceived[0]!=116) & ((int)messageReceived[0] != 102))
                            {
                                variable = GetVariable(messageReceived);
                                Broadcast("机械臂" + ">>>>" + messageReceived);
                            }
                            else
                                Broadcast("机械臂" + ">>>>" + messageReceived);
                        }
                        else
                            Broadcast(this._clientNick + ">>>>" + messageReceived);
                    }
                }
                lock (this._client.GetStream())
                {
                    this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception ex)
            {
                ALLClients.Remove(this._clientIP);
                Broadcast(this._clientIP + "已经离开服务器");//已经离开服务器
            }
        }

        //向客户端发送消息
        public void sendMessage(string message)
        {
            try
            {
                System.Net.Sockets.NetworkStream ns;
                lock (this._client.GetStream())
                {
                    ns = this._client.GetStream();
                }
                //对信息进行编码
                byte[] bytesToSend = Encoding.UTF8.GetBytes(message);
                ns.Write(bytesToSend, 0, bytesToSend.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log("Error:" + ex);
            }
        }

        //向客户端广播消息
        public static void Broadcast(string message)
        {
            //oldstr = message+"\n";
            print(message);//打印消息

            foreach (DictionaryEntry c in ALLClients)
            {
                ((ChatClient)(c.Value)).sendMessage(message + Environment.NewLine);
            }
        }

        public string ByteTransfer(byte[] data)
        {
            string str = System.Text.Encoding.UTF8.GetString(data);
            return str;
        }

        public float[] GetVariable(string str)
        {
            float[] variable = new float[6];
            decimal[] temp1 = new decimal[6];
            string[] temp = new string[50];
            int j = 0;
            int k = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] == 44)
                {
                    j++;
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
            for(int i=0; i<=j; i++)
            {
                variable[i] = Convert.ToSingle(temp1[i]);
            }
            return variable;
        }

        void Update()
        {

        }
    }
}
