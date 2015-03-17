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

namespace SpaceEngineersScripting_GravDriveToggleDirection
{
    class CodeEditorEmulator
    {
        IMyGridTerminalSystem GridTerminalSystem = null;

        #region CodeEditor
        //Grav Drive toggle direction script 

        static string upperOutputScreen = "LCD Panel: Upper Cockpit: Grav Drives";
        static string lowerOutputScreen = "LCD Panel: Lower Cockpit: Grav Drives";
        static string debugOutputScreen = "Script Output Screen";
        static float lowerGravityLimit = -1;
        static float upperGravityLimit = 1;
        static string lightsGroup = "Cockpit Lights";
        static string gravDriveGroup = "Grav Drive: Borealis";
        static string reverseDirection = "REVERSE";
        static string reverseMessage = "  Grav Drive Direction: " + reverseDirection;
        static string forwardDirection = "FORWARD";
        static string forwardMessage = "  Grav Drive Direction: " + forwardDirection;
        static string directionMessageType = "DIRECTION";
        static string powerOn = "ON";
        static string powerOnMessage = "  Grav Drive Power: " + powerOn;
        static string powerOff = "OFF";
        static string powerOffMessage = "  Grav Drive Power: " + powerOff;
        static string powerMessageType = "POWER";
        static string undetermined = "???";
        static string undeterminedDirectionMessage = "  Grav Drive Direction: " + undetermined;
        static string undeterminedPowerMessage = "  Grav Drive Power: " + undetermined;
        static Color reverseColor = new Color(255, 0, 0);  //red 
        static Color forwardColor = new Color(0, 255, 0);  //green 

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

        public string getNewScreenMessage(string screenName, string messageType, string message)
        {
            IMyTextPanel screen = GridTerminalSystem.GetBlockWithName(screenName) as IMyTextPanel;
            string oldMessage = screen.GetPublicText();

            if (!oldMessage.Contains("\n"))
            {
                oldMessage = undeterminedPowerMessage + "\n" + undeterminedDirectionMessage;
            }

            string[] splits = oldMessage.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int messageToChange = 0;
            if (messageType == directionMessageType)
            {
                messageToChange = 1;
            }
            else
            {
                messageToChange = 0;
            }

            splits[messageToChange] = message;
            string message1 = "";
            string message2 = "";
            message1 = splits[0];
            message2 = splits[1];

            string newMessage = message1 + "\n" + message2;
            string newScreenMessage = newMessage + "\n\n\n\n\n\n\n\n\n\n" + newMessage;
            return newScreenMessage;
        }

        public void setScreenFontColor(string screenName, Color screenColor)
        {
            IMyTextPanel screen = GridTerminalSystem.GetBlockWithName(screenName) as IMyTextPanel;
            screen.SetValue("FontColor", screenColor);
        }

        public float getcurrentGravity(List<IMyTerminalBlock> blocks)
        {
            float gravity = 0;
            for (int i = 0; i < blocks.Count; i++)
            {
                IMyGravityGenerator currentBlock = (IMyGravityGenerator)blocks[i];
                string[] splits = currentBlock.CustomName.Split(':');
                if (splits[0] == "Grav Drive")
                {
                    gravity = currentBlock.Gravity;
                    break;
                }
            }
            return gravity;
        }

        public string getNewGravDriveDirection(float currentGravity)
        {

            if (currentGravity < upperGravityLimit)
            {
                setLightsAndScreenColor(forwardColor);
                string newMessage = getNewScreenMessage(upperOutputScreen, directionMessageType, forwardMessage);
                clearOutputScreen(upperOutputScreen);
                clearOutputScreen(lowerOutputScreen);
                print(upperOutputScreen, newMessage);
                print(lowerOutputScreen, newMessage);
                return forwardDirection;
            }
            else
            {
                setLightsAndScreenColor(reverseColor);
                string newMessage = getNewScreenMessage(upperOutputScreen, directionMessageType, reverseMessage);
                clearOutputScreen(upperOutputScreen);
                clearOutputScreen(lowerOutputScreen);
                print(upperOutputScreen, newMessage);
                print(lowerOutputScreen, newMessage);
                return reverseDirection;
            }
        }

        public void setLightsAndScreenColor(Color newColor)
        {
            List<IMyBlockGroup> groups = GridTerminalSystem.BlockGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == lightsGroup)
                {
                    List<IMyTerminalBlock> lights = groups[i].Blocks;
                    for (int j = 0; j < lights.Count; j++)
                    {
                        IMyInteriorLight light = lights[j] as IMyInteriorLight;
                        light.SetValue("Color", newColor);
                    }
                }
            }
            setScreenFontColor(upperOutputScreen, newColor);
            setScreenFontColor(lowerOutputScreen, newColor);
        }

        void Main()
        {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyGravityGenerator>(blocks);
            float currentGravity = getcurrentGravity(blocks);
            string newGravDriveDirection = getNewGravDriveDirection(currentGravity);
            for (int i = 0; i < blocks.Count; i++)
            {
                IMyGravityGenerator gravGen = (IMyGravityGenerator)blocks[i];
                string[] splits = gravGen.CustomName.Split(':');
                if (splits[0] == "Grav Drive")
                {
                    if (newGravDriveDirection == forwardDirection)
                    {
                        gravGen.SetValueFloat("Gravity", upperGravityLimit);
                    }
                    else
                    {
                        gravGen.SetValueFloat("Gravity", lowerGravityLimit);
                    }
                }
            }
        }
        #endregion
    }
}