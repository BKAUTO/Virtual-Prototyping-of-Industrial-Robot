using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class ChatControl : MonoBehaviour
    {

        public InputField chatInput;
        public Text chatText;
        public ScrollRect scrollRect;
        string username = "用户";
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (chatInput.text != "")
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

        }
    }
