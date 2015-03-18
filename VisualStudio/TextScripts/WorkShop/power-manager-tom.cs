using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common.ObjectBuilders;
using VRageMath;
using VRage;

namespace SpaceEngineersScripting_PowerManagerTom
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor

        //Power regex for Solar Panels and Reactors
        System.Text.RegularExpressions.Regex pwrRegex = new System.Text.RegularExpressions.Regex(
        "Max Output: (\\d+\\.?\\d*) (\\w?)W.*Current Output: (\\d+\\.?\\d*) (\\w?)W"
        , System.Text.RegularExpressions.RegexOptions.Singleline);

        //regex altered from the above, this will detect stored and available power on a battery. 
        System.Text.RegularExpressions.Regex batteryRegex = new System.Text.RegularExpressions.Regex(
        "Max Stored Power: (\\d+\\.?\\d*) (\\w?)Wh.*Current Input: (\\d+\\.?\\d*) (\\w?)W.*Stored power: (\\d+\\.?\\d*) (\\w?)Wh"
        , System.Text.RegularExpressions.RegexOptions.Singleline);

        static string solarGroup = "Solar Panels: Borealis";
        static string batteryGroup = "Batteries: Borealis";
        static string reactorGroup = "Reactors: Borealis";

        static string outputScreen = "Script Output Screen";

        public void clearOutputScreen(string screenName)
        {
            IMyTextPanel screen = GridTerminalSystem.GetBlockWithName(screenName) as IMyTextPanel;
            screen.WritePublicText("");
        }

        public void print(string screenName, string message)
        {
            IMyTextPanel screen = GridTerminalSystem.GetBlockWithName(screenName) as IMyTextPanel;
            screen.WritePublicText(message + "\n", true);
        }

        void Main()
        {
            clearOutputScreen(outputScreen);
            //want battery set to charge if solar panels are outputting enough excess power to charge them. 
            //want battery set to discharge if solar panels are not outputting enough excess power(to save uranium) 
            //(stretch) enable reactors if batteries are all set to "discharge" and still not producing enough power. 
            //batteries should never charge from reactors. 

            //time to wait after shutting batteries down to check solar panels. 
            //int holdTime = 100;
            //without this delay, the solar panels won't update their output fast enough. 
            //100ms is sufficient, less might work. 

            //Grab all solar panels, batteries, and reactors on the grid. 
            var solars = new List<IMyTerminalBlock>();
            var batteries = new List<IMyTerminalBlock>();
            var reactors = new List<IMyTerminalBlock>();

            List<IMyBlockGroup> groups = GridTerminalSystem.BlockGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == solarGroup)
                {
                    solars = groups[i].Blocks;
                }
                if (groups[i].Name == batteryGroup)
                {
                    batteries = groups[i].Blocks;
                }
                if (groups[i].Name == reactorGroup)
                {
                    reactors = groups[i].Blocks;
                }
            }

            double pwrMaxSum = 0.0;
            double pwrNowSum = 0.0;
            double solarPowerRatio = 0.0;
            double batteryPowerRatio = 0.0;
            double batteryCharge = 0.0;

            //These lines are to grab antennas for text ouptut. 
            //Don't use them if you don't want your antennas getting randomly renamed. 
            //    var antennas = new List<IMyTerminalBlock>(); 
            //    GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(antennas); 

            //Get total solar power ratio(thanks again 10Jin) 
            for (int i = 0; i < solars.Count; i++)
            {
                System.Text.RegularExpressions.Match match = pwrRegex.Match(solars[i].DetailedInfo);
                Double n;
                if (match.Success)
                {
                    if (Double.TryParse(match.Groups[1].Value, out n))
                        pwrMaxSum += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[2].Value));

                    if (Double.TryParse(match.Groups[3].Value, out n))
                        pwrNowSum += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[4].Value));
                }
            }
            //get total power draw due to battery charging 
            for (int i = 0; i < batteries.Count; i++)
            {
                System.Text.RegularExpressions.Match match = batteryRegex.Match(batteries[i].DetailedInfo);
                if (match.Success)
                {
                    Double n;
                    if (Double.TryParse(match.Groups[3].Value, out n))
                        batteryCharge += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[4].Value));
                }
            }
            pwrNowSum -= batteryCharge;
            //make sure any solar panels were found. 
            if (pwrMaxSum > 0.0)
            {
                solarPowerRatio = pwrNowSum / pwrMaxSum;
                //antennas[0].SetCustomName("Solar power usage ratio: " + solarPowerRatio); 
            }
            else
            {
                solarPowerRatio = 9999;
                //antennas[0].SetCustomName("Error: no solar energy being produced."); 
            }

            print(outputScreen, "pwrNowSum: " + pwrNowSum.ToString());
            print(outputScreen, "pwrMaxSum: " + pwrMaxSum.ToString());
            print(outputScreen, "solarPowerRatio: " + solarPowerRatio.ToString());

            if (solarPowerRatio < 0.8)//approximate cutoff due to floating-point weirdness 
            {
                //Turn the reactors off(to save uranium) 
                for (int i = 0; i < reactors.Count; i++)
                {
                    reactors[i].GetActionWithName("OnOff_Off").Apply(reactors[i]);
                }
                //grab all batteries, set them to "charge." 
                for (int i = 0; i < batteries.Count; i++)
                {
                    //turn battery on because it was turned off for the solar check. 
                    //            batteries[i].GetActionWithName("OnOff_On").Apply(batteries[i]); 
                    bool recharge = true;
                    //Fun fact: Batteries don't have a field to tell you whether they're in "charge" mode or not! 
                    //Fortunately, the DetailedInfo string will tell you how long it will take to recharge or discharge. 
                    //checking which of these messages is in use will let us know whether we're 
                    //charging or discharging. 
                    string batteryInfo = batteries[i].DetailedInfo;
                    recharge = batteryInfo.Contains("recharged");
                    //if discharging 
                    if (!recharge)
                    {
                        batteries[i].GetActionWithName("Recharge").Apply(batteries[i]);
                    }
                }
            }
            else//battery power needed. 
            {
                print(outputScreen, "battery power needed");
                for (int i = 0; i < batteries.Count; i++)
                {
                    //turn battery on because it was turned off for the solar check. 
                    batteries[i].GetActionWithName("OnOff_On").Apply(batteries[i]);
                    bool recharge = true;
                    //Check whether we're currently in recharge mode 
                    string batteryInfo = batteries[i].DetailedInfo;
                    recharge = batteryInfo.Contains("recharged");
                    //if it is, switch it out of recharge mode. 
                    if (recharge)
                    {
                        batteries[i].GetActionWithName("Recharge").Apply(batteries[i]);
                        print(outputScreen, "all the way in");
                    }
                }
                //check battery current/max 
                for (int i = 0; i < batteries.Count; i++)
                {
                    //filter out batteries that are empty from being counted among supply. 
                    IMyBatteryBlock thisBattery = batteries[i] as IMyBatteryBlock;
                    if (thisBattery.HasCapacityRemaining)
                    {
                        System.Text.RegularExpressions.Match match = batteryRegex.Match(batteries[i].DetailedInfo);
                        Double n;

                        if (match.Success)
                        {
                            //check if battery is "empty."(<0.5% charge) 
                            Double pwrMaxStored = 0.0;
                            Double pwrNowStored = 0.0;
                            Double pwrStoredPercent = 0.0;

                            if (Double.TryParse(match.Groups[1].Value, out n))
                                pwrMaxStored = n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[2].Value));

                            if (Double.TryParse(match.Groups[3].Value, out n))
                                pwrNowStored += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[4].Value));

                            pwrStoredPercent = pwrNowStored / pwrMaxStored * 100;
                            //antennas[0].SetCustomName("Battery is at " + pwrStoredPercent + " percent of capacity."); 

                            if (pwrStoredPercent > 0.5)
                            {
                                //there's enough battery power to be usable, check the output. 
                                match = batteryRegex.Match(batteries[i].DetailedInfo);
                                if (match.Success)
                                {
                                    if (Double.TryParse(match.Groups[1].Value, out n))
                                        pwrMaxSum += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[2].Value));

                                    if (Double.TryParse(match.Groups[3].Value, out n))
                                        pwrNowSum += n * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(match.Groups[4].Value));
                                }
                            }
                        }
                    }
                }
                //if THAT's above 0.99, turn on the reactors. 
                if (pwrMaxSum > 0.0)
                {
                    batteryPowerRatio = pwrNowSum / pwrMaxSum;
                }
                else
                {
                    batteryPowerRatio = 9999;
                }
                if (batteryPowerRatio > 0.9)
                {
                    for (int i = 0; i < reactors.Count; i++)
                    {
                        reactors[i].GetActionWithName("OnOff_On").Apply(reactors[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reactors.Count; i++)
                    {
                        reactors[i].GetActionWithName("OnOff_Off").Apply(reactors[i]);
                    }
                }
            }

            return;
        }
        #endregion
    }
}