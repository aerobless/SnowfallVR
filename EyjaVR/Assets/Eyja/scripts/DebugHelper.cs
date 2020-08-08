using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugHelper : MonoBehaviour
{

    public static DebugHelper Instance;

    bool inMenu;

    Text logText;

    void Awake()
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        var rt = DebugUIBuilder.instance.AddLabel("Debug");
        logText = rt.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }
    }

    public void Log(string msg)
    {
        if(logText != null){
            logText.text = msg;
        }
    }
}
