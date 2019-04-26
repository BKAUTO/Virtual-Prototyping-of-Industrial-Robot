using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using server;


public class SendButton : MonoBehaviour
{
    public InputField jointInput1,jointInput2,jointInput3,jointInput4,jointInput5,jointInput6;
    public Text chatText;
    public ScrollRect scrollRect;
    string username = "上位机";

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
        if(Demo.recvStr2[0] == 49)//机械臂运动完毕可以接收新的关节角度
        {
            Demo.displayText = "\n  " + "<color=red>" + "开始运动至当前位姿" + "</color>";
            Demo.flag2 = true;

            byte[] bs = Encoding.UTF8.GetBytes(jointInput1.text+"\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//发送第一个角度给客户端
            Debug.Log(jointInput1.text);
            jointInput1.text = "";

            bs = Encoding.UTF8.GetBytes(jointInput2.text+"\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//第二个
            Debug.Log(jointInput2.text);
            jointInput2.text = "";

            bs = Encoding.UTF8.GetBytes(jointInput3.text + "\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//第三个
            Debug.Log(jointInput3.text);
            jointInput3.text = "";

            bs = Encoding.UTF8.GetBytes(jointInput4.text + "\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//第四个
            Debug.Log(jointInput4.text);
            jointInput4.text = "";

            bs = Encoding.UTF8.GetBytes(jointInput5.text + "\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//第五个
            Debug.Log(jointInput5.text);
            jointInput5.text = "";

            bs = Encoding.UTF8.GetBytes(jointInput6.text + "\n");
            Demo.lst[3].Send(bs, bs.Length, 0);//第六个
            Debug.Log(jointInput6.text);
            jointInput6.text = "";
        }
        else if(Demo.recvStr2[0] == 48)//机械臂仍在运动
        {
            Demo.displayText = "\n  " + "<color=red>" + "机械臂仍在运动，请稍等..." + "</color>";
            Demo.flag2 = true;
        }
        jointInput1.ActivateInputField();
        jointInput2.ActivateInputField();
        jointInput3.ActivateInputField();
        jointInput4.ActivateInputField();
        jointInput5.ActivateInputField();
        jointInput6.ActivateInputField();
    }
}