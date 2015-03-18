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

namespace SpaceEngineersScripting_UpdateInfoScreen
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor
        //Display Info for Upper Cockpit LCD

        static string outputScreen = "LCD Panel: Upper Cockpit: Info";
        static string solarGroup = "Solar Panels: Borealis";

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
            }

            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks);

            double totalPower = Math.Round((((double)solars.Count * 120) / 1000), 2);

            clearOutputScreen(outputScreen);
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