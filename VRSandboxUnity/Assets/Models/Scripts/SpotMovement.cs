using UnityEngine;
using UnityEngine.Animations.Rigging;


public class SpotMovement : MonoBehaviour
{
    //Variables to allow drag and drop rig in the inspector
    public Rig rig = null; 
    public DogProxy dogProxy;
    public GameObject body;
     

    //variables to drag and drop controllers and input from json
    public Transform targetFL = null;
    public Transform targetFR = null;
    public Transform targetBL = null;
    public Transform targetBR = null;

    public Transform init_FL = null;
    public Transform init_FR = null;
    public Transform init_BL = null;
    public Transform init_BR = null;
    
    //new position input variables
    private Vector3 FL= new Vector3(.1f,.3f,.11f);
    private Vector3 FR= new Vector3(.1f,.3f,.11f);
    private Vector3 BL= new Vector3(.1f,.3f,.11f);
    private Vector3 BR= new Vector3(.1f,.3f,.11f);
    float x = .55f;
    float y = .6f;
    float z = .8f;
    
    

    //adjustable in case some speed adjustment is needed. This will probably be some interpolation later.
    public float Movespeed = 0f;
    private void Awake()
	{
        dogProxy.OnStatusUpdate += UpdateSpot;
	}
    private void UpdateSpot()
    {
        //TO DO: get the input from the dogs

        //apply the input to the foot controllers
        //print(dogProxy.FootState)
        targetFL.position = body.transform.position + body.transform.TransformDirection(new Vector3((float)dogProxy.FootStates[0].footPositionRtBody.x, (float)dogProxy.FootStates[0].footPositionRtBody.y, (float)dogProxy.FootStates[0].footPositionRtBody.z));
        targetFR.position = body.transform.position + body.transform.TransformDirection(new Vector3((float)dogProxy.FootStates[1].footPositionRtBody.x, (float)dogProxy.FootStates[1].footPositionRtBody.y, (float)dogProxy.FootStates[1].footPositionRtBody.z));
        targetBL.position = body.transform.position + body.transform.TransformDirection(new Vector3((float)dogProxy.FootStates[2].footPositionRtBody.x, (float)dogProxy.FootStates[2].footPositionRtBody.y, (float)dogProxy.FootStates[2].footPositionRtBody.z));
        targetBR.position = body.transform.position + body.transform.TransformDirection(new Vector3((float)dogProxy.FootStates[3].footPositionRtBody.x, (float)dogProxy.FootStates[3].footPositionRtBody.y, (float)dogProxy.FootStates[3].footPositionRtBody.z));

       
        
        //print(dogProxy.FootPoss.x);
        //targetFL.localPosition = dogProxy.FootPoss;
    
    
    //     Movespeed = 0;

    //     while (Movespeed <= 1){
    //         print(Movespeed);
    //         targetFL.localPosition = Vector3.Lerp(targetFL.localPosition ,FL, Movespeed);
    //         targetFR.localPosition = Vector3.Lerp(targetFR.localPosition ,FR, Movespeed);
    //         targetBL.localPosition = Vector3.Lerp(targetBL.localPosition ,BL, Movespeed);
    //         targetBR.localPosition = Vector3.Lerp(targetBR.localPosition ,BR, Movespeed);
    //         Movespeed += Time.deltaTime;
    //     }   
    // FL = new Vector3(x,y,z);
    // FR = new Vector3(x,y,z);
    // BL = new Vector3(x,y,z);
    // BR = new Vector3(x,y,z); 
    // float hold = x;
    // x = y;
    // y=z;
    // z = hold;
    }

}
