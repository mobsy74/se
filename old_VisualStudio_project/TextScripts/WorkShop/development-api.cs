        void Main() 
        { 
            //Two examples of searching for a beacon or antenna 
            //It searches for a keyword that exists in the beacon's name,  
            //  but excludes disabled antenna and beacons or ones which match based on the exclusion 
            var antenna = GridTerminalSystem.MyAntenna("%", "Status Report"); 
            var beacon = GridTerminalSystem.MyBeacon("Status Report", "%"); 
 
            //Simple easy broadcasting method, disabled and enables antenna for dedicated server updates 
            antenna.BroadcastMessage("Hello World"); 
 
 
            //Still in progress, intended functionality is to check if ship has no one piloting it, if not then activates dampeners 
            GridTerminalSystem.ActivateVacancyDampeners(); 
 
            //Applies a rather simple virus for dampening, power conserving, and taking over cargo ships 
            //Note that at this time, the faction check is not working 
            //Still prety safe to use on a small fighter ship connected with a cargo ship 
            GridTerminalSystem.ApplyVirus("MyFactionName"); 
 
            //A really useful command to clean out assemblers that have left over ingots 
            GridTerminalSystem.CleanAssemblers(); 
 
            //Returns the hull integrity of the ship. Fully repaired is 100% 
            float integrity = GridTerminalSystem.Integrity(); 
 
            //Renames various objects in the grid based on an ordered numbering so it looks good in the terminal 
            //Highly customizable if you go look at it and follow by example, supports mods 
            GridTerminalSystem.OrderNames(); 
 
            //Returns the total space used in the grid. Ex. If completely empty will return 0.0 
            float spaceUsed = GridTerminalSystem.SpaceUsed(); 
 
            //This API includes a series of selectors for getting groups of objects easily from the terminal system. 
            //If none are found will return an empty list, the count on it will be 0 
            //Here are some common examples 
            var blocks = GridTerminalSystem.Blocks; //This one is part of the base in game API 
            var lights = GridTerminalSystem.Lights(); 
            var doors = GridTerminalSystem.Doors(); 
            var gravitygens = GridTerminalSystem.GravityGenerators(); 
            var cockpits = GridTerminalSystem.Cockpits(); 
 
            //This API includes a series of selectors for getting a single object strongly typed, searched by name 
            //If none are found it will return null, so do a null check on the object returned 
            //Here are some examples 
            IMyTerminalBlock gavitygen1 = GridTerminalSystem.GetBlockWithName("MyGenerator"); //This one is part of the base in game API 
            IMyGravityGeneratorBase gavitygen2 = GridTerminalSystem.GravityGenerator("MyGenerator"); 
            IMyInteriorLight light1 = GridTerminalSystem.InteriorLight("Light 1"); 
 
            //You can also do all the methods which KSH has announced is available for all blocks through their methods 
            //Note that some of the methods I had to fudge the name like "close" due to it being a protected name in a base class 
            var door = GridTerminalSystem.Door("Door 1"); 
            door.OpenIt(); 
            door.Disable(); 
 
            GridTerminalSystem.GetBlockWithName("PowerWhore").Disable(); //You can write fluent expressions! 
 
            var cockpit = GridTerminalSystem.Cockpits()[0] as IMyCockpit; 
            if (!cockpit.DampenersOverride) cockpit.ToggleDampeners(); 
 
            //There are a few group methods which can be done for collections 
            bool disabledSomething = GridTerminalSystem.Lights().AttemptDisable(); 
            bool enabledSomething = GridTerminalSystem.Doors().AttemptEnable(); 
            bool closedSomething = GridTerminalSystem.Doors().AttemptClose(); 
            GridTerminalSystem.Lights().Enable(); 
            GridTerminalSystem.LargeGatlingTurrets().Disable(); 
            GridTerminalSystem.Warheads().Toggle(); 
            GridTerminalSystem.SoundBlocks().Action("PlaySound"); 
 
            //Finally, there are generic methods for all IMyTerminalBlocks 
            var allblocks = GridTerminalSystem.Blocks; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var block = allblocks[x]; 
 
                if (!(block is IMyProjector)) 
                { 
                    block.Enable(); 
                } 
                if (block is IMyLightingBlock) 
                { 
                    block.Disable(); 
                    block.Toggle(); 
                } 
                if (block is IMyDoor) 
                { 
                    ((IMyDoor)block).CloseIt(); 
                } 
 
                if (block is IMySoundBlock) 
                    block.Action("PlaySound"); 
            } 
        } 
    } 
 
    #region CyberVicAPI 
    public static class Functions 
    { 
        public static string RawToKg(long rawAmount) 
        { 
            return ((float)rawAmount / 1000000).ToString("#,##0.00") + " kg"; 
        } 
    } 
    public static class BlockOperations 
    { 
        public static void ActivateVacancyDampeners(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            //This is work in progress so escaping 
            return; 
 
            List<IMyTerminalBlock> cockpits = GridTerminalSystem.Cockpits(); 
            if (cockpits.Count != 1) return; 
 
            // In this IF, we need to also validate that the cockpit is empty 
            var cockpit = (IMyCockpit)cockpits[0]; 
            if (!cockpit.DampenersOverride) 
                cockpit.Action("DampenersOverride"); 
        } 
        public static void ApplyVirus(this IMyGridTerminalSystem GridTerminalSystem, string myFaction) 
        { 
            // Assuming that the ship applying the virus only has 1 connector 
            // If there is not more than one connector (meaning it's not docked with another ship and locked) then don't apply 
            if (GridTerminalSystem.Connectors().Count == 1) 
                return; 
 
            //Loop through all ship objects disabling or otherwise settings them up for hostile takeover 
            var blocks = GridTerminalSystem.Blocks; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var block = blocks[x]; 
                if (block.CustomNameWithFaction.Contains(myFaction)) 
                    continue; 
 
                //Most important thing, turn off use of the cockpit, turn on dampeners, turn on thrusters 
                if (block is IMyCockpit) 
                { 
                    var cockpit = block as IMyCockpit; 
                    if (cockpit.ControlWheels) 
                        cockpit.ToggleControlWheels(); 
                    if (!cockpit.ControlThrusters) 
                        cockpit.ToggleControlThrusters(); 
                    if (cockpit.HandBrake) 
                        cockpit.ToggleHandBrake(); 
                    if (!cockpit.DampenersOverride) 
                        cockpit.ToggleDampeners(); 
                    block.Disable(); 
                } 
                if (block is IMyThrust) 
                { 
                    block.Enable(); 
                } 
 
                //Enable and open doors 
                if (block is IMyDoor) 
                { 
                    var door = block as IMyDoor; 
 
                    door.Enable(); 
                    door.OpenIt(); 
                } 
 
                //Power conservation to ensure ship takeover 
                if (block is IMyReactor) 
                { 
                    block.Enable(); 
                } 
                if (block is IMyBatteryBlock) 
                { 
                    var battery = block as IMyBatteryBlock; 
                    battery.ToggleRecharge(); 
                } 
                if (block is IMyBatteryBlock 
                 || block is IMyGravityGenerator 
                 || block is IMyInteriorLight 
                 || block is IMyReflectorLight) 
                { 
                    block.Disable(); 
                } 
 
                //Disable tools 
                if (block is IMyShipDrill 
                 || block is IMyShipGrinder 
                 || block is IMyShipWelder) 
                { 
                    block.Disable(); 
                } 
 
                //Disable weapon and critical systems 
                if (block is IMyLargeTurretBase 
                 || block is IMySmallMissileLauncher 
                 || block is IMySmallMissileLauncherReload 
                 || block is IMySensorBlock 
                 || block is IMyMedicalRoom 
                 || block is IMyWarhead) 
                { 
                    block.Disable(); 
                } 
            } 
        } 
        public static void CleanAssemblers(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            List<IMyTerminalBlock> assemblers = GridTerminalSystem.Assemblers(); 
            List<IMyTerminalBlock> containers = GridTerminalSystem.Containers(); 
 
            for (int x = 0; x < assemblers.Count; x++) 
            { 
                var assemblerInvOwner = (assemblers[x] as IMyInventoryOwner); 
                var assemblerInventory = assemblerInvOwner.GetInventory(0); 
 
                int cargo_index = -1; 
                if (assemblerInventory.CurrentVolume.RawValue > 0) 
                { 
                    var assemblerItems = assemblerInventory.GetItems(); 
 
                    for (int y = 0; y < containers.Count; y++) 
                    { 
                        var cargoInventoryOwner = containers[y] as IMyInventoryOwner; 
                        if (cargoInventoryOwner != null && !cargoInventoryOwner.GetInventory(0).IsFull) 
                        { 
                            cargo_index = y; 
                            break; 
                        } 
                    } 
 
                    if (cargo_index != -1) 
                    { 
                        var cargoInventoryOwner = containers[cargo_index] as IMyInventoryOwner; 
                        if (cargoInventoryOwner != null) 
                        { 
                            var cargoInventory = cargoInventoryOwner.GetInventory(0); 
                            var cargoItems = cargoInventory.GetItems(); 
 
                            for (int y = 0; y < assemblerItems.Count; y++) 
                            { 
                                if (!cargoInventory.IsFull) 
                                { 
                                    assemblerInventory.TransferItemTo(cargoInventory, y, null, true, null); 
                                } 
                                else 
                                { 
                                    x--; 
                                    continue; 
                                } 
                            } 
                        } 
                    } 
                } 
            } 
        } 
        public static float Integrity(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            float assemblyRatioTotal = 0; 
            int assemblyRatioCount = 0; 
            var blocks = GridTerminalSystem.Blocks; 
 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                assemblyRatioTotal += blocks[x].DisassembleRatio; 
                assemblyRatioCount++; 
            } 
            if (assemblyRatioCount > 0) 
                return 101f - (assemblyRatioTotal / (float)assemblyRatioCount); 
            else 
                return 0; 
        } 
        public static void OrderNames(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyAssembler>(blocks); 
            blocks.OrderNames(); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyButtonPanel>(blocks); 
            blocks.OrderNames("Button Panel", "Button Panel"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(blocks); 
            blocks.OrderNames("*", "Cont"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyCockpit>(blocks); 
            blocks.OrderNames("LargeBunk", "Large Bunk"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyCockpit>(blocks); 
            blocks.OrderNames("Cryogenic Pod", "Cryogenic Pod"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyCockpit>(blocks); 
            blocks.OrderNames("Flight Seat"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyCockpit>(blocks); 
            blocks.OrderNames("EE Computer Desk", "Computer Desk"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyDoor>(blocks); 
            blocks.OrderNames("Door"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyDoor>(blocks); 
            blocks.OrderNames("Large Gate", "Gate"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyDoor>(blocks); 
            blocks.OrderNames("Docking Ring Airlock", "Airlock"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyGravityGenerator>(blocks); 
            blocks.OrderNames("*", "Gravity Gen"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyGyro>(blocks); 
            blocks.OrderNames("*", "Gyro"); 
 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Large Armor Sloped Thrust", "LgArmoredSlopedThruster"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Large Armor Thrust", "LgArmoredThruster"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyReactor>(blocks); 
            blocks.OrderNames("Large Reactor", "LgReactor"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Large Thruster", "LgThruster"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyLightingBlock>(blocks); 
            blocks.OrderNames("*", "Light"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyMedicalRoom>(blocks); 
            blocks.OrderNames("Medical Room", "Med-bay"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipMergeBlock>(blocks); 
            blocks.OrderNames("Docking Ring Clamp", "Docking Ring"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyRefinery>(blocks); 
            blocks.OrderNames("Arc Furnace", "Furnace"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyRefinery>(blocks); 
            blocks.OrderNames("Refinery", "Ref"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Small Armor Sloped Thrust", "SmlArmoredSlopedThruster"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Small Armor Thrust", "SmlArmoredThruster"); 
            blocks.Clear(); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyReactor>(blocks); 
            blocks.OrderNames("Small Reactor", "SmlReactor"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            blocks.OrderNames("Small Thruster", "SmlThruster"); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipWelder>(blocks); 
            blocks.OrderNames(); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipGrinder>(blocks); 
            blocks.OrderNames(); 
 
            blocks.Clear(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(blocks); 
            blocks.OrderNames(); 
        } 
        public static float SpaceUsed(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            long volumeUsed = 0; 
            long volumeTotal = 0; 
            var blocks = GridTerminalSystem.Blocks; 
 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                if (blocks[x] as IMyShipGrinder != null 
                 || blocks[x] as IMyShipDrill != null 
                 || blocks[x] as IMyCargoContainer != null) 
                { 
                    var inventoryOwner = blocks[x] as IMyInventoryOwner; 
                    if (inventoryOwner != null) 
                    { 
                        volumeUsed += inventoryOwner.GetInventory(0).CurrentVolume.RawValue; 
                        volumeTotal += inventoryOwner.GetInventory(0).MaxVolume.RawValue; 
                    } 
                } 
            } 
 
            return (float)(((double)volumeUsed / (double)volumeTotal) * 100); 
        } 
 
        public static void OrderNames(this List<IMyTerminalBlock> blocks, string subClassName = "*", string customizedName = "") 
        { 
            int number = 0; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                if (blocks[x].DefinitionDisplayNameText.ToUpperInvariant() == subClassName.ToUpperInvariant() || subClassName == "*") 
                { 
                    number++; 
                    if (blocks[x] is IMyThrust || (!blocks[x].CustomName.Contains("[") && !blocks[x].CustomName.Contains("("))) 
                    { 
                        if (customizedName == "") 
                        { 
                            blocks[x].SetCustomName(blocks[x].DefinitionDisplayNameText + " " + number); 
                        } 
                        else 
                        { 
                            blocks[x].SetCustomName(customizedName + " " + number); 
                        } 
                    } 
                } 
            } 
        } 
 
        public static void BroadcastMessage(this IMyBeacon beacon, string message) 
        { 
            if (beacon != null) 
            { 
                beacon.Disable(); 
                beacon.SetCustomName(message); 
                beacon.Enable(); 
            } 
        } 
        public static void BroadcastMessage(this IMyRadioAntenna antenna, string message) 
        { 
            if (antenna != null) 
            { 
                antenna.Disable(); 
                antenna.SetCustomName(message); 
                antenna.Enable(); 
            } 
        } 
    } 
    public static class BlockActions 
    { 
        //Antenna 
        public static void IncreaseRadius(this IMyRadioAntenna block) 
        { 
            block.Action("IncreaseRadius"); 
        } 
        public static void DecreaseRadius(this IMyRadioAntenna block) 
        { 
            block.Action("DecreaseRadius"); 
        } 
 
        //Battery 
        public static void ToggleRecharge(this IMyBatteryBlock block) 
        { 
            block.Action("Recharge"); 
        } 
 
        //Beacon 
        public static void IncreaseRadius(this IMyBeacon block) 
        { 
            block.Action("IncreaseRadius"); 
        } 
        public static void DecreaseRadius(this IMyBeacon block) 
        { 
            block.Action("DecreaseRadius"); 
        } 
 
        //ButtonPanel 
        public static void AnyoneCanUse(this IMyButtonPanel block) 
        { 
            block.Action("AnyoneCanUse"); 
        } 
 
        //Cockpit 
        public static void ToggleControlWheels(this IMyCockpit block) 
        { 
            block.Action("ControlWheels"); 
        } 
        public static void ToggleControlThrusters(this IMyCockpit block) 
        { 
            block.Action("ControlThrusters"); 
        } 
        public static void ToggleHandBrake(this IMyCockpit block) 
        { 
            block.Action("HandBrake"); 
        } 
        public static void ToggleDampeners(this IMyCockpit block) 
        { 
            block.Action("DampenersOverride"); 
        } 
 
        //Connector 
        public static void ToggleThrowOut(this IMyShipConnector block) 
        { 
            block.Action("ThrowOut"); 
        } 
        public static void ToggleCollectAll(this IMyShipConnector block) 
        { 
            block.Action("CollectAll"); 
        } 
        public static void ToggleSwitchLock(this IMyShipConnector block) 
        { 
            block.Action("SwitchLock"); 
        } 
 
        //Door 
        public static void OpenIt(this IMyDoor block) 
        { 
            block.Action("Open_On"); 
        } 
        public static void CloseIt(this IMyDoor block) 
        { 
            block.Action("Open_Off"); 
        } 
        public static void ToggleOpenClose(this IMyDoor block) 
        { 
            block.Action("Open"); 
        } 
 
        //GravityGenerator 
        public static void IncreaseWidth(this IMyGravityGeneratorBase block) 
        { 
            block.Action("IncreaseWidth"); 
        } 
        public static void DecreaseWidth(this IMyGravityGeneratorBase block) 
        { 
            block.Action("DecreaseWidth"); 
        } 
        public static void IncreaseHeight(this IMyGravityGeneratorBase block) 
        { 
            block.Action("IncreaseHeight"); 
        } 
        public static void DecreaseHeight(this IMyGravityGeneratorBase block) 
        { 
            block.Action("DecreaseHeight"); 
        } 
        public static void IncreaseDepth(this IMyGravityGeneratorBase block) 
        { 
            block.Action("IncreaseDepth"); 
        } 
        public static void DecreaseDepth(this IMyGravityGeneratorBase block) 
        { 
            block.Action("DecreaseDepth"); 
        } 
        public static void IncreaseGravity(this IMyGravityGeneratorBase block) 
        { 
            block.Action("IncreaseGravity"); 
        } 
        public static void DecreaseGravity(this IMyGravityGeneratorBase block) 
        { 
            block.Action("DecreaseGravity"); 
        } 
 
        //Gyroscope 
        public static void IncreasePower(this IMyGyro block) 
        { 
            block.Action("IncreasePower"); 
        } 
        public static void DecreasePower(this IMyGyro block) 
        { 
            block.Action("DecreasePower"); 
        } 
        public static void Override(this IMyGyro block) 
        { 
            block.Action("Override"); 
        } 
        public static void IncreaseYaw(this IMyGyro block) 
        { 
            block.Action("IncreaseYaw"); 
        } 
        public static void DecreaseYaw(this IMyGyro block) 
        { 
            block.Action("DecreaseYaw"); 
        } 
        public static void IncreasePitch(this IMyGyro block) 
        { 
            block.Action("IncreasePitch"); 
        } 
        public static void DecreasePitch(this IMyGyro block) 
        { 
            block.Action("DecreasePitch"); 
        } 
        public static void IncreaseRoll(this IMyGyro block) 
        { 
            block.Action("IncreaseRoll"); 
        } 
        public static void DecreaseRoll(this IMyGyro block) 
        { 
            block.Action("DecreaseRoll"); 
        } 
 
        //LandingGear 
        public static void Lock(this IMyLandingGear block) 
        { 
            block.Action("Lock"); 
        } 
        public static void Unlock(this IMyLandingGear block) 
        { 
            block.Action("Unlock"); 
        } 
        public static void SwitchLock(this IMyLandingGear block) 
        { 
            block.Action("SwitchLock"); 
        } 
        public static void AutoLock(this IMyLandingGear block) 
        { 
            block.Action("Autolock"); 
        } 
        public static void IncreaseBreakForce(this IMyLandingGear block) 
        { 
            block.Action("IncreaseBreakForce"); 
        } 
        public static void DecreaseBreakForce(this IMyLandingGear block) 
        { 
            block.Action("DecreaseBreakForce"); 
        } 
 
        //LargeGatlingTurret 
        public static void IncreaseRange(this IMyLargeGatlingTurret block) 
        { 
            block.Action("IncreaseRange"); 
        } 
        public static void DecreaseRange(this IMyLargeGatlingTurret block) 
        { 
            block.Action("DecreaseRange"); 
        } 
 
        //LargeInteriorTurret 
        public static void IncreaseRange(this IMyLargeInteriorTurret block) 
        { 
            block.Action("IncreaseRange"); 
        } 
        public static void DecreaseRange(this IMyLargeInteriorTurret block) 
        { 
            block.Action("DecreaseRange"); 
        } 
 
        //Light 
        public static void IncreaseRadius(this IMyLightingBlock block) 
        { 
            block.Action("IncreaseRadius"); 
        } 
        public static void DecreaseRadius(this IMyLightingBlock block) 
        { 
            block.Action("DecreaseRadius"); 
        } 
        public static void IncreaseBlinkInterval(this IMyLightingBlock block) 
        { 
            block.Action("IncreaseBlink Interval"); 
        } 
        public static void DecreaseBlinkInterval(this IMyLightingBlock block) 
        { 
            block.Action("DecreaseBlink Interval"); 
        } 
        public static void IncreaseBlinkLength(this IMyLightingBlock block) 
        { 
            block.Action("IncreaseBlink Lenght"); 
        } 
        public static void DecreaseBlinkLength(this IMyLightingBlock block) 
        { 
            block.Action("IncreaseBlink Lenght"); 
        } 
        public static void IncreaseBlinkOffset(this IMyLightingBlock block) 
        { 
            block.Action("IncreaseBlink Offset"); 
        } 
        public static void DecreaseBlinkOffset(this IMyLightingBlock block) 
        { 
            block.Action("DecreaseBlink Offset"); 
        } 
 
        //MissleTurret 
        public static void IncreaseRange(this IMyLargeMissileTurret block) 
        { 
            block.Action("IncreaseRange"); 
        } 
        public static void DecreaseRange(this IMyLargeMissileTurret block) 
        { 
            block.Action("DecreaseRange"); 
        } 
 
        //MissleTurret 
        public static void Reverse(this IMyPistonBase block) 
        { 
            block.Action("Reverse"); 
        } 
        public static void IncreaseVelocity(this IMyPistonBase block) 
        { 
            block.Action("IncreaseVelocity"); 
        } 
        public static void DecreaseVelocity(this IMyPistonBase block) 
        { 
            block.Action("DecreaseVelocity"); 
        } 
        public static void ResetVelocity(this IMyPistonBase block) 
        { 
            block.Action("ResetVelocity"); 
        } 
        public static void IncreaseUpperLimit(this IMyPistonBase block) 
        { 
            block.Action("IncreaseUpperLimit"); 
        } 
        public static void DecreaseUpperLimit(this IMyPistonBase block) 
        { 
            block.Action("DecreaseUpperLimit"); 
        } 
        public static void IncreaseLowerLimit(this IMyPistonBase block) 
        { 
            block.Action("IncreaseLowerLimit"); 
        } 
        public static void DecreaseLowerLimit(this IMyPistonBase block) 
        { 
            block.Action("DecreaseLowerLimit"); 
        } 
 
        //MotorSuspension 
        public static void Steering(this IMyMotorSuspension block) 
        { 
            block.Action("Steering"); 
        } 
        public static void Propulsion(this IMyMotorSuspension block) 
        { 
            block.Action("Propulsion"); 
        } 
        public static void IncreaseDamping(this IMyMotorSuspension block) 
        { 
            block.Action("IncreaseDamping"); 
        } 
        public static void DecreaseDamping(this IMyMotorSuspension block) 
        { 
            block.Action("DecreaseDamping"); 
        } 
        public static void IncreaseStrength(this IMyMotorSuspension block) 
        { 
            block.Action("IncreaseStrength"); 
        } 
        public static void DecreaseStrength(this IMyMotorSuspension block) 
        { 
            block.Action("DecreaseStrength"); 
        } 
        public static void IncreaseFriction(this IMyMotorSuspension block) 
        { 
            block.Action("IncreaseFriction"); 
        } 
        public static void DecreaseFriction(this IMyMotorSuspension block) 
        { 
            block.Action("DecreaseFriction"); 
        } 
        public static void IncreasePower(this IMyMotorSuspension block) 
        { 
            block.Action("IncreasePower"); 
        } 
        public static void DecreasePower(this IMyMotorSuspension block) 
        { 
            block.Action("DecreasePower"); 
        } 
 
        //ProgrammableBlock 
        public static void Run(this IMyProgrammableBlock block) 
        { 
            block.Action("Run"); 
        } 
 
        //Rotor 
        public static void Reverse(this IMyMotorStator block) 
        { 
            block.Action("Reverse"); 
        } 
        public static void Detach(this IMyMotorStator block) 
        { 
            block.Action("Detach"); 
        } 
        public static void Attach(this IMyMotorStator block) 
        { 
            block.Action("Attach"); 
        } 
        public static void IncreaseTorque(this IMyMotorStator block) 
        { 
            block.Action("IncreaseTorque"); 
        } 
        public static void DecreaseTorque(this IMyMotorStator block) 
        { 
            block.Action("DecreaseTorque"); 
        } 
        public static void IncreaseBrakingTorque(this IMyMotorStator block) 
        { 
            block.Action("IncreaseBrakingTorque"); 
        } 
        public static void DecreaseBrakingTorque(this IMyMotorStator block) 
        { 
            block.Action("DecreaseBrakingTorque"); 
        } 
        public static void IncreaseVelocity(this IMyMotorStator block) 
        { 
            block.Action("IncreaseVelocity"); 
        } 
        public static void DecreaseVelocity(this IMyMotorStator block) 
        { 
            block.Action("DecreaseVelocity"); 
        } 
        public static void ResetVelocity(this IMyMotorStator block) 
        { 
            block.Action("ResetVelocity"); 
        } 
        public static void IncreaseLowerLimit(this IMyMotorStator block) 
        { 
            block.Action("IncreaseLowerLimit"); 
        } 
        public static void DecreaseLowerLimit(this IMyMotorStator block) 
        { 
            block.Action("DecreaseLowerLimit"); 
        } 
        public static void IncreaseUpperLimit(this IMyMotorStator block) 
        { 
            block.Action("IncreaseUpperLimit"); 
        } 
        public static void DecreaseUpperLimit(this IMyMotorStator block) 
        { 
            block.Action("DecreaseUpperLimit"); 
        } 
        public static void IncreaseDisplacement(this IMyMotorStator block) 
        { 
            block.Action("IncreaseDisplacement"); 
        } 
        public static void DecreaseDisplacement(this IMyMotorStator block) 
        { 
            block.Action("DecreaseDisplacement"); 
        } 
 
        //Sensor 
        public static void IncreaseLeft(this IMySensorBlock block) 
        { 
            block.Action("IncreaseLeft"); 
        } 
        public static void DecreaseLeft(this IMySensorBlock block) 
        { 
            block.Action("DecreaseLeft"); 
        } 
        public static void IncreaseRight(this IMySensorBlock block) 
        { 
            block.Action("IncreaseRight"); 
        } 
        public static void DecreaseRight(this IMySensorBlock block) 
        { 
            block.Action("DecreaseRight"); 
        } 
        public static void IncreaseBottom(this IMySensorBlock block) 
        { 
            block.Action("IncreaseBottom"); 
        } 
        public static void DecreaseBottom(this IMySensorBlock block) 
        { 
            block.Action("DecreaseBottom"); 
        } 
        public static void IncreaseTop(this IMySensorBlock block) 
        { 
            block.Action("IncreaseTop"); 
        } 
        public static void DecreaseTop(this IMySensorBlock block) 
        { 
            block.Action("DecreaseTop"); 
        } 
        public static void IncreaseBack(this IMySensorBlock block) 
        { 
            block.Action("IncreaseBack"); 
        } 
        public static void DecreaseBack(this IMySensorBlock block) 
        { 
            block.Action("DecreaseBack"); 
        } 
        public static void IncreaseFront(this IMySensorBlock block) 
        { 
            block.Action("IncreaseFront"); 
        } 
        public static void DecreaseFront(this IMySensorBlock block) 
        { 
            block.Action("DecreaseFront"); 
        } 
        public static void ToggleDetectPlayers(this IMySensorBlock block) 
        { 
            block.Action("Detect Players"); 
        } 
        public static void ToggleDetectFloatingObjects(this IMySensorBlock block) 
        { 
            block.Action("Detect Floating Objects"); 
        } 
        public static void ToggleDetectSmallShips(this IMySensorBlock block) 
        { 
            block.Action("DetectSmallShips"); 
        } 
        public static void ToggleDetectLargeShips(this IMySensorBlock block) 
        { 
            block.Action("Detect Large Ships"); 
        } 
        public static void ToggleDetectStations(this IMySensorBlock block) 
        { 
            block.Action("Detect Stations"); 
        } 
        public static void ToggleDetectAsteroids(this IMySensorBlock block) 
        { 
            block.Action("Detect Asteroids"); 
        } 
        public static void ToggleDetectOwner(this IMySensorBlock block) 
        { 
            block.Action("Detect Owner"); 
        } 
        public static void ToggleDetectFriendly(this IMySensorBlock block) 
        { 
            block.Action("Detect Friendly"); 
        } 
        public static void ToggleDetectNeutral(this IMySensorBlock block) 
        { 
            block.Action("Detect Neutral"); 
        } 
        public static void ToggleDetectEnemy(this IMySensorBlock block) 
        { 
            block.Action("Detect Enemy"); 
        } 
 
        //SoundBlock 
        public static void IncreaseVolumeSlider(this IMySoundBlock block) 
        { 
            block.Action("IncreaseVolumeSlider"); 
        } 
        public static void DecreaseVolumeSlider(this IMySoundBlock block) 
        { 
            block.Action("DecreaseVolumeSlider"); 
        } 
        public static void IncreaseRangeSlider(this IMySoundBlock block) 
        { 
            block.Action("IncreaseRangeSlider"); 
        } 
        public static void DecreaseRangeSlider(this IMySoundBlock block) 
        { 
            block.Action("DecreaseRangeSlider"); 
        } 
        public static void Play(this IMySoundBlock block) 
        { 
            block.Action("PlaySound"); 
        } 
        public static void Stop(this IMySoundBlock block) 
        { 
            block.Action("StopSound"); 
        } 
        public static void IncreaseLoopableSlider(this IMySoundBlock block) 
        { 
            block.Action("IncreaseLoopableSlider"); 
        } 
        public static void DecreaseLoopableSlider(this IMySoundBlock block) 
        { 
            block.Action("DecreaseLoopableSlider"); 
        } 
 
        //Thrusters 
        public static void IncreaseOverride(this IMyThrust block) 
        { 
            block.Action("IncreaseOverride"); 
        } 
        public static void DecreaseOverride(this IMyThrust block) 
        { 
            block.Action("DecreaseOverride"); 
        } 
 
        //Timer 
        public static void IncreaseTriggerDelay(this IMyTimerBlock block) 
        { 
            block.Action("IncreaseTriggerDelay"); 
        } 
        public static void DecreaseTriggerDelay(this IMyTimerBlock block) 
        { 
            block.Action("DecreaseTriggerDelay"); 
        } 
        public static void TriggerNow(this IMyTimerBlock block) 
        { 
            block.Action("TriggerNow"); 
        } 
        public static void Start(this IMyTimerBlock block) 
        { 
            block.Action("Start"); 
        } 
        public static void Stop(this IMyTimerBlock block) 
        { 
            block.Action("Stop"); 
        } 
 
        //Warhead 
        public static void IncreaseDetonationTime(this IMyWarhead block) 
        { 
            block.Action("IncreaseDetonationTime"); 
        } 
        public static void DecreaseDetonationTime(this IMyWarhead block) 
        { 
            block.Action("DecreaseDetonationTime"); 
        } 
        public static void StartCountdown(this IMyWarhead block) 
        { 
            block.Action("StartCountdown"); 
        } 
        public static void StopCountdown(this IMyWarhead block) 
        { 
            block.Action("StopCountdown"); 
        } 
        public static void Safety(this IMyWarhead block) 
        { 
            block.Action("Safety"); 
        } 
        public static void Detonate(this IMyWarhead block) 
        { 
            block.Action("Detonate"); 
        } 
 
 
        //Attempts on lists 
        public static bool AttemptEnable(this List<IMyTerminalBlock> blocks) 
        { 
            bool enabledSomething = false; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var functionalBlock = blocks[x] as IMyFunctionalBlock; 
 
                if (functionalBlock != null && !functionalBlock.Enabled) 
                { 
                    blocks[x].Enable(); 
                    enabledSomething = true; 
                } 
            } 
            return enabledSomething; 
        } 
        public static bool AttemptClose(this List<IMyTerminalBlock> blocks) 
        { 
            bool closedSomething = false; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var door = blocks[x] as IMyDoor; 
 
                if (door != null && door.Open) 
                { 
                    door.CloseIt(); 
                    closedSomething = true; 
                } 
            } 
            return closedSomething; 
        } 
        public static bool AttemptDisable(this List<IMyTerminalBlock> blocks) 
        { 
            bool disabledSomething = false; 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var functionalBlock = blocks[x] as IMyFunctionalBlock; 
 
                if (functionalBlock != null && functionalBlock.Enabled) 
                { 
                    blocks[x].Disable(); 
                    disabledSomething = true; 
                } 
            } 
            return disabledSomething; 
        } 
 
        //Generic list actions 
        public static void Enable(this List<IMyTerminalBlock> blocks) 
        { 
            blocks.Action("OnOff_On"); 
        } 
        public static void Disable(this List<IMyTerminalBlock> blocks) 
        { 
            blocks.Action("OnOff_Off"); 
        } 
        public static void Toggle(this List<IMyTerminalBlock> blocks) 
        { 
            blocks.Action("OnOff"); 
        } 
        public static void Action(this List<IMyTerminalBlock> blocks, string action) 
        { 
            for (int x = 0; x < blocks.Count; x++) 
            { 
                var block = blocks[x]; 
                var foundAction = block.GetActionWithName(action); 
                if (foundAction != null) 
                { 
                    foundAction.Apply(block); 
                } 
            } 
        } 
 
        //Generic actions 
        public static void ToggleUseConveyor(this IMyFunctionalBlock block) 
        { 
            block.Action("UseConveyor"); 
        } 
        public static void Enable(this IMyTerminalBlock block) 
        { 
            block.Action("OnOff_On"); 
        } 
        public static void Disable(this IMyTerminalBlock block) 
        { 
            block.Action("OnOff_Off"); 
        } 
        public static void Toggle(this IMyTerminalBlock block) 
        { 
            block.Action("OnOff"); 
        } 
        public static void Action(this IMyTerminalBlock block, string action) 
        { 
            if (block != null) 
            { 
                var foundAction = block.GetActionWithName(action); 
                if (foundAction != null) 
                { 
                    foundAction.Apply(block); 
                } 
            } 
        } 
    } 
    public static class Selectors 
    { 
        public static IMyRadioAntenna MyAntenna(this IMyGridTerminalSystem GridTerminalSystem, string broadcastKeyword = null, string broadcastExclusion = null) 
        { 
            List<IMyTerminalBlock> antennas = GridTerminalSystem.Antennas(); 
            if (antennas.Count == 0) return null; 
 
            for (int x = 0; x < antennas.Count; x++) 
            { 
                if ((broadcastKeyword != null && antennas[x].CustomName.Contains(broadcastKeyword)) 
                 && (broadcastExclusion != null && !antennas[x].CustomName.Contains(broadcastExclusion)) 
                 && ((IMyRadioAntenna)antennas[x]).Enabled) 
                    return antennas[x] as IMyRadioAntenna; 
            } 
            return null; 
        } 
        public static IMyBeacon MyBeacon(this IMyGridTerminalSystem GridTerminalSystem, string broadcastKeyword = null, string broadcastExclusion = null) 
        { 
            List<IMyTerminalBlock> beacons = GridTerminalSystem.Beacons(); 
            if (beacons.Count == 0) return null; 
 
            for (int x = 0; x < beacons.Count; x++) 
            { 
                if ((broadcastKeyword != null && beacons[x].CustomName.Contains(broadcastKeyword)) 
                 && (broadcastExclusion != null && !beacons[x].CustomName.Contains(broadcastExclusion)) 
                 && ((IMyRadioAntenna)beacons[x]).Enabled) 
                    return beacons[x] as IMyBeacon; 
            } 
            return null; 
        } 
 
        public static List<IMyTerminalBlock> Antennas(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Assemblers(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyAssembler>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Batteries(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Beacons(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyBeacon>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ButtonPanels(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyButtonPanel>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Cameras(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyCameraBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Cockpits(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipController>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Connectors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Collectors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyCollector>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Containers(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Doors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyDoor>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> GravityGenerators(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyGravityGenerator>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Gyroscopes(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyGyro>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> InteriorLights(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> LandingGears(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> LargeGatlingTurrets(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> LargeInteriorTurrets(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyLargeInteriorTurret>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> LargeMissileTurrets(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyLargeMissileTurret>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Lights(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> MedicalRooms(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyMedicalRoom>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> MergeBlocks(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipMergeBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> MotorSuspensions(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> OreDetectors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyOreDetector>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Pistons(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ProgrammableBlocks(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Projectors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyProjector>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Refineries(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyRefinery>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ReflectorLights(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyReflectorLight>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> RemoteControls(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyRemoteControl>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Rotors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Sensors(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> SphericalGravityGenerators(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyGravityGeneratorSphere>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ShipDrills(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ShipGrinders(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipGrinder>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> ShipWelders(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyShipWelder>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> SmallGatlingGuns(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMySmallGatlingGun>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> SmallMissleLaunchers(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMySmallMissileLauncher>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> SolarPanels(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> SoundBlocks(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMySoundBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Timers(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Thrusters(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> VirtualMassBlocks(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyVirtualMass>(blocks); 
            return blocks; 
        } 
        public static List<IMyTerminalBlock> Warheads(this IMyGridTerminalSystem GridTerminalSystem) 
        { 
            var blocks = new List<IMyTerminalBlock>(); 
            GridTerminalSystem.GetBlocksOfType<IMyWarhead>(blocks); 
            return blocks; 
        } 
 
        public static IMyRadioAntenna Antenna(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyRadioAntenna; 
        } 
        public static IMyAssembler Assembler(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyAssembler; 
        } 
        public static IMyBatteryBlock Battery(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyBatteryBlock; 
        } 
        public static IMyBeacon Beacon(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyBeacon; 
        } 
        public static IMyButtonPanel ButtonPanel(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyButtonPanel; 
        } 
        public static IMyCameraBlock Camera(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyCameraBlock; 
        } 
        public static IMyCargoContainer Container(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyCargoContainer; 
        } 
        public static IMyCockpit Cockpit(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyCockpit; 
        } 
        public static IMyShipConnector Connector(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyShipConnector; 
        } 
        public static IMyCollector Collector(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyCollector; 
        } 
        public static IMyDoor Door(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyDoor; 
        } 
        public static IMyGravityGenerator GravityGenerator(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyGravityGenerator; 
        } 
        public static IMyGyro Gyroscope(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyGyro; 
        } 
        public static IMyInteriorLight InteriorLight(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyInteriorLight; 
        } 
        public static IMyLandingGear LandingGear(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyLandingGear; 
        } 
        public static IMyLargeGatlingTurret LargeGatlingTurret(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyLargeGatlingTurret; 
        } 
        public static IMyLargeInteriorTurret LargeInteriorTurret(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyLargeInteriorTurret; 
        } 
        public static IMyLargeMissileTurret LargeMissileTurret(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyLargeMissileTurret; 
        } 
        public static IMyLightingBlock Light(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyLightingBlock; 
        } 
        public static IMyMedicalRoom MedicalRoom(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyMedicalRoom; 
        } 
        public static IMyShipMergeBlock MergeBlock(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyShipMergeBlock; 
        } 
        public static IMyMotorSuspension MotorSuspension(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyMotorSuspension; 
        } 
        public static IMyOreDetector OreDetector(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyOreDetector; 
        } 
        public static IMyPistonBase Piston(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyPistonBase; 
        } 
        public static IMyProgrammableBlock ProgrammableBlock(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyProgrammableBlock; 
        } 
        public static IMyProjector Projector(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyProjector; 
        } 
        public static IMyRefinery Refineries(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyRefinery; 
        } 
        public static IMyReflectorLight ReflectorLights(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyReflectorLight; 
        } 
        public static IMyRemoteControl RemoteControls(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyRemoteControl; 
        } 
        public static IMyMotorStator Rotor(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyMotorStator; 
        } 
        public static IMySensorBlock Sensors(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMySensorBlock; 
        } 
        public static IMyGravityGeneratorSphere SphericalGravityGenerator(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyGravityGeneratorSphere; 
        } 
        public static IMyShipDrill ShipDrills(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyShipDrill; 
        } 
        public static IMyShipGrinder ShipGrinders(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyShipGrinder; 
        } 
        public static IMyShipWelder ShipWelders(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyShipWelder; 
        } 
        public static IMySmallGatlingGun SmallGatlingGuns(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMySmallGatlingGun; 
        } 
        public static IMySmallMissileLauncher SmallMissleLaunchers(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMySmallMissileLauncher; 
        } 
        public static IMySolarPanel SolarPanels(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMySolarPanel; 
        } 
        public static IMySoundBlock SoundBlock(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMySoundBlock; 
        } 
        public static IMyTimerBlock TimerBlock(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyTimerBlock; 
        } 
        public static IMyThrust Thruster(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyThrust; 
        } 
        public static IMyVirtualMass VirtualMassBlock(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyVirtualMass; 
        } 
        public static IMyWarhead Warhead(this IMyGridTerminalSystem GridTerminalSystem, string name) 
        { 
            return GridTerminalSystem.GetBlockWithName(name) as IMyWarhead; 
        } 
    #endregion