using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using Chat;
using Pro;

namespace Handler
{
    public class ClientHandler : MonoBehaviour
    {

        const int portNo = 8088;
        private TcpClient _client;
        private byte[] data;

        public string nickName = "";
        public string message = "";
        public string sendMsg = "";
        // Use this for initialization
        void OnGUI()
        {
            nickName = GUI.TextField(new Rect(10, 10, 100, 20), nickName);
            message = GUI.TextArea(new Rect(10, 40, 300, 200), message);
            sendMsg = GUI.TextField(new Rect(10, 250, 210, 20), sendMsg);

            if(GUI.Button(new Rect(240, 10, 120, 20), "Arm_Connect"))
            {
                //Debug.Log("hello");
                IPAddress localAdd = IPAddress.Parse("192.168.2.160");
                IPEndPoint ipendpoint = new IPEndPoint(localAdd, 8088);
                this._client = new TcpClient(ipendpoint);
                this._client.Connect("192.168.2.125", portNo);
                data = new byte[this._client.ReceiveBufferSize];
                //SendMyMessage(txtNick.Text);
                SendMyMessage("");
                this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
            }

            if(GUI.Button(new Rect(120, 10, 80, 20), "Connect"))
            {
                this._client = new TcpClient();
                this._client.Connect("192.168.2.125", portNo);
                data = new byte[this._client.ReceiveBufferSize];

                //SendMyMessage(txtNick.Text);
                SendMyMessage(nickName);
                this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
            }

            if (GUI.Button(new Rect(230, 250, 80, 20), "Send"))
            {
                SendMyMessage(sendMsg);
                sendMsg = "";
            }
        }

        /// <summary>
        /// 向服务器发送数据（发送聊天信息）
        /// </summary>
        /// <param name="message"></param>
        public void SendMyMessage(string message)
        {
            try
            {
                NetworkStream ns = this._client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log("Error:" + ex);
            }
        }

        /// <summary>
        /// 接收服务器的数据（聊天信息）
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                int bytesRead;
                bytesRead = this._client.GetStream().EndRead(ar);

                if (bytesRead < 1)
                {
                    return;
                }
                else
                {
                    message += Encoding.UTF8.GetString(data, 0, bytesRead).ToString();
                }
                this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
            }
            catch (Exception ex)
            {
                print("Error:" + ex);
            }
        }
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}