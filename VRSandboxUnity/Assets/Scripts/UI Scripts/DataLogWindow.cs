using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class DataLogWindow : MonoBehaviour
{
    public TextMeshProUGUI LogText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(string[] messages)
    {
        StringBuilder messageText = new StringBuilder();

        foreach(string msg in messages)
        {
            messageText.AppendLine(msg);
        }

        LogText.text = messageText.ToString();
    }

    public void ClearData()
    {
        // Clear the log text
        LogText.text = string.Empty;
    }


}
