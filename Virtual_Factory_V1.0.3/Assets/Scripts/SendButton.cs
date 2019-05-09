using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using server;


public class SendButton : MonoBehaviour
{
    public InputField InputX,InputY,InputZ,InputQ1,InputQ2,InputQ3,InputQ4;
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
        Demo.displayText = "\n  " + "<color=red>" + "开始运动至期望位姿" + "</color>";
        Demo.flag2 = true;

        Demo.pose[0] = System.Convert.ToDouble(InputX.text);
        InputX.text = "";

        Demo.pose[1] = System.Convert.ToDouble(InputY.text);
        InputY.text = "";

        Demo.pose[2] = System.Convert.ToDouble(InputZ.text);
        InputZ.text = "";

        Demo.pose[3] = System.Convert.ToDouble(InputQ1.text);
        InputQ1.text = "";

        Demo.pose[4] = System.Convert.ToDouble(InputQ2.text);
        InputQ2.text = "";

        Demo.pose[5] = System.Convert.ToDouble(InputQ3.text);
        InputQ3.text = "";

        Demo.pose[6] = System.Convert.ToDouble(InputQ4.text);
        InputQ4.text = "";

        InputX.ActivateInputField();
        InputY.ActivateInputField();
        InputZ.ActivateInputField();
        InputQ1.ActivateInputField();
        InputQ2.ActivateInputField();
        InputQ3.ActivateInputField();
        InputQ4.ActivateInputField();
    }
}