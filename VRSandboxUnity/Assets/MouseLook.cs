
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MouseLook : MonoBehaviour
{

   private Vector2 rotation; //current rotation in degrees
    [SerializeField] private Vector2 sensitivity;

    private GameObject thisClient;
    private GameObject camera;
    private bool startFlag = false;

    private void Awake()
    {
        thisClient = this.gameObject;
        camera = thisClient.transform.Find("Camera").gameObject;
    }

    private void StartingPos()
    {
        this.transform.position = new Vector3(1.41f, 1.4f, 3.14f);
        startFlag = true;
    }

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );
        return input;
    }

    private void Update()
    {
        //Velocity is the current input scaled by the chosen sensitivity
        Vector2 Velocity = sensitivity * GetInput();

        //new rotation
        rotation += Velocity * Time.deltaTime;

        //Convert rotation to Euler angle
        camera.transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);

        if (!startFlag)
        {
            StartingPos();
        }
    }
}