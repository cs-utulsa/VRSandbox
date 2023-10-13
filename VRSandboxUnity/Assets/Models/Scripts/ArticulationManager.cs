using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationManager : MonoBehaviour
{
    [SerializeField]
    private DogProxy dogProxy;

    [SerializeField]
    private ArticulationBody rootArticulationBody;
    [SerializeField]
    private ArticulationBody frontLeftHipArticulationBody;
    [SerializeField]
    private ArticulationBody frontLeftLegArticulationBody;
    [SerializeField]
    private ArticulationBody frontLeftKneeArticulationBody;
    [SerializeField]
    private ArticulationBody frontRightHipArticulationBody;
    [SerializeField]
    private ArticulationBody frontRightLegArticulationBody;
    [SerializeField]
    private ArticulationBody frontRightKneeArticulationBody;
    [SerializeField]
    private ArticulationBody hindLeftHipArticulationBody;
    [SerializeField]
    private ArticulationBody hindLeftLegArticulationBody;
    [SerializeField]
    private ArticulationBody hindLeftKneeArticulationBody;
    [SerializeField]
    private ArticulationBody hindRightHipArticulationBody;
    [SerializeField]
    private ArticulationBody hindRightLegArticulationBody;
    [SerializeField]
    private ArticulationBody hindRightKneeArticulationBody;

    private void Awake()
	{
        dogProxy.OnStatusUpdate += UpdateArticulationBodies;
	}

	private void UpdateArticulationBodies()
	{
        print("word");
        ////articulationdrive drive;
        ////articulationreducedspace space;
        //var kin_body_quat = dogProxy.KinematicState.transformsSnapshot.childToParentEdgeMap.flat_body.parentTformChild.rotation;
        //Quaternion body_quat = new Quaternion((float)kin_body_quat.x, (float)kin_body_quat.y, (float)kin_body_quat.z, (float)kin_body_quat.w);
        ////rootArticulationBody.transform.rotation = new Quaternion(0, 0, 0, 0);
        //rootArticulationBody.transform.rotation = body_quat;
        ////rootArticulationBody.

        //var kin_body_vec = dogProxy.KinematicState.transformsSnapshot.childToParentEdgeMap.flat_body.parentTformChild.position;
        //Vector3 body_vec = new Vector3((float)kin_body_vec.x, (float)kin_body_vec.y, (float)kin_body_vec.z);
        ////rootArticulationBody.transform.position = dogProxy.originalPos;
        //rootArticulationBody.transform.position = body_vec;

        foreach (JointState jointstate in dogProxy.KinematicState.jointStates)
        {
            switch (jointstate.name)
            {
                case "fl.hx":
                    var angle_flhx = (float)(jointstate.position) * Mathf.Rad2Deg;
                    var jointXDrive_flhx = frontLeftHipArticulationBody.xDrive;
                    jointXDrive_flhx.target = angle_flhx ;
                    jointXDrive_flhx.targetVelocity = (float)jointstate.velocity;
                    frontLeftHipArticulationBody.xDrive = jointXDrive_flhx;
                    break;
                case "fl.hy":
                    var angle_flhy = (float)(jointstate.position) * Mathf.Rad2Deg;
                    var jointXDrive_flhy = frontLeftLegArticulationBody.xDrive;
                    jointXDrive_flhy.target = angle_flhy;
                    jointXDrive_flhy.targetVelocity = (float)jointstate.velocity;
                    frontLeftLegArticulationBody.xDrive = jointXDrive_flhy;
                    break;
                case "fl.kn":
                    var angle_flkn = -(float)(-jointstate.position) * Mathf.Rad2Deg;
                    var jointXDrive_flkn = frontLeftKneeArticulationBody.xDrive;
                    jointXDrive_flkn.target = angle_flkn;
                    jointXDrive_flkn.targetVelocity = (float)jointstate.velocity;
                    frontLeftKneeArticulationBody.xDrive = jointXDrive_flkn;
                    //if (dogProxy.FootStates[0].contact == FootState.Contact.CONTACT_MADE)
                    //{
                    //    frontLeftKneeArticulationBody.transform.position = new Vector3(frontLeftKneeArticulationBody.transform.position.x, (float)-0.3205, frontLeftKneeArticulationBody.transform.position.z);
                    //}
                           
                    break;
                case "fr.hx":
                    var angle_frhx = (float)(jointstate.position) * Mathf.Rad2Deg;
                    var jointXDrive_frhx = frontRightHipArticulationBody.xDrive;
                    jointXDrive_frhx.target = angle_frhx;
                    jointXDrive_frhx.targetVelocity = (float)jointstate.velocity;
                    frontRightHipArticulationBody.xDrive = jointXDrive_frhx;
                    break;
                case "fr.hy":
                    var angle_frhy = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_frhy = frontRightLegArticulationBody.xDrive;
                    jointXDrive_frhy.target = angle_frhy;
                    jointXDrive_frhy.targetVelocity = (float)jointstate.velocity;
                    frontRightLegArticulationBody.xDrive = jointXDrive_frhy;
                    break;
                case "fr.kn":
                    var angle_frkn = -(float)-jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_frkn = frontRightKneeArticulationBody.xDrive;
                    jointXDrive_frkn.target = angle_frkn;
                    jointXDrive_frkn.targetVelocity = (float)jointstate.velocity;
                    frontRightKneeArticulationBody.xDrive = jointXDrive_frkn;
                    //if (dogProxy.FootStates[1].contact == FootState.Contact.CONTACT_MADE)
                    //{
                    //    frontRightKneeArticulationBody.transform.position = new Vector3(frontRightKneeArticulationBody.transform.position.x, (float)-0.3205, frontRightKneeArticulationBody.transform.position.z);
                    //}
                    break;
                case "hl.hx":
                    var angle_hlhx = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hlhx = hindLeftHipArticulationBody.xDrive;
                    jointXDrive_hlhx.target = angle_hlhx;
                    jointXDrive_hlhx.targetVelocity = (float)jointstate.velocity;
                    hindLeftHipArticulationBody.xDrive = jointXDrive_hlhx;
                    break;
                case "hl.hy":
                    var angle_hlhy = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hlhy = hindLeftLegArticulationBody.xDrive;
                    jointXDrive_hlhy.target = angle_hlhy;
                    jointXDrive_hlhy.targetVelocity = (float)jointstate.velocity;
                    hindLeftLegArticulationBody.xDrive = jointXDrive_hlhy;
                    break;
                case "hl.kn":
                    var angle_hlkn = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hlkn = hindLeftKneeArticulationBody.xDrive;
                    jointXDrive_hlkn.target = angle_hlkn;
                    jointXDrive_hlkn.targetVelocity = (float)jointstate.velocity;
                    hindLeftKneeArticulationBody.xDrive = jointXDrive_hlkn;
                    //if (dogProxy.FootStates[2].contact == FootState.Contact.CONTACT_MADE)
                    //{
                    //    hindLeftKneeArticulationBody.transform.position = new Vector3(hindLeftKneeArticulationBody.transform.position.x, (float)-0.3205, hindLeftKneeArticulationBody.transform.position.z);
                    //}
                    break;
                case "hr.hx":
                    var angle_hrhx = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hrhx = hindRightHipArticulationBody.xDrive;
                    jointXDrive_hrhx.target = angle_hrhx;
                    jointXDrive_hrhx.targetVelocity = (float)jointstate.velocity;
                    hindRightHipArticulationBody.xDrive = jointXDrive_hrhx;
                    break;
                case "hr.hy":
                    var angle_hrhy = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hrhy = hindRightLegArticulationBody.xDrive;
                    jointXDrive_hrhy.target = angle_hrhy;
                    jointXDrive_hrhy.targetVelocity = (float)jointstate.velocity;
                    hindRightLegArticulationBody.xDrive = jointXDrive_hrhy;
                    break;
                case "hr.kn":
                    var angle_hrkn = (float)jointstate.position * Mathf.Rad2Deg;
                    var jointXDrive_hrkn = hindRightKneeArticulationBody.xDrive;
                    jointXDrive_hrkn.target = angle_hrkn;
                    jointXDrive_hrkn.targetVelocity = (float)jointstate.velocity;
                    hindRightKneeArticulationBody.xDrive = jointXDrive_hrkn;
                    //if (dogProxy.FootStates[3].contact == FootState.Contact.CONTACT_MADE)
                    //{
                    //    hindRightKneeArticulationBody.transform.position = new Vector3(hindRightKneeArticulationBody.transform.position.x, (float)-0.3205, hindRightKneeArticulationBody.transform.position.z);
                    //}
                    break;
            }
        }
        
    }
}
