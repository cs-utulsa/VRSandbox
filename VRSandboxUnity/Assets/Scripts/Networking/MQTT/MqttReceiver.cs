/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;

public class MqttReceiver : M2MqttUnityClient
{
    //using C# Property GET/SET and event listener to reduce Update overhead in the controlled objects
    private string m_msg;

    public string msg
    {
        get
        {
            return m_msg;
        }
        set
        {
            m_msg = value;
            if (OnMessageArrived != null)
            {
                OnMessageArrived(m_msg);
            }
        }
    }

    public event OnMessageArrivedDelegate OnMessageArrived;
    public delegate void OnMessageArrivedDelegate(string newMsg);

    //using C# Property GET/SET and event listener to expose the connection status
    private bool m_isConnected;

    public bool isConnected
    {
        get
        {
            return m_isConnected;
        }
        set
        {
            if (m_isConnected == value) return;
            m_isConnected = value;
            if (OnConnectionSucceeded != null)
            {
                OnConnectionSucceeded(isConnected);
            }
        }
    }
    public event OnConnectionSucceededDelegate OnConnectionSucceeded;
    public delegate void OnConnectionSucceededDelegate(bool isConnected);

    // a list to store the messages
    private List<string> eventMessages = new List<string>();

    protected List<string> topicsToSubscribe = new List<string>();
    protected List<string> topicsToUnsubscribe = new List<string>();

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    protected override void OnConnecting()
    {
        Debug.Log($"Connecting to {brokerAddress}");
        base.OnConnecting();
    }

    protected override void OnConnected()
    {
        Debug.Log($"Succesffully connected to {brokerAddress}");
        base.OnConnected();
        isConnected = true;
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        Debug.Log("Disconnected.");
        isConnected = false;
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("CONNECTION LOST!");
    }

    /// <summary>
    /// Handles subscribing to topics
    /// </summary>
    protected override void SubscribeTopics()
    {
        Debug.Log("Subscribing to Topics");
        //remove any topics we are already subscribed to from the list
        var trueSubList = topicsToSubscribe.Where(x => !topicsToUnsubscribe.Contains(x));
        //subscribe to new topics
        client.Subscribe(topicsToSubscribe.ToArray(), new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //add new topics to the unsubscribe list
        topicsToUnsubscribe.AddRange(trueSubList);
    }

    /// <summary>
    /// subscribes to a given list of topics
    /// </summary>
    /// <param name="topics"></param>
    protected void SubscribeTopics(string[] topics)
    {
        topicsToSubscribe = new List<string>(topics);
        SubscribeTopics();
    }

    protected override void UnsubscribeTopics()
    {
        Debug.Log("Unsubscribing from Topics");
        client.Unsubscribe(topicsToUnsubscribe.ToArray());
    }

    protected void UnsubscribeTopics(string[] topics)
    {
        //store the topics we need to unsub from
        var temp = topicsToUnsubscribe;
        //Find which topics in the given list are ready to be unsubscribed
        topicsToUnsubscribe = new List<string>(topics).Where(x => temp.Contains(x)).ToList();
        //unsubscribe from those topics
        UnsubscribeTopics();
        //remove those topics from the unsubscribe list
        temp.RemoveAll(x => topicsToUnsubscribe.Contains(x));
        //restore the unsubscribe list
        topicsToUnsubscribe = temp;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        //The message is decoded
        string tempmsg = System.Text.Encoding.UTF8.GetString(message);

        Debug.Log("Received: " + tempmsg);
        Debug.Log("from topic: " + topic);

        msg = $"{topic}: {tempmsg}";

        StoreMessage(msg);
    }

    private void StoreMessage(string eventMsg)
    {
        if (eventMessages.Count > 50)
        {
            eventMessages.Clear();
        }
        eventMessages.Add(eventMsg);
    }

    protected override void Update()
    {
        //calls ProcessMqttEvents()
        base.Update(); 
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
    }
}