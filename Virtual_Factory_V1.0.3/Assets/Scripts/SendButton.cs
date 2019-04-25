using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        string addText = "\n  " + "<color=red>" + "开始运动至当前位姿" + "</color>";
        chatText.text += addText;
        //chatInput.text = "";
        jointInput1.ActivateInputField();
        jointInput2.ActivateInputField();
        jointInput3.ActivateInputField();
        jointInput4.ActivateInputField();
        jointInput5.ActivateInputField();
        jointInput6.ActivateInputField();
        Canvas.ForceUpdateCanvases();       //关键代码
        scrollRect.verticalNormalizedPosition = 0f;  //关键代码
        Canvas.ForceUpdateCanvases();   //关键代码
    }
}