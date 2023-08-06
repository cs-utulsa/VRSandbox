using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = -0.5f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = +0.5f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = +0.5f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = -0.5f;

        float moveSpeed = 3f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
