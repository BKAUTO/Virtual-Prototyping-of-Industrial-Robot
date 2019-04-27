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
        Demo.displayText = "\n  " + "<color=red>" + "开始运动至当前位姿" + "</color>";
        Demo.flag2 = true;

        Demo.pose[0] = System.Convert.ToDouble(jointInput1.text);
        jointInput1.text = "";

        Demo.pose[1] = System.Convert.ToDouble(jointInput2.text);
        jointInput2.text = "";

        Demo.pose[2] = System.Convert.ToDouble(jointInput3.text);
        jointInput3.text = "";

        Demo.pose[3] = System.Convert.ToDouble(jointInput4.text);
        jointInput4.text = "";

        Demo.pose[4] = System.Convert.ToDouble(jointInput5.text);
        jointInput5.text = "";

        Demo.pose[5] = System.Convert.ToDouble(jointInput6.text);
        jointInput6.text = "";
        jointInput1.ActivateInputField();
        jointInput2.ActivateInputField();
        jointInput3.ActivateInputField();
        jointInput4.ActivateInputField();
        jointInput5.ActivateInputField();
        jointInput6.ActivateInputField();
    }
}