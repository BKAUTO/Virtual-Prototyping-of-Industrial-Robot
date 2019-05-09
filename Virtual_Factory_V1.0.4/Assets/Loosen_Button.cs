using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using server;
using System.Text;

public class Loosen_Button : MonoBehaviour
{

    private Button btn6;
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
        if (Demo.flag3 == true)
        {
            try
            {
                string sendStr0 = "0";
                byte[] bs0 = Encoding.UTF8.GetBytes(sendStr0);
                Demo.lst[1].Send(bs0, bs0.Length, 0);//发送松开指令给客户端
            }
            catch
            {
                Demo.displayText = "\n" + "<color=red>" + "电爪已断开......\n" + "</color>";
                Demo.flag3 = false;
            }
        }
        else
        {
            Demo.displayText = "\n" + "<color=red>" + "电爪已断开......\n" + "</color>";
            Demo.flag2 = true;
        }

        /*
        Demo.bytes1 = Demo.lst[1].Receive(Demo.recvBytes, Demo.recvBytes.Length, 0);
        Demo.recvStr = Encoding.UTF8.GetString(Demo.recvBytes, 0, Demo.bytes1);
        Demo.displayText = "\n" + Demo.recvStr + "\n";
        */
    }
}
