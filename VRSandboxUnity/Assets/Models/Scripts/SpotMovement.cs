using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SpotMovement : MonoBehaviour
{
    //Variables to allow drag and drop rig in the inspector
    public Rig rig = null;   

    //variables to drag and drop controllers and input from json
    public Transform targetFL = null;
    public Transform targetFR = null;
    public Transform targetBL = null;
    public Transform targetBR = null;
    
    //new position input variables
    private Vector3 FL = new Vector3(1, 1, 1);
    private Vector3 FR;
    private Vector3 BL;
    private Vector3 BR;
     

    //adjustable in case some speed adjustment is needed. This will probably be some interpolation later.
    public float Movespeed = 3.5f;

    

    public void Awake()
    {
        //FL = new Vector3(.05f, .05f, .05f);
    }


    private void Update()
    {
        //TO DO: get the input from the dogs
        

        //apply the input to the foot controllers
        /*
        targetFL.Translate(FL * Time.deltaTime);
        targetFR.Translate(FR * Time.deltaTime);
        targetBL.Translate(BR * Time.deltaTime);
        targetBR.Translate(BR * Time.deltaTime);
        */
        for(int i = 2; i < 50; ++i)
        {
            if(i % 2 == 0)
            {
                targetFL.position = FL;
            }
            else
            {
                targetFL.position += -FL;
            }
        }
        

    }

}
