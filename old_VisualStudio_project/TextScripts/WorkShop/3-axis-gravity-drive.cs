// 3-axis Gravity Drive 
//  
// pilot gravity drive driven ship in full 3-axis using standard WASD movement 
// 
// gets power usage of each directional thruster by enabling only one thruster per physics cycle 
// using power usage values, determines direction ship is attempting to travel in 
// enables gravity generators in the direction corresponding to the thrusters in use 
// 
// issues: 
// If programming block is disabled at world save, on world load programming block is stuck off. Workaround by enabling block then grinding down and repairing block to reset ownership. (Remember to recompile code) 
// Difficulty coming to a complete stop. Will get under STOP_SPEED_LIMIT then drift at that speed occasionally cycling grav drive. Enable thrusters (by using Timer Disable Grav) to come to a complete stop with thrusters only. 
// "speed" variable is a scalar, should be a vector based on current ship heading. Would fix non-current direction constantly toggling back and forth. 
// 
// Setup: 
// Directions: "Forward", "Backward", "Left", "Right", "Up", "Down" 
// 6 small thrusters, 1 per direction, grouped, each named containing "Thruster <direction>". Do NOT rely on the games autonaming of thrusters eg. "Thruster Forward" is acceptable, "Thruster (Forward)" is not 
// Gravity Generators, grouped, each named containing the direction such that when gravity is set to +1, will push ship in that direction  
// mass blocks, grouped 
// programming block 
// Timer Block - Grav Enable, hotkey to Trigger Now: programming block - enable, thruster group - disable, Timer Gravity Drive - trigger now 
// Timer Block - Grav Disable, hotkey to Trigger Now: programming block - disable, thruster group - enable, gravity generators group - disable, mass blocks group - disable, Timer Disable Delay - run now 
// Timer Block - Gravity Drive, set for 1 second: Timer Gravity Drive - trigger now, Timer Gravity Drive - start, programming block - run  (trigger now AND start are needed for world saves/reloads to continue script running) 
// Timer Block - Grav Disable Delay: thruster group - enable (needed due to programming block enable/disable being delayed 1 cycle) 
// 
 
 
IMyProgrammableBlock thisBlock; 
 
List<IMyTerminalBlock> myGravityGenerators = new List<IMyTerminalBlock>(); 
List<IMyTerminalBlock> myMassBlocks = new List<IMyTerminalBlock>(); 
List<IMyTerminalBlock> myThrusters = new List<IMyTerminalBlock>(); 
 
const float THRUSTER_ERROR_FACTOR = 10000f;		// error factor in power reading caused by power output not giving exact values (eg. 2.21 MW) 
const float THRUSTER_DEFAULT_POWER = 560000f;	// power used by one thruster under normal load (not dampeners) 
const float THRUSTER_DEFAULT_POWER_LOW = THRUSTER_DEFAULT_POWER - THRUSTER_ERROR_FACTOR;	// power usage thresholds for normal thrust 
const float THRUSTER_DEFAULT_POWER_HIGH =  THRUSTER_DEFAULT_POWER + THRUSTER_ERROR_FACTOR; 
 
const string CURRENT_OUTPUT = "Current Output:";  // localized string power output 
 
const int CYCLE_POWER_DELAY = 3;	// number of cycles before a change to enabled blocks affect power usage 
const int CYCLE_LENGTH = 6;			// number of cycles in the loop 
const int THRUSTER_COUNT = 6;		// two per axis 
const int CHECK_GRAV_ON_CYCLE = CYCLE_POWER_DELAY-1;	// check after previous cycles power changes have been applied 
const int SET_GRAV_ON_CYCLE = CHECK_GRAV_ON_CYCLE+((CHECK_GRAV_ON_CYCLE+CYCLE_POWER_DELAY) % 2);	// apply gravity such that power usage kicks in during even cycles at start of a thruster axis 
 
const int CALCS_PER_SECOND = 60; 
const int SPEED_MULTIPLIER = CALCS_PER_SECOND/CYCLE_LENGTH;		// how often is current speed checked 
const float STOP_SPEED_LIMIT = 1.5f;	// speed under which check only for normal thrust (no dampeners) 
 
float[] currentPowerUsage = new float[CYCLE_LENGTH];	// power usage per cycle 
int[] gravNeeded = new int[3] {0, 0, 0};	// gravity needed per axis 
int[] gravNeededPrev = new int[3] {0, 0, 0};	// previous cycles gravity needed 
 
int cycle = 0;		// where in the cycle we are 
Vector3 position;	// save previous position for speed calculation 
 
bool init = false;	// first run initialization 
 
const string THRUSTER_PREFIX = "Thruster "; 	// prefix for thruster naming convention, needs to be immediately followed by a direction 
string[] directions = new string[] {"Forward", "Backward", "Left", "Right", "Up", "Down"};	// strings for thruster and gravity generator names 
 
void Main() 
{ 
	if (!init) 
	{ 
		doInit(); 
		return; 
	} 
	 
	// Get a list of thrusters 
	GridTerminalSystem.GetBlocksOfType<IMyThrust>(myThrusters, FilterCurrentShip); 
 
	// which thrusters are being set for this cycle 
	int prevThrusterNum = (cycle-1+CYCLE_LENGTH) % CYCLE_LENGTH; 
	int powerThrusterNum = (cycle-CYCLE_POWER_DELAY+CYCLE_LENGTH) % CYCLE_LENGTH; 
 
	// power usage on this cycle 
	currentPowerUsage[powerThrusterNum] = GetPowerUsage(); 
 
	if (cycle == CHECK_GRAV_ON_CYCLE) 
	{ 
		// get current position and speed 
		Vector3 newPosition = thisBlock.GetPosition(); 
		float speed = Vector3.Distance(position, newPosition) * SPEED_MULTIPLIER; 
		position = newPosition; 
		 
		// based on speed and power usage, which direction of gravity generators need to be enabled 
		for (int i = 0; i < 3; i++) 
		{ 
			float power = currentPowerUsage[i*2] - currentPowerUsage[i*2+1]; 
			// if were under the stop speed, make sure power usage is within standard thrust thresholds 
			if ((Math.Abs(power) < THRUSTER_DEFAULT_POWER_LOW) || ((speed < STOP_SPEED_LIMIT) && Math.Abs(power) > THRUSTER_DEFAULT_POWER_HIGH)) 
				gravNeeded[i] = 0; 
			else 
				gravNeeded[i] = Math.Sign(power); 
		} 
	} 
	if (cycle == SET_GRAV_ON_CYCLE) 
	{ 
		// make a list of axis and see if grav is needed in each direction 
		List<int> gravAxisCheck = new List<int>(); 
		for (int i = 0; i < 3; i++) 
			if (gravNeeded[i] != gravNeededPrev[i]) 
				gravAxisCheck.Add(i); 
		// set gravity on each needed axis 
		if (gravAxisCheck.Count > 0) 
		{ 
			GridTerminalSystem.GetBlocksOfType<IMyGravityGenerator>(myGravityGenerators, FilterCurrentShip); 
			for (int i = 0; i < myGravityGenerators.Count; i++) 
			{ 
				for (int j = 0; j < gravAxisCheck.Count; j++) 
				{ 
					if (myGravityGenerators[i].CustomName.Contains(directions[gravAxisCheck[j]*2])) 
					{ 
						SetGravDrive((IMyGravityGenerator)myGravityGenerators[i], gravNeeded[gravAxisCheck[j]]); 
						break; 
					} 
					else if (myGravityGenerators[i].CustomName.Contains(directions[gravAxisCheck[j]*2+1])) 
					{ 
						SetGravDrive((IMyGravityGenerator)myGravityGenerators[i], -gravNeeded[gravAxisCheck[j]]); 
						break; 
					} 
				} 
			} 
		}	 
		// enable/disable mass blocks if they're needed 
		GridTerminalSystem.GetBlocksOfType<IMyVirtualMass>(myMassBlocks, FilterCurrentShip); 
		for (int i = 0; i < myMassBlocks.Count; i++) 
			myMassBlocks[i].GetActionWithName(((gravNeeded[0] == 0 && gravNeeded[1] == 0 && gravNeeded[2] == 0) ? "OnOff_Off" : "OnOff_On")).Apply(myMassBlocks[i]); 
			 
		Array.Copy(gravNeeded, gravNeededPrev, 3); 
	} 
 
	// rotate enabled thruster 
	ThrusterEnable(prevThrusterNum, false); 
	ThrusterEnable(cycle, true); 
	cycle = (cycle+1) % CYCLE_LENGTH; 
} 
 
void doInit() 
{ 
	init = true; 
	List<IMyTerminalBlock> programmables = new List<IMyTerminalBlock>(); 
	GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(programmables, FilterProgrammablesRunning); 
	if (programmables.Count != 1) 
		throw new Exception("Cannot find this block"); 
	thisBlock = (programmables[0] as IMyProgrammableBlock); 
 
	// initialize current position 
	position = thisBlock.GetPosition(); 
} 
void dbg(string m) 
{ 
	List<IMyTerminalBlock> l = new List<IMyTerminalBlock>();  
	GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(l, FilterCurrentShip); 
	l[0].SetCustomName(l[0].CustomName + (l[0].CustomName.Length == 0 ? "" : "\n") + m); 
} 
void dbgClear() 
{ 
	List<IMyTerminalBlock> l = new List<IMyTerminalBlock>();  
	GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(l, FilterCurrentShip); 
	l[0].SetCustomName(""); 
} 
 
// sets gravity value and disables/enables generator as needed 
void SetGravDrive(IMyGravityGenerator gravGen, float gravVal) 
{ 
	gravGen.SetValue("Gravity", gravVal); 
	gravGen.GetActionWithName((gravVal == 0f ? "OnOff_Off" : "OnOff_On")).Apply(gravGen); 
} 
 
// enables/disables given thruster 
void ThrusterEnable(int facing, bool enable) 
{ 
	IMyThrust thruster = GetThruster(facing); 
	if (thruster != null) 
		thruster.RequestEnable(enable); 
} 
 
// finds thruster based on name 
IMyThrust GetThruster(int facing) 
{ 
	for (int i = 0; i < myThrusters.Count; i++) 
		if (myThrusters[i].CustomName.Contains(THRUSTER_PREFIX+directions[facing])) return (myThrusters[i] as IMyThrust); 
	return null; 
} 
 
// finds power output from detailedinfo strings returned by reactors, etc 
float GetPowerFromString(string detailedInfo) 
{ 
	int start = detailedInfo.IndexOf(CURRENT_OUTPUT)+CURRENT_OUTPUT.Length+1; 
	int end = detailedInfo.IndexOf(" ", start); 
	char unit = detailedInfo[end+1]; 
	float p = 0; 
	float.TryParse(detailedInfo.Substring(start, end-start), out p); 
	return p * (float)Math.Pow(1000f, " kMGTPEZY".IndexOf(unit)); 
} 
 
// loops through all power generating blocks to find overall power usage 
float GetPowerUsage() 
{ 
	List<IMyTerminalBlock> powerBlocks = new List<IMyTerminalBlock>(); 
	List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>(); 
	GridTerminalSystem.GetBlocksOfType<IMyReactor>(powerBlocks); 
	GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks); 
	powerBlocks.AddRange(blocks); 
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(blocks); 
	powerBlocks.AddRange(blocks); 
	float reactorPower = 0; 
	for (int i = 0; i < powerBlocks.Count; i++) 
	{ 
		reactorPower += GetPowerFromString(powerBlocks[i].DetailedInfo); 
	} 
	return reactorPower; 
} 
 
bool FilterCurrentShip(IMyTerminalBlock block) 
{ 
	return block.CubeGrid == thisBlock.CubeGrid; 
} 
bool FilterProgrammablesRunning(IMyTerminalBlock block) 
{ 
	return (block as IMyProgrammableBlock).IsRunning; 
}