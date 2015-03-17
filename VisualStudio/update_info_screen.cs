﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Common.ObjectBuilders;
using VRageMath;
using VRage;

namespace SpaceEngineersScripting_UpdateInfoScreen
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor
        //Display Info for Upper Cockpit LCD

        static string outputScreen = "LCD Panel: Upper Cockpit: Info";

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

        //Calculate max power output from solar panels on main ship
        void Main()
        {
            clearOutputScreen(outputScreen);
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks);
            int totalPanels = 0;
            for (int i = 0; i < blocks.Count; i++)
            {
                string[] splits = blocks[i].CustomName.Split(':');

                //If there is no : in the name (eg: Solar Panel: Skiff)  
                if (splits.Length == 1)
                {
                    totalPanels++;
                }
            }
            double totalPower = Math.Round((((double)totalPanels * 120) / 1000), 2);

            //print(outputScreen, totalPanels.ToString() + " Solar Panels | " + String.Format("{0:F2}", totalPower) + "MW max power");

            print(outputScreen, " " + String.Format("{0:F2}", totalPower) + "MW power output at full sail");
            print(outputScreen, " 1) Toggle Grav Drive power");
            print(outputScreen, " 2) Toggle Grav Drive direction");
            print(outputScreen, " 3) EMERGENCY BREAKING");
            print(outputScreen, " 4) Toggle Battery Recharge");
            print(outputScreen, " 5) Toggle Driller Dampeners");
            print(outputScreen, " 6) Camera View");
            print(outputScreen, " 7) Update Upper Info Screen");
            print(outputScreen, " 8) Toggle Antenna");
            print(outputScreen, " 9) Toggle Reactors");
        }

        #endregion
    }
}