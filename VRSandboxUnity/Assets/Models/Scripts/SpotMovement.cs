using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SpotMovement : MonoBehaviour
{
    public Rig rig = null;   

    public Transform targetFL = null;
    public Transform targetFR = null;
    public Transform targetBL = null;
    public Transform targetBR = null;

    public float Movespeed = 3.5f;

    private void Update()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
    }

}
