using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MqttPublisher
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if(isConnected) {
            SendCommand("L4MP", "on");
            Debug.Log("Command sent");
        }

    }

    public void SendCommand(string deviceID, string command)
    {
        topicPublish = $"commands/{deviceID}/{command}";
        messagePublish = " "; // Message body is used for arguments of commands, if any
        Publish();
    }

}
