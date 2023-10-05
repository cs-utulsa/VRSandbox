using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private DogProxy dogProxy;

    [SerializeField]
    private RawImage videoTarget;
    [SerializeField]
    private TMP_Text videoStatusText;
    [SerializeField]
    private TMP_Dropdown videoDropdown;

    [SerializeField]
    private Button autowalkRunButton;
    [SerializeField]
    private TMP_Dropdown autowalkDropdown;
    [SerializeField]
    private Button acquireLeaseButton;
    [SerializeField]
    private Button releaseLeaseButton;
    [SerializeField]
    private Button acquireEStopButton;
    [SerializeField]
    private Button allowEStopButton;

    [SerializeField]
    private Image powerStatusBackground;
    [SerializeField]
    private TMP_Text powerStatusText;
    [SerializeField]
    private Image motorPowerBackground;
    [SerializeField]
    private TMP_Text motorPowerText;
    [SerializeField]
    private Image leaseStatusBackground;
    [SerializeField]
    private TMP_Text leaseStatusText;
    [SerializeField]
    private Image estopStatusBackground;
    [SerializeField]
    private TMP_Text estopStatusText;



    // Method gets autowalk routes, set video's texture, and setup button / video event listeners
    private void Start()
    {
        if (dogProxy == null) Debug.LogError("CanvasManager: DogProxy is null");

        //videoTarget.texture = dogProxy.VideoTexture;
        dogProxy.GetAutowalkRoutes(UpdateAutowalkRoutes);

        //videoDropdown.onValueChanged.AddListener(OnVideoSelect);
        autowalkRunButton.onClick.AddListener(OnAutowalkRun);
        acquireLeaseButton.onClick.AddListener(OnAcquireLease);
        releaseLeaseButton.onClick.AddListener(OnReleaseLease);
        acquireEStopButton.onClick.AddListener(OnAcquireEStop);
        allowEStopButton.onClick.AddListener(OnAllowEStop);

        dogProxy.OnVideoConnected += () =>
        {
            videoStatusText.gameObject.SetActive(false);
        };
        dogProxy.OnVideoDisconnected += () =>
        {
            videoStatusText.gameObject.SetActive(true);
            videoStatusText.text = "Video\nDisconnected";
        };

        dogProxy.OnStatusUpdate += () =>
        {
        //     switch (dogProxy.PowerStatus)
		// 	{
        //         case PowerState.RobotPowerState.ROBOT_POWER_STATE_UNKNOWN:
        //             powerStatusBackground.color = Color.gray;
        //             powerStatusText.text = "Power Status:\nUNKNOWN";
        //             break;
        //         case PowerState.RobotPowerState.ROBOT_POWER_STATE_ON:
        //             powerStatusBackground.color = Color.green;
        //             powerStatusText.text = "Power Status:\nON";
        //             break;
        //         case PowerState.RobotPowerState.ROBOT_POWER_STATE_OFF:
        //             powerStatusBackground.color = Color.red;
        //             powerStatusText.text = "Power Status:\nOFF";
        //             break;
		// 	}

        //     switch (dogProxy.MotorPower)
        //     {
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_UNKNOWN:
        //             motorPowerBackground.color = Color.gray;
        //             motorPowerText.text = "Motor Power:\nUNKNOWN";
        //             break;
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_OFF:
        //             motorPowerBackground.color = Color.red;
        //             motorPowerText.text = "Motor Power:\nOFF";
        //             break;
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_ON:
        //             motorPowerBackground.color = Color.green;
        //             motorPowerText.text = "Motor Power:\nON";
        //             break;
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_POWERING_ON:
        //             motorPowerBackground.color = Color.yellow;
        //             motorPowerText.text = "Motor Power:\nPOWERING ON";
        //             break;
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_POWERING_OFF:
        //             motorPowerBackground.color = Color.yellow;
        //             motorPowerText.text = "Motor Power:\nPOWERING OFF";
        //             break;
        //         case PowerState.MotorPowerState.MOTOR_POWER_STATE_ERROR:
        //             motorPowerBackground.color = new Color(255, 191, 0);
        //             motorPowerText.text = "Motor Power:\nERROR";
        //             break;
        //     }

        //     switch (dogProxy.LeaseOwned)
        //     {
        //         case true:
        //             leaseStatusBackground.color = Color.green;
        //             leaseStatusText.text = "Lease Status:\nOWNED";
        //             break;
        //         case false:
        //             leaseStatusBackground.color = Color.red;
        //             leaseStatusText.text = "Lease Status:\nUNOWNED";
        //             break;
        //     }

        //     switch (dogProxy.EStopState)
        //     {
        //         case EStopState.State.STATE_UNKNOWN:
        //             estopStatusBackground.color = Color.gray;
        //             estopStatusText.text = "Power Status:\nUNKNOWN";
        //             break;
        //         case EStopState.State.STATE_ESTOPPED:
        //             estopStatusBackground.color = Color.red;
        //             estopStatusText.text = "Power Status:\nESTOPPED";
        //             break;
        //         case EStopState.State.STATE_NOT_ESTOPPED:
        //             estopStatusBackground.color = Color.green;
        //             estopStatusText.text = "Power Status:\nNOT_ESTOPED";
        //             break;
        //     }
            };
    }

    // Method updates the UI when autowalks are revived
    private void UpdateAutowalkRoutes(List<string> autowalks)
	{
        if (autowalks != null)
		{
            autowalkDropdown.ClearOptions();
            autowalkDropdown.AddOptions(autowalks);
		}
        else
		{
            Debug.LogError("Failed to update auto-walk routes");
		}
    }

    // private void OnVideoSelect(int value)
	// {
    //     if (value == 0) dogProxy.StopVideo();
    //     else dogProxy.StartVideo((DogProxy.Camera)(value - 1));
	// }

    // Method attempts to run the autowalk selected in the dropdown
    private void OnAutowalkRun()
	{
        if (autowalkDropdown.options.Count > 0)
		{
            dogProxy.Autowalk(autowalkDropdown.options[autowalkDropdown.value].text, result =>
            {
                if (result == UnityWebRequest.Result.Success)
                {

                }
                else
                {
                    Debug.LogError("Failed to run auto-walk");
                    //TODO: Show UI error.
                }
            });
		}
	}

    // Method attempts to acquire a lease
    private void OnAcquireLease()
	{
        acquireLeaseButton.interactable = false;
        dogProxy.AcquireLease(result =>
        {
            acquireLeaseButton.interactable = true;
            if (result == UnityWebRequest.Result.Success)
			{
                acquireLeaseButton.gameObject.SetActive(false);
                releaseLeaseButton.gameObject.SetActive(true);
			}
            else
			{
                Debug.LogError("Failed to acquire lease");
                //TODO: Show UI error.
			}
        });
	}

    // Method attempts to release a lease
    private void OnReleaseLease()
    {
        releaseLeaseButton.interactable = false;
        dogProxy.ReleaseLease(result =>
        {
            releaseLeaseButton.interactable = true;
            if (result == UnityWebRequest.Result.Success)
            {
                acquireLeaseButton.gameObject.SetActive(true);
                releaseLeaseButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to release lease");
                //TODO: Show UI error.
            }
        });
    }

    // Method attempts to acquire a e-stop
    private void OnAcquireEStop()
	{
        acquireEStopButton.interactable = false;
        dogProxy.AcquireEStop(result =>
        {
            acquireEStopButton.interactable = true;
            if (result == UnityWebRequest.Result.Success)
            {
                acquireEStopButton.gameObject.SetActive(false);
                allowEStopButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to acquire e-stop");
                //TODO: Show UI error.
            }
        });
    }

    // Method attempts to allow the e-stop
    private void OnAllowEStop()
	{
        allowEStopButton.interactable = false;
        dogProxy.AllowEStop(result =>
        {
            allowEStopButton.interactable = true;
            if (result == UnityWebRequest.Result.Success)
            {
                allowEStopButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to allow e-stop");
                //TODO: Show UI error.
            }
        });
    }

    public void OnTogglePower() => dogProxy.TogglePower();
    public void OnSelfRight() => dogProxy.SelfRight();
    public void OnStand() => dogProxy.Stand();
    public void OnSit() => dogProxy.Sit();
    public void OnRollover() => dogProxy.Rollover();
    public void OnMoveForward() => dogProxy.MoveForward();
    public void OnMoveBackward() => dogProxy.MoveBackward();
    public void OnStrafeLeft() => dogProxy.StrafeLeft();
    public void OnStrafeRight() => dogProxy.StrafeRight();
    public void OnTurnLeft() => dogProxy.TurnLeft();
    public void OnTurnRight() => dogProxy.TurnRight();
}
