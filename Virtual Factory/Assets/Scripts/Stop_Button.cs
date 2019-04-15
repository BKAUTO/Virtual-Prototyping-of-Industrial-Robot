using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using server;
using System.Text;


public class Stop_Button : MonoBehaviour
{
    private Button btn6;
    private bool Flag3 = false;
    private void Start()
    {
        btn6 = GetComponent<Button>();

        btn6.onClick.AddListener(
            delegate ()
            {
                OnClickBtn6();
            }
            );
    }
    public void OnClickBtn6()
    {
        if (Flag3 == false)
        {
            string sendStr1 = "stop\r";
            byte[] bs1 = Encoding.UTF8.GetBytes(sendStr1);
            Demo.lst[0].Send(bs1, bs1.Length, 0);//发送信息给客户端
            string sendStr2 = "state\r";
            byte[] bs2 = Encoding.UTF8.GetBytes(sendStr2);
            Demo.lst[0].Send(bs2, bs2.Length, 0);//发送信息给客户端
            Flag3 = true;
        }

        else if(Flag3 == true)
        {
            string sendStr3 = "start\r";
            byte[] bs3 = Encoding.UTF8.GetBytes(sendStr3);
            Demo.lst[0].Send(bs3, bs3.Length, 0);//发送信息给客户端
            string sendStr4 = "state\r";
            byte[] bs4 = Encoding.UTF8.GetBytes(sendStr4);
            Demo.lst[0].Send(bs4, bs4.Length, 0);//发送信息给客户端
            Flag3 = false;
        }

        Demo.bytes1 = Demo.lst[0].Receive(Demo.recvBytes, Demo.recvBytes.Length, 0);
        Demo.recvStr = Encoding.UTF8.GetString(Demo.recvBytes, 0, Demo.bytes1);
        Demo.displayText = "\n" + Demo.recvStr + "\n";
        Demo.flag2 = true;
    }
}