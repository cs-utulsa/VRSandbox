using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SpotMovement : MonoBehaviour
{
    public Rig rigFL = null;
    public Transform targetFL = null;
    public float Movespeed = 3.5f;

    private void Update()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
    }

}
