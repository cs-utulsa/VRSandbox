using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

public class DogProxy : MonoBehaviour
{
	// private const string host = "http://10.0.1.69:5001";
	// private const string statusHost = "http://10.0.1.69:5001/status";
	// private const string statusPath = "/status/socket.io";
	private const string host = "http://10.0.1.69:5001";
	private const string statusHost = "http://10.0.1.69:5010";
	private const string statusPath = "";
	private Texture2D videoTexture;

	private SocketIOUnity statusSocket;
	//private VideoStream videoStream;

	[Serializable]
	private class RobotStatusUpdate
	{
		public PowerState.RobotPowerState power_status;
		public PowerState.MotorPowerState motor_power;
		public EStopState.State have_estop;
		public double battery_level;
		public bool lease_owned;
		public string lease_owner;
		public bool autowalking;
		public KinematicState kinematicState;
		public FootState[] footState;
	}

	// Method initializes videoTexture so that a null reference exception will not occur when LoadImage is called
	private void Awake()
	{
		videoTexture = new Texture2D(1, 1);
	}
	

	// Method initializes the image socket
	private void Start()
	{
		statusSocket = new SocketIOUnity(statusHost, new SocketIOOptions()
		{
			Path = statusPath,
			//ConnectionTimeout = TimeSpan.FromMilliseconds(5000),
			//ReconnectionAttempts = 3,
		});

		statusSocket.JsonSerializer = new NewtonsoftJsonSerializer();

		// Called when the socket connects to the server
		statusSocket.OnConnected += (sender, e) =>
		{
			Debug.Log($"Connected to {host}{statusPath}");
			// statusSocket.Emit("hashdata");
			statusSocket.Emit("message");
			UnityThread.executeInUpdate(() => OnStatusConnected?.Invoke());
		};

		// Called when the socket disconnects from the server
		statusSocket.OnDisconnected += (sender, e) =>
		{
			Debug.Log($"Disconnected from {host}{statusPath}");
			UnityThread.executeInUpdate(() => OnStatusDisconnected?.Invoke());
		};

		// Called when the socket reconnects to the server
		statusSocket.OnReconnected += (sender, e) =>
		{
			Debug.Log($"Reconnected to {host}{statusPath}");
			UnityThread.executeInUpdate(() => OnStatusConnected?.Invoke());
		};

		// Called on every reconnect attempt
		statusSocket.OnReconnectAttempt += (sender, e) =>
		{
			Debug.Log($"Reconnecting to {host}{statusPath}");
			UnityThread.executeInUpdate(() => OnStatusReconnecting?.Invoke());
		};

		// Called when a reconnect attempt fails
		statusSocket.OnReconnectError += (sender, e) =>
		{
			Debug.LogError($"Error Reconnecting to {host}{statusPath}");
		};

		// Called only if the number of reconnection attempts is equal to ReconnectionAttempts in the socket options.
		// Note that ReconnectionAttempts is by default int.MaxValue, meaning that the socket will try to reconnect forever
		statusSocket.OnReconnectFailed += (sender, e) =>
		{
			Debug.LogError($"Failed to reconnect to {host}{statusPath}");
			UnityThread.executeInUpdate(() => OnStatusDisconnected?.Invoke());
		};

		// Called when an status update is received from the server
		statusSocket.OnUnityThread("status update", response =>
		{
			Debug.Log("Status Update");
			RobotStatusUpdate robotStatus = response.GetValue<RobotStatusUpdate>();

			//"[{\"power_status\":\"ROBOT_POWER_STATE_ON\",\"motor_power\":\"STATE_OFF\",\"have_estop\":\"STATE_NOT_ESTOPPED\",\"battery_level\":65.0,\"lease_owned\":\"True\",\"lease_owner\":\"WASDClient76b67cff3017:__main__.py-55\",\"autowalking\":false}]"

			PowerStatus = robotStatus.power_status;
			MotorPower = robotStatus.motor_power;
			BatteryLevel = robotStatus.battery_level;
			EStopState = robotStatus.have_estop;
			LeaseOwned = robotStatus.lease_owned;
			KinematicState = robotStatus.kinematicState;
			FootStates= robotStatus.footState;
			UnityThread.executeInUpdate(() => OnStatusUpdate.Invoke());
		});

		statusSocket.OnAnyInUnityThread((eventName, response) =>
		{
			Debug.Log(eventName);
		});

		// OnVideoUpdate += () =>
		// {
		// 	UnityThread.executeInUpdate(() => videoStream.UseImage((rawImage) =>
		// 	{
		// 		if (!videoTexture.LoadImage(rawImage)) Debug.LogError("failed to load image");
		// 	}));
		// };

		StartStatus();
	}

	// Method disconnects the socket when the application closes
	// The object's finalizer should call this but have had mixed results in the editor.
	private void OnApplicationQuit()
	{
		statusSocket.DisconnectAsync();
	}

	// Class for deserializing autowalk routes due to JSON formating
	private class AutowalkRoutesDTO
	{
		public List<string> autowalks;
	}

	// Method is a coroutine that waits for the Get request to finish before invoking the callback method
	// Note that T is the type that will be deserialized from the recieved JSON
	// If the request failed the types default value will be passed to the callback method (0 for integers, null for objects)
	private IEnumerator APIGet<T>(string path, Action<T> action)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(host + path))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(webRequest.error);
				action(default);
			}
			else action(JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text));
		}
	}

	// Method is a coroutine that waits for the Get request to finish before invoking the callback method
	private IEnumerator APIGet(string path, Action<UnityWebRequest.Result> action = null)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(host + path))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result != UnityWebRequest.Result.Success) Debug.LogError(webRequest.error);
			action(webRequest.result);
		}
	}

	// Method is a coroutine that waits for the Post request to finish before invoking the callback method
	// Note that data must be a serializable type
	private IEnumerator APIPost(string path, object data, Action<UnityWebRequest.Result> action = null)
	{
		using UnityWebRequest webRequest = UnityWebRequest.Post(host + path, JsonConvert.SerializeObject(data));
		yield return webRequest.SendWebRequest();

		if (webRequest.result != UnityWebRequest.Result.Success) Debug.LogError(webRequest.error);
		action(webRequest.result);
	}

	public delegate void DogProxyEvent();

	public enum Camera
	{
		Stitch = 0,
		Left = 1,
		Right = 2,
		Back = 3,
	}

	public event DogProxyEvent OnVideoUpdate;
	public event DogProxyEvent OnVideoConnected;
	public event DogProxyEvent OnVideoDisconnected;
	public event DogProxyEvent OnStatusUpdate;
	public event DogProxyEvent OnStatusConnected;
	public event DogProxyEvent OnStatusReconnecting;
	public event DogProxyEvent OnStatusDisconnected;
	public PowerState.RobotPowerState PowerStatus { get; private set; }
	public PowerState.MotorPowerState MotorPower { get; private set; }
	public EStopState.State EStopState { get; private set; }
	public bool LeaseOwned { get; private set; }
	public double BatteryLevel { get; private set; }
	public KinematicState KinematicState { get; private set; }
	public FootState[] FootStates { get; private set; }
	public Texture2D VideoTexture { get { return videoTexture; } }

	public void GetRobotState(string robot, Action<RobotState> action) => StartCoroutine(APIGet<RobotState>($"/api/{robot}data", action));
	public void GetAutowalkRoutes(Action<List<string>> action) => StartCoroutine(APIGet<AutowalkRoutesDTO>("/api/autowalk/routes", result => action(result?.autowalks)));

	// public void StartVideo(Camera camera)
	// {
	// 	string[] cameraPaths = { "/stitch_image/output", "/left_image/output", "/right_image/output", "/back_image/output" };

	// 	videoStream = new VideoStream(host + cameraPaths[(int)camera]);
	// 	videoStream.OnConnected += (sender, e) =>
	// 	{
	// 		UnityThread.executeInUpdate(() => OnVideoConnected?.Invoke());
	// 	};
	// 	videoStream.OnDisconnected += (sender, e) =>
	// 	{
	// 		UnityThread.executeInUpdate(() => OnVideoDisconnected?.Invoke());
	// 	};
	// 	videoStream.OnReceived += (sender, e) =>
	// 	{
	// 		UnityThread.executeInUpdate(() => OnVideoUpdate?.Invoke());
	// 	};
	// }
	// public void StopVideo()
	// {
	// 	videoStream?.Dispose();
	// }

	public void StartStatus()
	{
		statusSocket.ConnectAsync();
		UnityThread.executeInUpdate(() => OnStatusReconnecting?.Invoke());
	}
	public void StopStatus() => statusSocket.DisconnectAsync();

	public void AcquireLease(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/lease/acquire", action));
	public void ReleaseLease(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/lease/release", action));
	public void HijackLease(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/lease/hijack", action));
	public void DisconnectLease(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/lease/disconnect", action));

	public void AcquireEStop(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/estop/acquire", action));
	public void AllowEStop(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/estop/allow", action));
	public void StopEStop(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/estop/stop", action));
	public void StartEStop(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/estop/start", action));

	public void TogglePower(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/toggle/power", action));
	public void SelfRight(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/selfright", action));
	public void Stand(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/stand", action));
	public void Sit(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/sit", action));
	public void Rollover(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/rollover", action));
	public void Autowalk(string autowalk, Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIPost("/api/autowalk", autowalk, action));
	public void MoveForward(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/move_forward", action));
	public void MoveBackward(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/move_backward", action));
	public void StrafeLeft(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/strafe_left", action));
	public void StrafeRight(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/strafe_right", action));
	public void TurnLeft(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/turn_left", action));
	public void TurnRight(Action<UnityWebRequest.Result> action = null) => StartCoroutine(APIGet("/api/turn_right", action));
	
}
