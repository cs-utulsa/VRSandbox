using System;
using UnityEngine;

// Quaternion implemented using doubles instead of floats to match API documentation
[Serializable]
public class QuaternionDouble
{
	public double x;
	public double y;
	public double z;
	public double w;
}

// Vector3 implemented using doubles instead of floats to match API documentation
[Serializable]
public class Vector3Double
{
	public double x;
	public double y;
	public double z;
}

// See https://dev.bostondynamics.com/protos/bosdyn/api/proto_reference for documentation on following types

[Serializable]
public class PowerState
{
	public enum MotorPowerState
	{
		STATE_UNKNOWN = 0,
		MOTOR_POWER_STATE_UNKNOWN = 0,
		STATE_OFF = 1,
		MOTOR_POWER_STATE_OFF = 1,
		STATE_ON = 2,
		MOTOR_POWER_STATE_ON = 2,
		STATE_POWERING_ON = 3,
		MOTOR_POWER_STATE_POWERING_ON = 3,
		STATE_POWERING_OFF = 4,
		MOTOR_POWER_STATE_POWERING_OFF = 4,
		STATE_ERROR = 5,
		MOTOR_POWER_STATE_ERROR = 5,
	}

	public enum ShorePowerState
	{
		STATE_UNKNOWN_SHORE_POWER = 0,
		SHORE_POWER_STATE_UNKNOWN = 0,
		STATE_ON_SHORE_POWER = 1,
		SHORE_POWER_STATE_ON = 1,
		STATE_OFF_SHORE_POWER = 2,
		SHORE_POWER_STATE_OFF = 2,
	}

	public enum RobotPowerState
	{
		ROBOT_POWER_STATE_UNKNOWN = 0,
		ROBOT_POWER_STATE_ON = 1,
		ROBOT_POWER_STATE_OFF = 2,
	}

	public enum PayloadPortsPowerState
	{
		PAYLOAD_PORTS_POWER_STATE_UNKNOWN = 0,
		PAYLOAD_PORTS_POWER_STATE_ON = 1,
		PAYLOAD_PORTS_POWER_STATE_OFF = 2,
	}

	public enum WifiRadioPowerState
	{
		WIFI_RADIO_POWER_STATE_UNKNOWN = 0,
		WIFI_RADIO_POWER_STATE_ON = 1,
		WIFI_RADIO_POWER_STATE_OFF = 2,
	}

	public DateTime timestamp;
	public MotorPowerState motorPowerState;
	public ShorePowerState shorePowerState;
	public double locomotionChargePercentage;
	public string locomotionEstimatedRuntime;
	public RobotPowerState robotPowerState;
	public PayloadPortsPowerState payloadPortsPowerState;
	public WifiRadioPowerState wifiRadioPowerState;
}

[Serializable]
public class BatteryState
{
	public enum Status
	{
		STATUS_UNKNOWN = 0,
		STATUS_MISSING = 1,
		STATUS_CHARGING = 2,
		STATUS_DISCHARGING = 3,
		STATUS_BOOTING = 4,
	}

	public DateTime timestamp;
	public string identifier;
	public double chargePercentage;
	public string estimatedRuntime;
	public double current;
	public double voltage;
	public double[] temperatures;
	public Status status;
}

[Serializable]
public class CommsState
{
	public DateTime timestamp;
	public WiFiState wifiState;
}

[Serializable]
public class WiFiState
{
	public enum Mode
	{
		MODE_UNKNOWN = 0,
		MODE_ACCESS_POINT = 1,
		MODE_CLIENT = 2,
	}

	public Mode currentMode;
	public string essid;
}

[Serializable]
public class SystemFaultState
{
	[Serializable]
	public class AggregatedEntry
	{
		public string key;
		public SystemFault.Severity value;
	}

	public SystemFault[] faults;
	public SystemFault[] historicalFaults;
	public AggregatedEntry[] aggregated;
}

[Serializable]
public class SystemFault
{
	public enum Severity
	{
		SEVERITY_UNKNOWN = 0,
		SEVERITY_INFO = 1,
		SEVERITY_WARN = 2,
		SEVERITY_CRITICAL = 3,
	}

	public string name;
	public DateTime onsetTimestamp;
	public string duration;
	public int code;
	public ulong uid;
	public string errorMessage;
	public string attributes;
	public Severity severity;
}

[Serializable]
public class EStopState
{
	public enum Type
	{
		TYPE_UNKNOWN = 0,
		TYPE_HARDWARE = 1,
		TYPE_SOFTWARE = 2,
	}

	public enum State
	{
		STATE_UNKNOWN = 0,
		STATE_ESTOPPED = 1,
		STATE_NOT_ESTOPPED = 2,
	}

	public DateTime timestamp;
	public string name;
	public Type type;
	public State state;
	public string stateDescription;
}

[Serializable]
public class KinematicState
{
	public JointState[] jointStates;
	public SE3Velocity velocityOfBodyInVision;
	public SE3Velocity velocityOfBodyInOdom;
	public DateTime acquisitionTimestamp;
	public FrameTreeSnapshot transformsSnapshot;
}

[Serializable]
public class JointState
{
	public string name;
	public double position;
	public double velocity;
	public double acceleration;
	public double load;
}

[Serializable]
public class SE3Velocity
{
	public Vector3Double linear;
	public Vector3Double angular;
}

[Serializable]
public class FrameTreeSnapshot
{
	[Serializable]
	public class ChildToParentEdgeMapEntry
	{
		public ParentEdge body;
		public ParentEdge vision;
		public ParentEdge odom;
		//NOTE: other entries may be provided but cannot be parsed dynamically due to JSON notation
	}

	[Serializable]
	public class ParentEdge
	{
		public string parentFrameName;
		public SE3Pose parentTformChild;
	}

	public ChildToParentEdgeMapEntry childToParentEdgeMap;
}

[Serializable]
public class SE3Pose
{
	public Vector3Double position;
	public QuaternionDouble rotation;
}

[Serializable]
public class BehaviorFaultState
{
	public BehaviorFault[] faults;
}

[Serializable]
public class BehaviorFault
{
	public enum Cause
	{
		CAUSE_UNKNOWN = 0,
		CAUSE_FALL = 1,
		CAUSE_HARDWARE = 2,
		CAUSE_LEASE_TIMEOUT = 3,
	}

	public enum Status
	{
		STATUS_UNKNOWN = 0,
		STATUS_CLEARABLE = 1,
		STATUS_UNCLEARABLE = 2,
	}

	public uint behaviorFaultId;
	public DateTime onsetTimestamp;
	public Cause cause;
	public Status status;
}

[Serializable]
public class FootState
{
    public Vector3Double footPositionRtBody;
    public enum Contact
	{
		CONTACT_UNKNOWN = 0,
		CONTACT_MADE = 1,
		CONTACT_LOST = 2,
	}

	[Serializable]
	public class TerrainFoot
	{
		public double groundMuEst;
		public string frameName;
		public Vector3Double footSlipDistanceRtFrame;
		public Vector3Double footSlipVelocityRtFrame;
		public Vector3Double groundContactNormalRtFrame;
		public double visualSurfaceGroundPenetrationMean;
		public double visualSurfaceGroundPenetrationStd;
	}

	
	public Contact contact;
	public TerrainFoot terrain;
}

[Serializable]
public class ManipulatorState
{
	public enum StowState
	{
		STOWSTATE_UNKNOWN = 0,
		STOWSTATE_STOWED = 1,
		STOWSTATE_DEPLOYED = 2,
	}

	public enum CarryState
	{
		CARRY_STATE_UNKNOWN = 0,
		CARRY_STATE_NOT_CARRIABLE = 1,
		CARRY_STATE_CARRIABLE = 2,
		CARRY_STATE_CARRIABLE_AND_STOWABLE = 3,
	}

	public double gripperOpenPercentage;
	public bool isGripperHoldingItem;
	public Vector3Double estimatedEndEffectorForceInHand;
	public StowState stowState;
	public SE3Velocity velocityOfHandInVision;
	public SE3Velocity velocityOfHandInOdom;
	public CarryState carryState;
}

[Serializable]
public class ServiceFaultState
{
	[Serializable]
	public class AggregatedEntry
	{
		public string key;
		public ServiceFault.Severity value;
	}

	public ServiceFault[] faults;
	public ServiceFault[] historicalFaults;
	public AggregatedEntry[] aggregated;
}

[Serializable]
public class ServiceFault
{
	public enum Severity
	{
		SEVERITY_UNKNOWN = 0,
		SEVERITY_INFO = 1,
		SEVERITY_WARN = 2,
		SEVERITY_CRITICAL = 3,
	}

	public ServiceFaultId faultId;
	public string errorMessage;
	public string[] attributes;
	public Severity severity;
	public DateTime onsetTimestamp;
	public string duration;
}

[Serializable]
public class ServiceFaultId
{
	public string faultName;
	public string serviceName;
	public string payloadGuid;
}

[Serializable]
public class TerrainState
{
	public bool isUnsafeToSit;
}

[Serializable]
public class RobotState
{
	public PowerState powerState;
	public BatteryState[] batteryStates;
	public CommsState[] commsStates;
	public SystemFaultState systemFaultState;
	public EStopState[] estopStates;
	public KinematicState kinematicState;
	public BehaviorFaultState behaviorFaultState;
	public FootState[] footState;
	public ManipulatorState manipulatorState;
	public ServiceFaultState serviceFaultState;
	public TerrainState terrainState;
}
