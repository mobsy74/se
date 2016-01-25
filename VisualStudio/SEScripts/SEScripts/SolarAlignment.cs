using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;

namespace SEScripts_SolarAlignment
{
    public class ExampleScipt : MyGridProgram
    {
        #region CodeEditor
        /*  
        Script for automatic rotor stop when a solar panel or oxygen farm has high output.  
        Version: 1.04d 
        Sigurd Hansen  
        */

        #region Configuration 
        // HighPwrValue - Set this to your minimum power requirement in kW per panel. (Or oxygen L/min) 
        //118 for solar, 1.7 for oxygen
        const float HighPwrValue = 118f;

        // NameOfSolar - Search for solar blocks or oxygen farms with this name. 
        //  Maximum one item per array should be returned. Multiple arrays supported. 
        const string NameOfSolar = "SolarMain";

        // NameOfRotorX - Search for rotor blocks with this name. Maximum one item per array should be returned. 
        //  Multiple arrays supported. 
        const string NameOfRotorX = "RotorX";

        // rotorspeed - Maximum speed of rotor. Will be dynamically adjusted down when close to target. 
        //  Recommended value: Less than 1.7f. Dedicated servers and large arrays may work better with a lower speed setting. 
        const float rotorspeed = 1.4f;

        // EnableTimerMgmt - Enable dynamic timer trigger management for better accuracy and performance. 
        //  Recommended value: true 
        const bool EnableTimerMgmt = true;

        // NameOfTimer - Set this to the timer responsible for triggering this program. 
        //  Timer should start this programming block, then start itself. 
        const string NameOfTimer = "SolarTimerBlock";

        // TimerIdleDelay - Delay of timer when idle. Do not remove "f". 
        const float TimerIdleDelay = 8f;

        // TimerActiveDelay - Delay of timer when active. Do not remove "f". 
        //  Recommended value: 2f. NEVER set below 2. 
        const float TimerActiveDelay = 2f;

        // EnableTwoAxisMode - Enable dual axis mode. 
        //  Recommended value: Depends on design. 
        const bool EnableTwoAxisMode = true;

        // NameOfRotorY - Search for this name for Y axis. Must only find Y axis rotors. 
        //  Maximum one item per array should be returned. Multiple arrays supported. 
        const string NameOfRotorY = "RotorY";

        // EnableOxygenMode - Enables oxygen mode. 
        const bool EnableOxygenMode = false;

        // Auto set torque and braking torque to best practice value. 
        //  Recommended value: true 
        const bool ForceAutoTorque = true;

        // Duplication 
        // Applying rotor values to other rotors as well if set to true. 
        //  If unsure, set all to false. Recommended value: Depends on design. 
        const bool EnableDuplicateRotor1 = false;
        const bool EnableDuplicateRotor2 = false;
        const bool EnableDuplicateRotor3 = false;

        // You might want to inverse some rotors, for example on the other side of the axis. 
        //  If so, set the value to true. If unsure change later if rotors lock at wrong angle. 
        const bool InverseDuplicateRotor1 = false;
        const bool InverseDuplicateRotor2 = false;
        const bool InverseDuplicateRotor3 = false;

        // Enter the name of the source rotors you want to duplicate. 
        const string NameOfDuplicateSource1 = "RotorY";
        const string NameOfDuplicateSource2 = "RotorY";
        const string NameOfDuplicateSource3 = "RotorF";

        // Enter the name of the destination rotors you want to duplicate. 
        //  The code will search for rotors. For example:  
        //  Entering RotorZ will duplicate changes to RotorZa, RotorZB, RotorZQ and so forth. 
        const string SearchForDuplicateDest1 = "RotorZ";
        const string SearchForDuplicateDest2 = "RotorH";
        const string SearchForDuplicateDest3 = "RotorG";

        // Auto night mode (Beta) 
        //  Turns off rotors if night is detected. Suboptimal axis towards sun might trigger night mode at daytime. 
        //  Use at your own risk. Not recommended for ships. 
        const bool AutoNightMode = false;

        // Changes below this line not recommended. 
        //------------------------------------------------------------  
        #endregion
        List<IMyTerminalBlock> solarBlocks = new List<IMyTerminalBlock>();
        List<IMyTerminalBlock> rotorBlocksX = new List<IMyTerminalBlock>();
        List<IMyTerminalBlock> rotorBlocksY = new List<IMyTerminalBlock>();
        List<IMyTerminalBlock> Rotors = new List<IMyTerminalBlock>();
        List<IMyTerminalBlock> SourceRotors = new List<IMyTerminalBlock>();
        IMyTimerBlock timer;

        bool firstrun = true;
        bool nightmode = false;

        #region Main 
        void Main()
        {
            if (firstrun)
            {
                GridTerminalSystem.SearchBlocksOfName(NameOfSolar, solarBlocks); // Search for Solar Blocks 
                GridTerminalSystem.SearchBlocksOfName(NameOfRotorX, rotorBlocksX); // Search for Rotor Blocks  
                GridTerminalSystem.SearchBlocksOfName(NameOfRotorY, rotorBlocksY); // Search for RotorY Blocks  
                timer = GridTerminalSystem.GetBlockWithName(NameOfTimer) as IMyTimerBlock;
                if (timer == null && EnableTimerMgmt) { throw new Exception("Cannot find timer. Check timer name and recompile."); }
                if (solarBlocks.Count == 0) { throw new Exception("Cannot find solar panel. Check name and recompile."); }
                if (rotorBlocksX.Count == 0) { throw new Exception("Cannot find x-axis rotor blocks. Check name and recompile."); }
                if (rotorBlocksY.Count == 0 && EnableTwoAxisMode) { throw new Exception("Cannot find y-axis rotor blocks. Check name and recompile."); }
                if (rotorBlocksY.Count < rotorBlocksX.Count && EnableTwoAxisMode)
                {
                    int diff = rotorBlocksX.Count - rotorBlocksY.Count;
                    throw new Exception(diff + " Y-axis rotors missing. Fix and recompile.");
                }
                if (solarBlocks.Count > rotorBlocksX.Count) { throw new Exception("Too many solar panels found. Check solar panel names."); }
                Echo("Initializing core...\nX rotors: " + rotorBlocksX.Count.ToString() + "\nY rotors: " + rotorBlocksY.Count.ToString());
                Echo("Solar panels/Oxygen farms: " + solarBlocks.Count.ToString());
                if (EnableTwoAxisMode) Echo("Dual Axis mode enabled");
                if (EnableTimerMgmt) Echo("Timer management enabled");
                if (EnableOxygenMode) Echo("Oxygen Farm mode ON");
                if (EnableDuplicateRotor1 || EnableDuplicateRotor2 || EnableDuplicateRotor3) Echo("Duplication activated.");
                firstrun = false;
            }

            bool[] rotorOn = new bool[rotorBlocksX.Count]; // Stop or start rotor  
            bool[] rotorLow = new bool[rotorBlocksX.Count]; // True when power is way to low  
            bool[] rotorFineTune = new bool[rotorBlocksX.Count]; // Fine Tune, very slow rotor  
            bool[] reverse = new bool[rotorBlocksX.Count]; // Reverse rotor	  
            bool[] rotorOnY = new bool[rotorBlocksY.Count]; // Stop or start rotor  

            bool containsFalse = false; // Dynamic timer management. Increase or decrease timer  
            float pwr = 0f, lastPwr = 0f; // Current and last power reading  

            if (EnableOxygenMode)
            {
                for (int i = 0; i < solarBlocks.Count; i++)
                { // For each oxygen farm...  
                    var solar = solarBlocks[i] as IMyOxygenFarm; // Support for oxygen farm  
                    if (solar != null)
                    { // Yes I am  
                        GetOxygen(solar, ref pwr); // Get oxygen level, return into existing pwr variable  
                        lastPwr = GetAndSetLastOxygen(solar, pwr); // Get and set last runs oxygen level  
                        reverse[i] = (lastPwr < pwr || pwr == 0) ? false : true; // Change rotor direction  
                        rotorOn[i] = (pwr <= HighPwrValue) ? true : false; // Turn on rotor  
                        rotorLow[i] = (pwr < HighPwrValue / 2) ? true : false; // Slow or fast rotor  
                        rotorFineTune[i] = (pwr > HighPwrValue * 10 / 11) ? true : false; // Fine tune rotor  
                    }
                }
            }
            else {
                for (int i = 0; i < solarBlocks.Count; i++)
                { // For each solar panel...  
                    var solar = solarBlocks[i] as IMySolarPanel; // I am a Solar Panel  
                    if (solar != null)
                    { // Yes I am  
                        GetPower(solar, ref pwr); // Get Power from solar panel, return into existing pwr variable  
                        lastPwr = GetAndSetLastPwr(solar, pwr); // Get and set last runs power  
                        reverse[i] = (lastPwr < pwr || pwr == 0) ? false : true; // Change rotor direction  
                        rotorOn[i] = (pwr <= HighPwrValue) ? true : false; // Turn on rotor  
                        rotorLow[i] = (pwr < HighPwrValue / 2) ? true : false; // Slow or fast rotor  
                        rotorFineTune[i] = (pwr > HighPwrValue * 10 / 11) ? true : false; // Fine tune rotor 
                        if (lastPwr == 0 && pwr == 0 && AutoNightMode || Me.TerminalRunArgument == "Nightmode")
                        { // TEST 
                            nightmode = true;
                        }
                        else {
                            nightmode = false;
                        }
                    }
                }
            }
            if (nightmode)
            {
                Echo("Night mode.");
                // Do other stuff 
            }

            for (int i = 0; i < rotorBlocksX.Count; i++)
            { // For each rotorX...  
                IMyMotorStator rotor = rotorBlocksX[i] as IMyMotorStator; // I am a Rotor  
                if (rotor != null)
                { // Yes I am 
                    if (ForceAutoTorque)
                    {
                        rotor.SetValueFloat("BrakingTorque", 36000000); // Force torque. 
                        rotor.SetValueFloat("Torque", 30000000); // Force torque. 
                    }
                    if (nightmode)
                    {
                        TriggerRotor(rotor, false, false, ref containsFalse); // Stop rotor  
                    }
                    else {
                        SetRotorSpeed(rotor, rotorLow[i], rotorFineTune[i]); // Dynamic rotor speed  
                        if (!rotorOn[i])
                        { // Turn off...  
                            TriggerRotor(rotor, false, false, ref containsFalse); // Stop rotor  
                        }
                        else if (rotorOn[i] && EnableTwoAxisMode && reverse[i])
                        { // Turn On, dual axis mode, and reverse  
                            rotorOnY[i] = CheckAndUpdateRotorName(rotor);
                            if (rotorOnY[i])
                            {
                                TriggerRotor(rotor, false, false, ref containsFalse); // Y on, therefore X off.  
                            }

                            if (!rotorOnY[i])
                            {
                                TriggerRotor(rotor, true, reverse[i], ref containsFalse); // Y off, therefore X on, and reverse.  
                            }
                        }
                        else if (rotorOn[i] && EnableTwoAxisMode && !reverse[i])
                        {
                            rotorOnY[i] = CheckAndUpdateRotorName(rotor);
                            if (!rotorOnY[i])
                            {
                                TriggerRotor(rotor, true, reverse[i], ref containsFalse); // Start rotor. Reverse if needed  
                            }
                        }
                        else {
                            TriggerRotor(rotor, true, reverse[i], ref containsFalse); // Start rotor. Reverse if needed  
                        }
                    }
                }
            }

            if (EnableTwoAxisMode)
            {
                for (int i = 0; i < rotorBlocksY.Count; i++)
                { // For each rotorY...  
                    IMyMotorStator rotor = rotorBlocksY[i] as IMyMotorStator; // I am a Rotor  
                    if (rotor != null)
                    { // Yes I am 
                        if (ForceAutoTorque)
                        {
                            rotor.SetValueFloat("BrakingTorque", 36000000); // Force torque. Causes too much support without it 
                            rotor.SetValueFloat("Torque", 30000000); // 
                        }
                        if (nightmode)
                        {
                            TriggerRotor(rotor, false, false, ref containsFalse); // Stop rotor  
                        }
                        else {
                            if (rotorOnY[i] == true)
                            {
                                SetRotorSpeed(rotor, rotorLow[i], rotorFineTune[i]); // Dynamic rotor speed  
                                TriggerRotor(rotor, true, reverse[i], ref containsFalse); // Start rotor Y. Reverse if needed  
                            }
                            else {
                                TriggerRotor(rotor, false, false, ref containsFalse);
                            }
                        }
                    }
                }
            }

            if (containsFalse && EnableTimerMgmt)
            { // Dynamic timer 
                if (timer.TriggerDelay > TimerActiveDelay)
                {
                    AdjustTriggerDelay(timer, false); // Decrease Timer Delay  
                }
            }
            else {
                if (timer.TriggerDelay < TimerIdleDelay && EnableTimerMgmt)
                {
                    AdjustTriggerDelay(timer, true); // Increase Timer Delay  
                }
            }
            // Duplicate stuff 
            if (EnableDuplicateRotor3 && !containsFalse)
            { // Main rotors aligned, lets duplicate changes...   
                RotorDuplicate(NameOfDuplicateSource3, SearchForDuplicateDest3, InverseDuplicateRotor3);
            }
            if (EnableDuplicateRotor2 && !containsFalse)
            { // Main rotors aligned, lets duplicate changes... 
                RotorDuplicate(NameOfDuplicateSource2, SearchForDuplicateDest2, InverseDuplicateRotor2);
            }
            if (EnableDuplicateRotor1 && !containsFalse)
            { // Main rotors aligned, lets duplicate changes... 
                RotorDuplicate(NameOfDuplicateSource1, SearchForDuplicateDest1, InverseDuplicateRotor1);
            }
        }
        #endregion

        #region Methods 

        void AdjustTriggerDelay(IMyTimerBlock timer, bool Increase)
        {
            if (Increase) { timer.SetValue("TriggerDelay", TimerIdleDelay); } // Increase Timer Trigger Delay  
            else { timer.SetValue("TriggerDelay", TimerActiveDelay); } // Decrease Timer Trigger Delay  
        }

        void TriggerRotor(IMyMotorStator rotor, bool state, bool reverse, ref bool containsFalse)
        {
            if (!state)
            {
                rotor.GetActionWithName("OnOff_Off").Apply(rotor); // Stop rotor  
            }
            else {
                rotor.GetActionWithName("OnOff_On").Apply(rotor); // Start rotor  
                containsFalse = true; // Adjust timer for better accuracy  
                if (reverse) { rotor.GetActionWithName("Reverse").Apply(rotor); }
            }
        }

        void GetPower(IMySolarPanel solar, ref float pwr)
        {
            string value = "";
            string type = "";
            System.Text.RegularExpressions.Regex matchthis = new System.Text.RegularExpressions.Regex(@"^.+\n.+\:\s?([0-9\.]+)\s(.*)\n.+$");
            System.Text.RegularExpressions.Match match = matchthis.Match(solar.DetailedInfo);
            if (match.Success)
            {
                value = match.Groups[1].Value;
                type = match.Groups[2].Value;
                Echo(value + " " + type);
            }
            else throw new Exception("Can't parse DetailedInfo with regex");
            bool test = float.TryParse(value, out pwr); // Get power into variable  
            if (type == "W") { pwr /= 1000; } // Make sure power is in kW 
            if (type == "MW") { pwr *= 1000; } // Make sure power is in kW  
            if (!test) { throw new Exception("Can't parse power reading from solar panel: " + value); }
        }

        void GetOxygen(IMyOxygenFarm solar, ref float pwr)
        {
            string value = "";
            System.Text.RegularExpressions.Regex matchthis = new System.Text.RegularExpressions.Regex(@"^.+\n.+\n.+\:\s?([0-9\.]+)\s?L.*$");
            System.Text.RegularExpressions.Match match = matchthis.Match(solar.DetailedInfo);
            if (match.Success)
            {
                value = match.Groups[1].Value;
                Echo(value + " L/min");
            }
            else
            {
                Echo("Assuming 0; Can't parse DetailedInfo: " + value);
                value = "0";
            }
            bool test = float.TryParse(value, out pwr); // Get oxygen into variable  
            if (!test) { throw new Exception("Can't parse reading from oxygen farm\n"); }
        }

        float RotorPosition(IMyMotorStator Rotor)
        {
            if (Rotor == null)
            {
                Echo("Rotor not found. Returning 0");
                return 0;
            }
            string value = "";
            System.Text.RegularExpressions.Regex matchthis = new System.Text.RegularExpressions.Regex(@"^.+\n.+\:\s?(-?[0-9]+).*[\s\S]*$");
            System.Text.RegularExpressions.Match match = matchthis.Match(Rotor.DetailedInfo);
            if (match.Success)
            {
                value = match.Groups[1].Value;
            }
            else
            {
                Echo("Assuming 0; Can't parse Rotor DetailedInfo: " + value);
                value = "0";
            }
            float RotorOutput = float.Parse(value);
            return RotorOutput;
        }

        void RotorDuplicate(string SourceRotorName, string RotorName, bool Inverse)
        {
            GridTerminalSystem.SearchBlocksOfName(SourceRotorName, SourceRotors); // Search for Solar Source Blocks 
            IMyMotorStator SourceRotor = SourceRotors[0] as IMyMotorStator; // I am a Rotor 
            float SetPosition = RotorPosition(SourceRotor);
            GridTerminalSystem.SearchBlocksOfName(RotorName, Rotors); // Search for Solar Blocks  
            if (Rotors.Count == 0)
            {
                Echo("Cannot find any duplicate destination rotors.");
                return;
            }
            if (Inverse)
            {
                SetPosition = -SetPosition;
            }
            for (int i = 0; i < Rotors.Count; i++)
            { // For each rotor... 
                IMyMotorStator Rotor = Rotors[i] as IMyMotorStator; // I am a Rotor 
                float DestPosition = RotorPosition(Rotor);
                if (ForceAutoTorque)
                {
                    Rotor.SetValueFloat("BrakingTorque", 36000000);
                    Rotor.SetValueFloat("Torque", 10000000);
                }
                Rotor.SetValueFloat("LowerLimit", SetPosition);
                Rotor.SetValueFloat("UpperLimit", SetPosition);
                if (SetPosition == DestPosition)
                {
                    Rotor.SetValueFloat("Velocity", 0f);
                    Rotor.GetActionWithName("OnOff_Off").Apply(Rotor); // Stop rotor  
                }
                else if (SetPosition < DestPosition)
                {
                    Rotor.GetActionWithName("OnOff_On").Apply(Rotor); // Start rotor  
                    Rotor.SetValueFloat("Velocity", -rotorspeed);
                }
                else if (SetPosition > DestPosition)
                {
                    Rotor.GetActionWithName("OnOff_On").Apply(Rotor); // Start rotor  
                    Rotor.SetValueFloat("Velocity", rotorspeed);
                }
            }
        }

        void SetRotorSpeed(IMyMotorStator rotor, bool fast, bool FineTune)
        {
            float SetTo = 0f;
            bool VelocityPositive = (rotor.GetValueFloat("Velocity") > 0f) ? true : false; // True if positive velocity  
            if (fast)
            { // Under half of required power from solar panel. Will increase speed  
                SetTo = (VelocityPositive) ? rotorspeed : -rotorspeed;
            }
            else if (FineTune)
            { // Fine tune speed  
                SetTo = (VelocityPositive) ? rotorspeed / 3.7f : -rotorspeed / 3.7f;
            }
            else { // Not far from required power. Lower speed for increased accuracy  
                SetTo = (VelocityPositive) ? rotorspeed / 1.7f : -rotorspeed / 1.7f;
            }
            rotor.SetValue("Velocity", SetTo);
        }

        float GetAndSetLastPwr(IMySolarPanel solar, float CurrentPower)
        {
            float OldPwr = 0f;
            string[] words = solar.CustomName.Split(':'); // Colon split words  
            if (words.Length > 1)
            {   // If there is data after colon  
                if (!float.TryParse(words[1], out OldPwr)) { OldPwr = 0f; } // Try to get data into float variable  
            }
            solar.SetCustomName(words[0] + ":" + CurrentPower); // Set current power in solar panel name  
            return OldPwr;
        }

        float GetAndSetLastOxygen(IMyOxygenFarm solar, float CurrentPower)
        {
            float OldPwr = 0f;
            string[] words = solar.CustomName.Split(':'); // Colon split words  
            if (words.Length > 1)
            {   // If there is data after colon  
                if (!float.TryParse(words[1], out OldPwr)) { OldPwr = 0f; } // Try to get data into float variable  
            }
            solar.SetCustomName(words[0] + ":" + CurrentPower); // Set current power in solar panel name  
            return OldPwr;
        }

        bool CheckAndUpdateRotorName(IMyMotorStator rotor)
        {
            int OldCount = 0;
            string[] words = rotor.CustomName.Split(':'); // Colon split words  
            if (words.Length > 1)
            {   // If there is data after colon  
                if (!int.TryParse(words[1], out OldCount)) { OldCount = 0; } // Try to get data into int variable  
            }
            int NewCount = OldCount + 1;
            if (OldCount > 6)
            {
                rotor.SetCustomName(words[0] + ":0");
                return true;
            }
            else if (OldCount >= 3)
            {
                rotor.SetCustomName(words[0] + ":" + NewCount); // Set current count in rotor name  
                return true;
            }
            else {
                rotor.SetCustomName(words[0] + ":" + NewCount); // Set current count in rotor name  
                return false;
            }
        }
        #endregion

        #endregion
    }
}
