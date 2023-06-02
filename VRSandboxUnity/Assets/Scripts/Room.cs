using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    /// <summary>
    /// Collider representing the floor of the room. should be flat
    /// </summary>
    public Collider RoomCollider;

    /// <summary>
    /// All sensors currently used by this room
    /// </summary>
    public Sensor[] Sensors;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Sensor sensor in Sensors)
        {
            sensor.InitializeSensor(this);
            sensor.StartDisplayVisual();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
