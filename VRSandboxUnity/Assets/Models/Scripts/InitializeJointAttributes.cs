using System;
using Unity.Robotics;
using UnityEngine;

// Set the joint values to change how it moves
public class InitializeJointAttributes : MonoBehaviour
{
    // Play with setting these variables
    // to find the best spot behavior
    public float stiffness = 1500;
    public float damping = 100;
    public float forceLimit = 1000;
    public int dynamicVal = 10;

    int k_NumRobotJoints = 12; 

    [SerializeField]
    GameObject m_Spot;
    public GameObject Spot { get => m_Spot; set => m_Spot = value; }
    public static readonly string[] LinkNames =
    {   "base_link/front_rail/front_left_hip",
        "base_link/front_rail/front_left_hip/front_left_upper_leg",
        "base_link/front_rail/front_left_hip/front_left_upper_leg/front_left_lower_leg",
        "base_link/front_rail/front_right_hip",
        "base_link/front_rail/front_right_hip/front_right_upper_leg",
        "base_link/front_rail/front_right_hip/front_right_upper_leg/front_right_lower_leg",
        "base_link/rear_rail/rear_left_hip",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg/rear_left_lower_leg",
        "base_link/rear_rail/rear_right_hip",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg/rear_right_lower_leg"
    };


    void Start()
    {
        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            var articulationBody = m_Spot.transform.Find(LinkNames[i]).GetComponent<ArticulationBody>();

            // Not sure what to set these values to
            articulationBody.jointFriction = dynamicVal;
            articulationBody.angularDamping = dynamicVal;

            // Update important xDrive values here
            var jointXDrive = articulationBody.xDrive;
            jointXDrive.stiffness = stiffness;
            jointXDrive.damping = damping;
            jointXDrive.forceLimit = forceLimit;
            articulationBody.xDrive = jointXDrive;
        }
    }
}
