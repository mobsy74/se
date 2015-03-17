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

namespace SpaceEngineersScripting_ResetGravGenFields
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor
        //Reset field dimensions for all Borealis gravity generators

        static string cockpitGeneratorName = "Gravity Generator: Lower Cockpit";
        static string gravDriveBaseName = "Grav Drive: Generator ";
        static string centerGeneratorName = gravDriveBaseName + "C";
        static string[] corners = { "NW", "NE", "SW", "SE" };
        static string[] points = { "N", "E", "S", "W" };

        static float cockpitGenWidth = (float)12.5;
        static float cockpitGenHeight = (float)7.5;
        static float cockpitGenDepth = (float)22.5;

        static float centerGenWidth = (float)7.5;
        static float centerGenHeight = (float)7.5;
        static float centerGenDepth = (float)7.5;

        static float pointGenWidth = (float)7.5;
        static float pointGenHeight = (float)7.5;
        static float pointGenDepth = (float)12.5;

        static float cornerGenWidth = (float)12.5;
        static float cornerGenHeight = (float)7.5;
        static float cornerGenDepth = (float)12.5;


        void Main()
        {
            IMyGravityGenerator gravGen = null;

            gravGen = GridTerminalSystem.GetBlockWithName(cockpitGeneratorName) as IMyGravityGenerator;
            gravGen.SetValueFloat("Width", cockpitGenWidth);
            gravGen.SetValueFloat("Height", cockpitGenHeight);
            gravGen.SetValueFloat("Depth", cockpitGenDepth);

            gravGen = GridTerminalSystem.GetBlockWithName(centerGeneratorName) as IMyGravityGenerator;
            gravGen.SetValueFloat("Width", centerGenWidth);
            gravGen.SetValueFloat("Height", centerGenHeight);
            gravGen.SetValueFloat("Depth", centerGenDepth);

            for (int i = 0; i < points.Length; i++)
            {
                String gravGenName = gravDriveBaseName + points[i];
                gravGen = GridTerminalSystem.GetBlockWithName(gravGenName) as IMyGravityGenerator;
                gravGen.SetValueFloat("Width", pointGenWidth);
                gravGen.SetValueFloat("Height", pointGenHeight);
                gravGen.SetValueFloat("Depth", pointGenDepth);
            }

            for (int i = 0; i < corners.Length; i++)
            {
                String gravGenName = gravDriveBaseName + corners[i];
                gravGen = GridTerminalSystem.GetBlockWithName(gravGenName) as IMyGravityGenerator;
                gravGen.SetValueFloat("Width", cornerGenWidth);
                gravGen.SetValueFloat("Height", cornerGenHeight);
                gravGen.SetValueFloat("Depth", cornerGenDepth);
            }
        }
        #endregion
    }
}