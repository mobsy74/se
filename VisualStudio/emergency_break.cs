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

namespace SpaceEngineersScripting_EmergencyBreak
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor
        //Turn on the dampeners if they are off and toggle battery recharge

        static string controllerName = "Flight Seat: Upper Cockpit";
        static string batteryGroup = "Batteries: Borealis";

        public void toggleBatteries()
        {
            List<IMyBlockGroup> groups = GridTerminalSystem.BlockGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == batteryGroup)
                {
                    List<IMyTerminalBlock> batteries = groups[i].Blocks;
                    for (int j = 0; j < batteries.Count; j++)
                    {
                        IMyBatteryBlock battery = batteries[j] as IMyBatteryBlock;
                        bool recharging = true;
                        string batteryInfo = battery.DetailedInfo;
                        recharging = batteryInfo.Contains("recharged");
                        //if Charging, then switch them to discharge
                        if (recharging)
                        {
                            battery.GetActionWithName("Recharge").Apply(battery);
                        }
                    }
                }
            }
        }

        void Main()
        {
            IMyCockpit controller = GridTerminalSystem.GetBlockWithName(controllerName) as IMyCockpit;
            if (!controller.DampenersOverride)
            {
                controller.GetActionWithName("DampenersOverride").Apply(controller);
            }
            toggleBatteries();
        }
        #endregion
    }
}