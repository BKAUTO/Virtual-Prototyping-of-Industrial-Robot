using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SendButton : MonoBehaviour
{
    public InputField chatInput;
    public Text chatText;
    public ScrollRect scrollRect;
    string username = "用户";

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
        string addText = "\n  " + "<color=red>" + username + "</color>: " + chatInput.text;
        chatText.text += addText;
        chatInput.text = "";
        chatInput.ActivateInputField();
        Canvas.ForceUpdateCanvases();       //关键代码
        scrollRect.verticalNormalizedPosition = 0f;  //关键代码
        Canvas.ForceUpdateCanvases();   //关键代码
    }
}