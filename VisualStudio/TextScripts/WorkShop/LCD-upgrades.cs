/* v:1.12 [LCD linking support!] 
In-game script by MMaster 
 
[b]Manages multiple LCDs based on commands written in LCD public title. 
- Linked LCDs! 
- Automatic LCD scrolling up and down! 
- Any font size! 
- Filtered inventory items listing & missing items listing 
- Reactor, solar & battery power stats 
- Damaged blocks list with progress bars! 
- Filter blocks by name 
- Cargo space 
- Block count 
- Producing, Idle & Enabled summary / lis 
- Laser antenna status 
- Doors, gears & connectors status 
- Location & Time 
- Custom lines 
- Nicely formatted text with progress bars! 
- Multiple commands in single LCD! 
- Works on any server! 
 
NO PROGRAMMING NEEDED.[/b] 
 
I notify about updates on twitter so follow if interested.  
 
Please DO NOT publish this script or its derivations without my permission! 
 
[h1]QUICK GUIDE[/h1] 
1. Load this script to programmable block 
2. Build timer block, set it to 1 second 
3. Setup timer block actions: 1. Run program block 2. Start timer 
4. Start timer 
5. Build few LCD panels (or text panels) 
6. Name the LCDs however you like just add [LCD] to the end of name  
(e.g Text panel [LCD]) 
7. Set Font Size to 0.8 
8. Set LCDs public title to one of: 
[i]Inventory * +ingot[/i] 
- shows all ingots on ship/station 
[i]Inventory * +component[/i] 
- shows all components on ship/station 
[i]Missing * +component[/i] 
- shows missing components on ship/station 
[i]Power;echo;BlockCount * reactor;echo;Cargo[/i] 
- shows power stats and empty line,  
reactor count, empty line and cargo stats 
Note: Look at [b]COMMANDS section below for more detailed explanation[/b] 
9. LCD panels now show whatever you told them to 
 
 
[h1]Ultimate guide to answer all your questions:[/h1] 
 
http://steamcommunity.com/sharedfiles/filedetails/?id=407158161 
 
 * LCD linking is explained only in full guide! 
 
[h1]COMMANDS GUIDE[/h1] 
All commands usually work without entering any arguments. 
More commands are separated using [b];[/b] 
e.g: [i]Time Base Stats - Time: ;echo;Power;echo;Cargo;[/i] 
(display text following with current time;next line; 
show power stats;next line;show cargo stats) 
You can specify filters and other things by using command arguments. 
Each argument is just one word 
 
First argument usually specifies filter for name of blocks 
* means all blocks 
e.g: [i]Inventory * +ingot[/i] 
(this will show all ingots from all blocks) 
or: [i]Inventory Storage +component[/i] 
(this will show components from blocks which have Storage in name) 
 
Enter multiple words in single argument by using { and } 
e.g.: [i]Inventory {My Cargo Container}[/i] 
(this will show inventory of blocks which have 
"My Cargo Container" (without quotes) in name) 
 
 
[h1]COMMAND: Inventory[/h1] 
Displays inventory summary for certain item types 
Use InventoryX to not automatically add 0 items. 
 
No arguments: displays all items on current ship/station.      
1. argument: filters blocks based on name 
Next arguments: specify included/excluded item types and quotas 
 
[b]Item type and quota specification:[/b] 
Operator + or - adds or removes items from display 
[b]+all[/b] adds all item types to display  
[b]-ore[/b] removes ores from all items already added 
 * You need to add something for [b]-[/b] operator to work! 
[b]Use main types in specification:[/b] 
ore (all ores) 
ingot (all ingots) 
component (all components) 
ammo (all ammo boxes) 
tool (all tools + hand guns) 
[b]Or sub types:[/b] 
iron (both ore and ingot), gold, nickel, platinum, etc 
steelplate, construction, thrust, reactor, etc 
[b]Or both:[/b] 
ingot/iron (only iron ingots), ingot/uranium (only uranium) 
[b]You can combine that like this:[/b] 
+ingot/iron,gold (add iron and gold ingots) 
+ingot,component (add ingots and components) 
+steelplate,construction (steelplates and construction components) 
[b]To override progress bar quotas:[/b] 
+ingot:10000  
(adds all ingots with all of them having max progress bar value 10000) 
+component:1000 +steelplate:10000,construction:9000 
(adds all components with quota 1000,  
overrides steelplate and construction components with different quotas) 
 
[b]Example usages:[/b] 
Inventory {Ingot Storage} +ingot:30000 
Inventory Container +all -tool -ammo 
 
[h1]COMMAND: InvList[/h1] 
Same as Inventory, but does not display categories headers. 
  
[h1]COMMAND: Missing[/h1] 
Displays items which are low in stock (lower than set quota) 
 
Default minimum quota is 1. 
Works the same way as Inventory command.      
 
No arguments: displays all missing items on ship/station. 
1. argument: filters blocks based on name 
Next arguments: specify included/excluded item types and quotas 
 
[b]Example:[/b] 
Missing [STORAGE] +component:50 +ingot:100 +ammo:10 
 
[h1]COMMAND: Cargo[/h1] 
Displays cargo space of specified cargo containers 
 
No arguments: all containers on ship/station 
1. argument: filters containers based on name 
 
[b]Example:[/b] 
Cargo {Red Cargo} 
 
[h1]COMMAND: CargoAll[/h1] 
Displays cargo space of specified blocks 
Usage is same as Cargo 
 
[h1]COMMAND: Power[/h1] 
Displays power statistics for specified blocks 
Automatically separates reactors, solar panels and batteries 
 
No arguments: all reactors, solars and batteries 
1. argument: filters blocks based on name 
 
[b]Example:[/b] 
Power {Main Power} 
 
[h1]COMMAND: PowerSummary[/h1] 
Same as Power, but displays only total power output. 
 
[h1]COMMAND: Damage[/h1] 
Displays damaged ship/station blocks 
 
No arguments: all blocks on ship/station 
1. argument: filters blocks based on name 
 
[b]Example:[/b] 
Damage [SHIPYARD] 
 
[h1]COMMAND: BlockCount[/h1] 
Displays number of blocks of specified type 
Separates different sub types of blocks 
 
No arguments: nothing will be displayed! 
1. argument: filters blocks based on name, still nothing displayed! 
Next arguments: filter blocks based on type 
[b]Use main block type name like:[/b] 
reactor, thruster, container, refinery, assembler, etc 
[b]Types separated by space or ,[/b] 
 
[b]Example:[/b] 
BlockCount * thruster,gyro,reactor,solar,battery 
 
[h1]COMMAND: EnabledCount[/h1] 
Displays number of enabled blocks of specified type. 
Usage is same as BlockCount 
 
[h1]COMMAND: ProdCount[/h1] 
Displays number of producing blocks of specified type. 
Usage is same as BlockCount 
 
[b]Example:[/b] 
ProdCount * refinery,assembler 
 
[h1]COMMAND: Working[/h1] 
Displays all blocks of specified type showing their working state. 
 
Usage is same as BlockCount.  
[b]You can filter states like this:[/b] 
Working * assembler:work reactor:on 
 
[b]Example:[/b] 
Working Red refinery,assembler 
 
[h1]COMMAND: Echo[/h1] 
Displays single line of text 
 
No arguments: empty line 
Next arguments: text to be displayed 
 
[b]Examples:[/b] 
Echo MMaster's Text Panel 
Echo 
 
[h1]COMMAND: Time[/h1] 
Displays single line of text followed by current time 
 
No arguments: display only current time 
Next arguments: text to be shown before the time 
 
[b]Example:[/b] 
Time MMaster's Text Panel Time:  
  
[h1]COMMAND: Pos[/h1] 
Displays world position of the LCD panel. 
 
[h1]Tips[/h1] 
Use multiple programmable blocks with different LCD_TAG to split the work 
 
Name your LCD block like this: Text panel [LCD] #Heading 
it will add the text after # as first line 
Add spaces between # and text to move it right. 
 
If you use more panels: 
 * script updates one panel per run 
 * modify PANELS_PER_STEP to update more panels in single run 
 * DO NOT report problems with modified PANELS_PER_STEP 
 
[h1]Special Thanks[/h1] 
bssespaceengineers.com - awesome server 
Rhedd - for his contribution to modded items entries 
 
Watch Twitter: https://twitter.com/MattsPlayCorner 
and Facebook: https://www.facebook.com/MattsPlayCorner1080p 
for more crazy stuff from me in the future :) 
 */ 
    // Use this tag to identify LCDs managed by this script 
    public static string LCD_TAG = "[LCD]"; 
    // For german clients 
    public static string SECONDARY_TAG = "!LCD!"; 
    // How many panels to update per one step 
    public static int PANELS_PER_STEP = 1; 
    // How many lines to scroll per step 
    public static int DSCROLL_LINES_PER_STEP = 5; 
 
    // Enable debug to antenna or LCD marked with [DEBUG] 
    public static bool EnableDebug = false; 
 
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
    // DO NOT MODIFY ANYTHING BELOW THIS 
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
    public static int SCROLL_LINES_PER_STEP = 5; 
    public static int step = 0; 
    void Main() 
    { 
        // Init MMAPI and debug panels marked with [DEBUG] 
        MM.Init(GridTerminalSystem, EnableDebug); 
 
        LCDsProgram.SECONDARY_TAG = SECONDARY_TAG; 
        LCDsProgram lcdProg = new LCDsProgram(LCD_TAG, SCROLL_LINES_PER_STEP, PANELS_PER_STEP); 
        lcdProg.Run(step++); 
    } 
} 
 
public static class MMItems 
{ 
    public static Dictionary<string, MMItem> items = new Dictionary<string, MMItem>(); 
    public static List<string> items_keys = new List<string>(); 
    public static Dictionary<string, MMItem> itemsByShortName = new Dictionary<string, MMItem>(); 
 
    public static void Init() 
    { 
        if (items.Count > 0) 
            return; 
 
// ************************************************** 
// OK MAYBE YOU CAN ADD MODDED ITEMS AND MODIFY QUOTAS  
//        IF THAT WARNING DIDN'T SCARE YOU 
// ************************************************** 
// ITEMS AND QUOTAS LIST  
// (subType, mainType, mass, volume, quota, display name, short name) 
// ADD MODDED ITEMS TO THIS LIST 
// !! MAIN TYPES MUST GO TOGETHER FOR INV CATEGORIES !! 
// Generated from SE Data 27.2.2015 
Add("Stone", "Ore", 1f, 0.37f); 
Add("Iron", "Ore", 1f, 0.37f); 
Add("Nickel", "Ore", 1f, 0.37f); 
Add("Cobalt", "Ore", 1f, 0.37f); 
Add("Magnesium", "Ore", 1f, 0.37f); 
Add("Silicon", "Ore", 1f, 0.37f); 
Add("Silver", "Ore", 1f, 0.37f); 
Add("Gold", "Ore", 1f, 0.37f); 
Add("Platinum", "Ore", 1f, 0.37f); 
Add("Uranium", "Ore", 1f, 0.37f); 
Add("Stone", "Ingot", 1f, 0.37f, 40000, "Gravel", "gravel"); 
Add("Iron", "Ingot", 1f, 0.127f, 100000); 
Add("Nickel", "Ingot", 1f, 0.112f, 80000); 
Add("Cobalt", "Ingot", 1f, 0.112f, 30000); 
Add("Magnesium", "Ingot", 1f, 0.575f, 30000); 
Add("Silicon", "Ingot", 1f, 0.429f, 50000); 
Add("Silver", "Ingot", 1f, 0.095f, 50000); 
Add("Gold", "Ingot", 1f, 0.052f, 60000); 
Add("Platinum", "Ingot", 1f, 0.047f, 40000); 
Add("Uranium", "Ingot", 1f, 0.052f, 10000); 
// MODDED Antimatter Ultimate PVP Pack 
//Add("Antimatter", "Ingot", 1f, 0.37f, 50000); 
// Vanilla 
Add("Scrap", "Ingot", 1f, 0.254f); 
Add("AutomaticRifleItem", "PhysicalGunObject", 3f, 14f, 10); 
Add("WelderItem", "PhysicalGunObject", 5f, 8f); 
Add("AngleGrinderItem", "PhysicalGunObject", 3f, 20f); 
Add("HandDrillItem", "PhysicalGunObject", 22f, 120f); 
Add("Construction", "Component", 8f, 2f, 40000); 
Add("MetalGrid", "Component", 6f, 15f, 8000); 
Add("InteriorPlate", "Component", 3f, 5f, 25000); 
Add("SteelPlate", "Component", 20f, 3f, 200000); 
Add("Girder", "Component", 6f, 2f, 2000); 
Add("SmallTube", "Component", 4f, 2f, 16000); 
Add("LargeTube", "Component", 25f, 38f, 3000); 
Add("Motor", "Component", 24f, 8f, 3000); 
Add("Display", "Component", 8f, 6f, 350); 
Add("BulletproofGlass", "Component", 15f, 8f, 3000, "Bulletp. Glass", "bpglass"); 
Add("Computer", "Component", 0.2f, 1f, 5000); 
Add("Reactor", "Component", 25f, 8f, 10000); 
Add("Thrust", "Component", 40f, 10f, 16000, "Thruster", "thruster"); 
Add("GravityGenerator", "Component", 800f, 200f, 50, "GravGen", "gravgen"); 
Add("Medical", "Component", 150f, 160f, 120); 
Add("RadioCommunication", "Component", 8f, 140f, 200, "Radio-comm", "radio"); 
Add("Detector", "Component", 5f, 6f, 100); 
Add("Explosives", "Component", 2f, 2f, 100); 
Add("SolarCell", "Component", 8f, 20f, 1500); 
Add("PowerCell", "Component", 25f, 45f, 1500); 
// MODDED ITEM AzimuthSupercharger  
// (mass and volume not known - set same as thruster) 
//Add("AzimuthSupercharger", "Component", 40f, 10f, 1600, "Supercharger", "supercharger"); 
// Vanilla 
Add("NATO_5p56x45mm", "AmmoMagazine", 0.45f, 0.2f, 1000); 
Add("NATO_25x184mm", "AmmoMagazine", 35f, 16f, 2000); 
Add("Missile200mm", "AmmoMagazine", 45f, 60f, 1600); 
// MODDED ITEMS  
// Scatter ammo (mass and volume not known - set similar to NATO magazines) 
//Add("Scatter", "AmmoMagazine", 40f, 20f, 2000); 
/* Contributed by Rhedd */ 
// CSD Battlecannon 
//Add("250shell", "AmmoMagazine", 128f, 64f, 100); 
//Add("88shell", "AmmoMagazine", 16f, 16f, 1000); 
//Add("88hekc", "AmmoMagazine", 16f, 16f, 1000); 
// Minotaur Cannon 
//Add("MinotaurAmmo", "AmmoMagazine", 360f, 128f, 500); 
// Twin Missile Launchers 
//Add("TwinMiniMissile20mm", "AmmoMagazine", 4.5f, 6f, 1000); 
//Add("TwinMicroMissile5mm", "AmmoMagazine", 1f, 1.5f, 1000); 
// Twin Blaster Weapons 
//Add("MagazineBlasterPowerCellRed", "AmmoMagazine", 15f, 6f, 1000); 
//Add("MagazineBlasterPowerCellGreen", "AmmoMagazine", 15f, 6f, 1000); 
//Add("MagazineBlasterPowerCellBlue", "AmmoMagazine", 15f, 6f, 1000); 
//Add("MagazineSmallBlasterPowerCellRed", "AmmoMagazine", 15f, 6f, 1000); 
//Add("MagazineSmallBlasterPowerCellGreen", "AmmoMagazine", 15f, 6f, 1000); 
//Add("MagazineSmallBlasterPowerCellBlue", "AmmoMagazine", 15f, 6f, 1000); 
// Antimatter Ultimate PVP Pack 
//Add("AntimatterTorpedo200mm", "AmmoMagazine", 120f, 45f, 100); 
// Scatter turret ammo 
//Add("LargeScatter", "AmmoMagazine", 35f, 16f, 2000); 
// ISM weapons from FancyFez 
// ISM Mjollnir.FG Small ship fusion gun  
//Add("ISM_FusionAmmo", "AmmoMagazine", 35f, 10f, 1000); 
// ISM Longbow Small Ship Long Range Rifle 
//Add("ISM_LongbowAmmo", "AmmoMagazine", 35f, 2f, 1000); 
// ISM Mayfly CDS 
//Add("ISMNeedles", "AmmoMagazine", 35f, 16f, 1000); 
// ISM BullDog.01 Small ship gatling gun 
//Add("ISM_MinigunAmmo", "AmmoMagazine", 35f, 16f, 1000); 
// ISM Pikeman AA Defense System 
//Add("ISMTracer", "AmmoMagazine", 35f, 16f, 1000); 
// ISM Hellfire Launcher 
//Add("ISM_Hellfire", "AmmoMagazine", 45f, 60f, 500); 
 
    } 
 
    /* REALLY REALLY REALLY 
     * DO NOT MODIFY ANYTHING BELOW THIS 
     */ 
 
    public static string GetKey(int i) 
    { 
        return items_keys[i]; 
    } 
 
    public static MMItem GetValue(int i) 
    { 
        return items[items_keys[i]]; 
    } 
 
    // displayName - how the item will be displayed 
    // shortName - how the item can be called in arguments (eg: +supercharger) 
    public static void Add(string subType, string mainType, double mass, double volume, int quota = 0, string displayName = "", string shortName = "") 
    { 
        string fullType = subType + ' ' + mainType; 
        MMItem item = new MMItem(subType, mainType, mass, volume, quota, displayName, shortName); 
        items.Add(fullType, item); 
        if (shortName != "") 
            itemsByShortName.Add(shortName.ToLower(), item); 
        items_keys.Add(fullType); 
    } 
 
    public static bool GetItemsOfType(MMList<MMItem> itemlist, string subType = "", string mainType = "") 
    { 
        bool found = false; 
        for (int i = 0; i < items.Count; i++) 
        { 
            MMItem item = GetValue(i); 
            if (subType != "" && subType != item.subType) 
                continue; 
            if (mainType != "" && mainType != item.mainType) 
                continue; 
 
            itemlist.Add(item); 
            found = true; 
        } 
 
        return found; 
    } 
} 
 
public class LCDsProgram 
{ 
    // for german clients 
    public static string SECONDARY_TAG = ""; 
    // approximate width of LCD panel line 
    public const float LCD_LINE_WIDTH = 730; 
    // x position of inventory numbers 
    public const float LCD_LINE_NUMERS_POS = LCD_LINE_WIDTH - 30; 
    // x position of stat numbers 
    public const float LCD_LINE_STATS_POS = LCD_LINE_WIDTH - 170; 
    public const float LCD_LINE_INV_NUMBERS_POS = LCD_LINE_WIDTH - 170; 
    public const float LCD_LINE_INGOT_NUMBERS_POS = 375; 
    public const float LCD_LINE_DMG_NUMBERS_POS = LCD_LINE_WIDTH - 230; 
    public const float LCD_LINE_WORK_STATE_POS = LCD_LINE_WIDTH - 30; 
    public const float LCD_LINE_BLOCK_COUNT_POS = LCD_LINE_WIDTH - 30; 
    // scroll X lines per LCD update 
    public static int SCROLL_LINES = 5; 
    // number of component progress bar characters 
    public const int DINV_PROGRESS_CHARS = 38; 
    // stats progress bar characters (without percent at the end) 
    public const int DSTATS_PROGRESS_CHARS = 94; 
    // full line of progress bar 
    public const int DFULL_PROGRESS_CHARS = 116; 
     
    public static int INV_PROGRESS_CHARS = 0; 
    public static int STATS_PROGRESS_CHARS = 0; 
    public static int FULL_PROGRESS_CHARS = 0; 
     
    public static int PANELS_PER_STEP = 1; 
 
    public MMDict<string, MMPanel> panels = new MMDict<string, MMPanel>(); 
 
    public LCDsProgram(string nameLike, int sps, int pps) 
    { 
        MMBlockCollection textPanels = new MMBlockCollection(); 
        SCROLL_LINES = sps; 
        PANELS_PER_STEP = pps; 
        textPanels.AddBlocksOfType(MM.TextPanel, nameLike); 
        if (nameLike == "[LCD]" && SECONDARY_TAG != "") 
            textPanels.AddBlocksOfType(MM.TextPanel, SECONDARY_TAG); 
 
        for (int i = 0; i < textPanels.Count(); i++) 
        { 
            IMyTextPanel panel = (textPanels.Blocks[i] as IMyTextPanel); 
            string text = panel.CustomName; 
            MMPanel p = null; 
 
            int joinpos = text.IndexOf("!LINK:"); 
 
            if (joinpos < 0 || text.Length == joinpos + 6) 
            { 
                p = new MMPanel(); 
                if (panels.GetItem(text) != null) 
                { 
                    text += panel.NumberInGrid.ToString(); 
                } 
                p.panels.AddItem(text, panel); 
                panels.AddItem(text, p); 
                continue; 
            } 
 
            text = text.Substring(joinpos + 6); 
             
            string[] subs = text.Split(' '); 
            string group = subs[0]; 
            p = panels.GetItem(group); 
            if (p == null) 
            { 
                p = new MMPanel(); 
                panels.AddItem(group, p); 
            } 
            p.panels.AddItem(text, panel); 
        } 
    } 
 
    public void Run(int step) 
    { 
        if (panels.CountAll() == 0) 
            return; 
        for (int i = 0; i < PANELS_PER_STEP; i++) 
        { 
            RunSingle(panels.GetItemAt((step * PANELS_PER_STEP + i) % panels.CountAll())); 
        } 
    } 
 
    public void RunSingle(MMPanel panel) 
    { 
        panel.SortPanels(); 
        int isWide = (panel.IsWide()?1:0); 
        float fontSize = panel.GetFontSize(); 
        MMLCDTextManager.widthMod = (0.8f / fontSize) * (1 + isWide); 
        MMLCDTextManager.MMLCDText text = MMLCDTextManager.GetLCDText(panel);  
        text.SetTextFontSize(fontSize); 
        text.SetNumberOfScreens(panel.panels.CountAll()); 
 
        INV_PROGRESS_CHARS = (int)(DINV_PROGRESS_CHARS * MMLCDTextManager.widthMod); 
        STATS_PROGRESS_CHARS = (int)(DSTATS_PROGRESS_CHARS * MMLCDTextManager.widthMod); 
        FULL_PROGRESS_CHARS = (int)(DFULL_PROGRESS_CHARS * MMLCDTextManager.widthMod); 
 
        string pubText = panel.GetCustomName(); 
 
        if (pubText.Contains("#")) 
        { 
            pubText = pubText.Substring(pubText.LastIndexOf('#') + 1); 
        } 
        else 
        { 
            pubText = ""; 
        } 
 
        MMLCDTextManager.ClearText(panel); 
 
        if (pubText != "") 
            MMLCDTextManager.AddLine(panel, pubText); 
 
        string[] cmds = panel.GetPublicTitle().Split(';'); 
        for (int i = 0; i < cmds.Length; i++) 
        { 
            MM.Debug("Running command " + cmds[i]); 
            MMCommand cmd = new MMCommand(cmds[i]); 
 
            if (cmd.command.StartsWith("inventory") || 
                cmd.command == "missing" || 
                cmd.command.StartsWith("invlist")) 
                RunInvListing(panel, cmd); 
            else 
            if (cmd.command == "cargo" || 
                cmd.command == "cargoall") 
                RunCargoStatus(panel, cmd); 
            else 
            if (cmd.command == "power" || 
                cmd.command == "powersummary") 
                RunPowerStatus(panel, cmd); 
            else 
            if (cmd.command == "time") 
                RunCurrentTime(panel, cmd); 
            else 
            if (cmd.command == "echo") 
                RunEcho(panel, cmd); 
            else 
            if (cmd.command == "blockcount" || 
                cmd.command == "prodcount" || 
                cmd.command == "enabledcount") 
                RunBlockCount(panel, cmd); 
            else 
            if (cmd.command == "working") 
                RunWorkingList(panel, cmd); 
            else 
            if (cmd.command == "damage") 
                RunDamage(panel, cmd); 
            else 
            if (cmd.command == "pos") 
                RunPosition(panel, cmd); 
 
 
            MM.Debug("Done."); 
        } 
 
        if (!MM.EnableDebug) 
            MMLCDTextManager.UpdatePanel(panel); 
        //MM.Debug("Updated panel text."); 
    } 
 
    public void RunPosition(MMPanel panel, MMCommand cmd) 
    { 
        MMLCDTextManager.Add(panel, "Location: "); 
        MMLCDTextManager.AddRightAlign(panel, panel.GetPosition(), LCD_LINE_WORK_STATE_POS); 
        MMLCDTextManager.AddLine(panel, ""); 
    } 
 
    public void RunBlockCount(MMPanel panel, MMCommand cmd) 
    { 
        bool enabledCnt = (cmd.command == "enabledcount"); 
        bool producingCnt = (cmd.command == "prodcount"); 
        MMBlockCollection blocks = new MMBlockCollection(); 
        if (cmd.arguments.Count == 0) 
            blocks.Blocks = MM._GridTerminalSystem.Blocks; 
 
        for (int i = 0; i < cmd.arguments.Count; i++) 
        { 
            MMArgument arg = cmd.arguments[i]; 
 
            for (int subi = 0; subi < arg.sub.Count; subi++) 
            { 
                blocks.Clear(); 
                MM.GetBlocksOfType(ref blocks.Blocks, int.MaxValue, arg.sub[subi]); 
 
                MMBlockCollection f_blocks = blocks; 
                if (cmd.nameLike != "" && cmd.nameLike != "*") 
                { 
                    f_blocks = new MMBlockCollection(); 
                    blocks.GetBlocksWithNameLike(f_blocks, cmd.nameLike); 
                } 
 
                string name = ""; 
 
                if (f_blocks.Count() == 0) 
                { 
                    name = arg.sub[subi]; 
                    name = char.ToUpper(name[0]) + name.Substring(1).ToLower(); 
                    MMLCDTextManager.Add(panel, name + " count: "); 
                    string countstr = "0"; 
                    if (enabledCnt || producingCnt) 
                        countstr = "0 / 0"; 
                    MMLCDTextManager.AddRightAlign(panel, countstr, LCD_LINE_BLOCK_COUNT_POS); 
                    MMLCDTextManager.AddLine(panel, ""); 
                } 
                else 
                { 
                    Dictionary<string, int> typeCount = new Dictionary<string, int>(); 
                    Dictionary<string, int> typeWorkingCount = new Dictionary<string, int>(); 
                    List<string> blockTypes = new List<string>(); 
 
                    for (int j = 0; j < f_blocks.Count(); j++) 
                    { 
                        IMyProductionBlock prod = f_blocks.Blocks[j] as IMyProductionBlock; 
                        name = MM.GetBlockTypeDisplayName(f_blocks.Blocks[j]); 
                        if (blockTypes.Contains(name)) 
                        { 
                            typeCount[name]++; 
                            if ((enabledCnt && f_blocks.Blocks[j].IsWorking) || 
                                (producingCnt && prod != null && prod.IsProducing)) 
                                typeWorkingCount[name]++; 
                        } 
                        else 
                        { 
                            typeCount.Add(name, 1); 
                            blockTypes.Add(name); 
                            if (enabledCnt || producingCnt)  
                                if ((enabledCnt && f_blocks.Blocks[j].IsWorking) || 
                                    (producingCnt && prod != null && prod.IsProducing)) 
                                    typeWorkingCount.Add(name, 1); 
                                else 
                                    typeWorkingCount.Add(name, 0); 
                        } 
                    } 
                    for (int j = 0; j < typeCount.Count; j++) 
                    { 
                        MMLCDTextManager.Add(panel, blockTypes[j] + " count: "); 
                        string countstr = ""; 
                        if (enabledCnt || producingCnt) 
                            countstr = typeWorkingCount[blockTypes[j]].ToString() 
                                + " / " + typeCount[blockTypes[j]].ToString(); 
                        else 
                            countstr = typeCount[blockTypes[j]].ToString(); 
                        MMLCDTextManager.AddRightAlign(panel, countstr, LCD_LINE_BLOCK_COUNT_POS); 
                        MMLCDTextManager.AddLine(panel, ""); 
                    } 
                } 
            } 
        } 
    } 
 
    public string GetWorkingString(IMyTerminalBlock block) 
    { 
        if (!block.IsWorking) 
            return "OFF"; 
 
        IMyProductionBlock prod = block as IMyProductionBlock; 
        if (prod != null) 
            if (prod.IsProducing) 
                return "WORK"; 
            else 
                return "IDLE"; 
 
        IMyLandingGear gear = block as IMyLandingGear; 
        if (gear != null) 
            return MM.GetLandingGearStatus(gear); 
 
        IMyDoor door = block as IMyDoor; 
        if (door != null) 
        { 
            if (door.Open) 
                return "OPEN"; 
            else 
                return "CLOSED"; 
        } 
 
        IMyShipConnector conn = block as IMyShipConnector; 
        if (conn != null) 
            if (conn.IsLocked) 
                return "LOCK"; 
            else 
                return "UNLOCK"; 
 
        IMyLaserAntenna lasant = block as IMyLaserAntenna; 
        if (lasant != null) 
            return MM.GetLaserAntennaStatus(lasant); 
 
        return "ON"; 
    } 
 
    public void RunWorkingList(MMPanel panel, MMCommand cmd) 
    { 
        MMBlockCollection blocks = new MMBlockCollection(); 
 
        if (cmd.arguments.Count == 0) 
            blocks.Blocks = MM._GridTerminalSystem.Blocks; 
 
        for (int i = 0; i < cmd.arguments.Count; i++) 
        { 
            MMArgument arg = cmd.arguments[i]; 
 
            for (int subi = 0; subi < arg.sub.Count; subi++) 
            { 
                if (arg.sub[subi] == "") 
                    continue; 
                string[] subparts = arg.sub[subi].Split(':'); 
                string subargtype = subparts[0]; 
                string subargstate = ""; 
                if (subparts.Length > 1) 
                    subargstate = subparts[1].ToLower(); 
 
                blocks.Clear(); 
                MM.GetBlocksOfType(ref blocks.Blocks, int.MaxValue, subargtype); 
 
                MMBlockCollection f_blocks = blocks; 
                if (cmd.nameLike != "" && cmd.nameLike != "*") 
                { 
                    f_blocks = new MMBlockCollection(); 
                    blocks.GetBlocksWithNameLike(f_blocks, cmd.nameLike); 
                } 
 
                if (f_blocks.Count() > 0) { 
                    Dictionary<string, int> typeCount = new Dictionary<string, int>(); 
                    List<string> blockTypes = new List<string>(); 
 
                    for (int j = 0; j < f_blocks.Count(); j++) 
                    { 
                        IMyTerminalBlock block = f_blocks.Blocks[j]; 
                        if (block == null) 
                            continue; 
 
                        string onoff = GetWorkingString(block); 
                        if (subargstate != "" && onoff.ToLower() != subargstate) 
                            continue; 
                        string blockName = block.CustomName; 
                        blockName = MMStringFunc.GetStringTrimmed(blockName, LCD_LINE_WORK_STATE_POS - 60); 
                        MMLCDTextManager.Add(panel, blockName); 
                        MMLCDTextManager.AddRightAlign(panel, onoff, LCD_LINE_WORK_STATE_POS); 
                        MMLCDTextManager.AddLine(panel, ""); 
                    } 
                } 
            } 
        } 
    } 
 
    public void RunEcho(MMPanel panel, MMCommand cmd) 
    { 
        int idx = cmd.commandLine.IndexOf(' '); 
        string msg = ""; 
        if (idx >= 0) 
            msg = cmd.commandLine.Substring(idx + 1); 
 
        MMLCDTextManager.AddLine(panel, msg); 
    } 
 
    public void RunDamage(MMPanel panel, MMCommand cmd) 
    { 
        MMBlockCollection blocks = new MMBlockCollection(); 
 
        if (cmd.nameLike == "" || cmd.nameLike == "*") 
            blocks.Blocks = MM._GridTerminalSystem.Blocks; 
        else 
            blocks.AddBlocksOfNameLike(cmd.nameLike); 
 
        bool found = false; 
 
        MMList<IMySlimBlock> slims = blocks.GetSlimBlocks(); 
        for (int i = 0; i < slims.Count; i++) 
        { 
            IMySlimBlock slim = slims[i]; 
            float hull = (slim.BuildIntegrity - slim.CurrentDamage); 
            float perc = 100 * (hull / slim.MaxIntegrity); 
 
            /*string debug = "AccDmg: " + slim.AccumulatedDamage.ToString() + " BlvlR: " + slim.BuildLevelRatio.ToString() + 
                " CurDmg: " + slim.CurrentDamage.ToString() + " DmgRt: " + slim.DamageRatio + " MaxDef: " + slim.MaxDeformation; 
            MMLCDTextManager.AddLine(panel, debug);*/ 
            if (perc >= 100) 
                continue; 
 
            found = true; 
             
            MMLCDTextManager.Add(panel, MMStringFunc.GetStringTrimmed(slim.FatBlock.DisplayNameText,  
                LCD_LINE_DMG_NUMBERS_POS - 70) + " "); 
            MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(hull) + " / ",  
                LCD_LINE_DMG_NUMBERS_POS); 
            MMLCDTextManager.Add(panel, MM.FormatLargeNumber(slim.MaxIntegrity)); 
            MMLCDTextManager.AddRightAlign(panel, ' ' + perc.ToString("0.0") + "%", LCD_LINE_WIDTH); 
            MMLCDTextManager.AddLine(panel, ""); 
            MMLCDTextManager.AddProgressBar(panel, perc, FULL_PROGRESS_CHARS); 
            MMLCDTextManager.AddLine(panel, ""); 
        } 
 
        if (!found) 
            MMLCDTextManager.AddLine(panel, "No damaged blocks found."); 
    } 
 
    public void RunCargoStatus(MMPanel panel, MMCommand cmd) 
    { 
        MMBlockCollection blocks = new MMBlockCollection(); 
        bool alltypes = (cmd.command == "cargoall"); 
        bool allnames = (cmd.nameLike == "" || cmd.nameLike == "*"); 
 
        if (alltypes) 
        { 
            if (allnames) 
                blocks.Blocks = MM._GridTerminalSystem.Blocks; 
            else 
                blocks.AddBlocksOfNameLike(cmd.nameLike); 
        } 
        else 
        { 
            if (allnames) 
                blocks.AddBlocksOfType(MM.CargoContainer); 
            else 
                blocks.AddBlocksOfType(MM.CargoContainer, 
                    cmd.nameLike); 
        } 
 
        double usedCargo = 0; 
        double totalCargo = 0; 
        double percentCargo = blocks.GetCargoSpaceSummary( 
            out usedCargo, out totalCargo); 
 
        MMLCDTextManager.Add(panel, "Cargo Space: "); 
        MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(usedCargo) + "L / ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(totalCargo) + "L"); 
        MMLCDTextManager.AddProgressBar(panel, percentCargo, STATS_PROGRESS_CHARS); 
        MMLCDTextManager.AddLine(panel, ' ' + percentCargo.ToString("0.0") + "%"); 
    } 
 
    public void ShowPowerOutput(MMPanel panel, MMBlockCollection generators, string title) 
    { 
        double usedPower = 0; 
        double totalPower = 0; 
        double percentPower = generators.GetPowerOutput( 
            out usedPower, out totalPower); 
 
        MMLCDTextManager.Add(panel, title + ": "); 
        MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(usedPower) + "W / ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(totalPower) + "W"); 
        MMLCDTextManager.AddProgressBar(panel, percentPower, STATS_PROGRESS_CHARS); 
        MMLCDTextManager.AddLine(panel, ' ' + percentPower.ToString("0.0") + "%"); 
    } 
 
    public void ShowBatteriesInfo(MMPanel panel, MMBlockCollection batteries, string title) 
    { 
        double output = 0; 
        double max_output = 0; 
        double input = 0; 
        double max_input = 0; 
 
        double stored = 0; 
        double max_stored = 0; 
        double percent_stored = 
            batteries.GetBatteryStats( 
                out output, out max_output, 
                out input, out max_input, 
                out stored, out max_stored); 
        double percent_output = MM.GetPercent(output, max_output); 
        double percent_input = MM.GetPercent(input, max_input); 
 
        MMLCDTextManager.Add(panel, title + ": "); 
        MMLCDTextManager.AddRightAlign(panel, "(IN " + MM.FormatLargeNumber(input) + "W / OUT ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(output) + "W)"); 
         
        MMLCDTextManager.Add(panel, "  Power Stored: "); 
        MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(stored) + "W / ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(max_stored) + "W"); 
        MMLCDTextManager.AddProgressBar(panel, percent_stored, STATS_PROGRESS_CHARS); 
        MMLCDTextManager.AddLine(panel, ' ' + percent_stored.ToString("0.0") + "%"); 
 
        MMLCDTextManager.Add(panel, "  Power Output: "); 
        MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(output) + "W / ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(max_output) + "W"); 
        MMLCDTextManager.AddProgressBar(panel, percent_output, STATS_PROGRESS_CHARS); 
        MMLCDTextManager.AddLine(panel, ' ' + percent_output.ToString("0.0") + "%"); 
 
        MMLCDTextManager.Add(panel, "  Power Input: "); 
        MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(input) + "W / ", LCD_LINE_STATS_POS); 
        MMLCDTextManager.AddLine(panel, MM.FormatLargeNumber(max_input) + "W"); 
        MMLCDTextManager.AddProgressBar(panel, percent_input, STATS_PROGRESS_CHARS); 
        MMLCDTextManager.AddLine(panel, ' ' + percent_input.ToString("0.0") + "%"); 
    } 
 
    public void RunPowerStatus(MMPanel panel, MMCommand cmd) 
    { 
        MMBlockCollection reactors = new MMBlockCollection(); 
        MMBlockCollection solars = new MMBlockCollection(); 
        MMBlockCollection batteries = new MMBlockCollection(); 
        int got = 0; 
        bool issummary = (cmd.command == "powersummary"); 
 
        if (cmd.nameLike == "*") 
            cmd.nameLike = ""; 
 
        reactors.AddBlocksOfType(MM.Reactor, 
            cmd.nameLike); 
        solars.AddBlocksOfType(MM.SolarPanel, 
            cmd.nameLike); 
        batteries.AddBlocksOfType(MM.BatteryBlock, 
            cmd.nameLike); 
 
        got = 0; 
        if (reactors.Count() > 0) got++; 
        if (solars.Count() > 0) got++; 
        if (batteries.Count() > 0) got++; 
 
        if (got < 1) 
        { 
            MMLCDTextManager.AddLine(panel, "No power source found!"); 
            return; 
        } 
 
        string title = "Total Output"; 
 
        if (!issummary) 
        { 
            if (reactors.Count() > 0) 
                ShowPowerOutput(panel, reactors, "Reactor Output"); 
            if (solars.Count() > 0) 
                ShowPowerOutput(panel, solars, "Solar Output"); 
            if (batteries.Count() > 0) 
                ShowBatteriesInfo(panel, batteries, "Batteries Status"); 
        } 
        else 
        { 
            title = "Power Output"; 
            got = 10; // hack ;) 
        } 
 
        if (got == 1) 
            return; 
 
        MMBlockCollection blocks = new MMBlockCollection(); 
        blocks.AddFromCollection(reactors); 
        blocks.AddFromCollection(solars); 
        blocks.AddFromCollection(batteries); 
        ShowPowerOutput(panel, blocks, title); 
    } 
 
    public void RunCurrentTime(MMPanel panel, MMCommand cmd) 
    { 
        int first_space = cmd.commandLine.IndexOf(' '); 
        if (first_space >= 0) 
            MMLCDTextManager.Add(panel, cmd.commandLine.Substring(first_space + 1)); 
        MMLCDTextManager.AddLine(panel, DateTime.Now.ToShortTimeString()); 
    } 
 
    private bool IsMainType(string subarg) 
    { 
        return (subarg == "ingot" || subarg == "ore" || 
            subarg == "component" || subarg == "ammo" || 
            subarg == "tool" || subarg == "physicalgunobject" || 
            subarg == "ammomagazine"); 
    } 
 
    private void ShowInventoryLine(MMPanel panel, string msg, double num, double num_ores, int quota) 
    { 
        if (quota > 0) 
        { 
 
            double perc = 100; 
            perc = Math.Min(100, 100 * num / quota); 
 
            if (num_ores >= 0) 
            { 
                MMLCDTextManager.Add(panel, msg + ' '); 
                MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(num), LCD_LINE_INGOT_NUMBERS_POS); 
                MMLCDTextManager.Add(panel, " / " + MM.FormatLargeNumber(quota)); 
                MMLCDTextManager.AddRightAlign(panel, "+" + MM.FormatLargeNumber(num_ores) + " ore", LCD_LINE_WIDTH); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
                MMLCDTextManager.AddProgressBar(panel, perc, FULL_PROGRESS_CHARS); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
            } 
            else 
            { 
                MMLCDTextManager.AddProgressBar(panel, perc, INV_PROGRESS_CHARS); 
                MMLCDTextManager.Add(panel, ' ' + msg + ' '); 
                MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(num), LCD_LINE_INV_NUMBERS_POS); 
                MMLCDTextManager.AddLine(panel, " / " + MM.FormatLargeNumber(quota)); 
            } 
        } 
        else 
        { 
            if (num_ores >= 0) 
            { 
                MMLCDTextManager.Add(panel, msg + ':'); 
                MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(num), LCD_LINE_INGOT_NUMBERS_POS); 
                MMLCDTextManager.AddRightAlign(panel, "+" + MM.FormatLargeNumber(num_ores) + " ore", LCD_LINE_WIDTH); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
            } 
            else 
            { 
                MMLCDTextManager.Add(panel, msg + ':'); 
                MMLCDTextManager.AddRightAlign(panel, MM.FormatLargeNumber(num), LCD_LINE_NUMERS_POS); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
            } 
        } 
    } 
 
    public void PrintItemsOfMain(MMPanel panel, MMItemAmounts amounts, bool missing, bool simple, string mainType, string displayType) 
    { 
        MMList<MMAmountSpec> items = amounts.GetAmountsOfMain(mainType); 
        MM.Debug("Geting " + items.Count.ToString() + " items of Main " + mainType); 
        if (items.Count > 0) 
        { 
            if (!simple) 
            { 
                if (MMLCDTextManager.GetLCDText(panel).current_line > 0 && MMLCDTextManager.GetLCDText(panel).lines[0] != "") 
                    MMLCDTextManager.AddLine(panel, "");    // add empty line 
                MMLCDTextManager.AddCenter(panel, "<< " + displayType + " summary >>", LCD_LINE_WIDTH / 2); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
            } 
             
            for (int i = 0; i < items.Count; i++) 
            { 
                double num = items[i].current; 
 
                if (missing && num >= items[i].min) 
                    continue; 
 
                MM.Debug("Printing " + items[i].subType); 
                int quota = items[i].max; 
                if (missing) 
                    quota = items[i].min; 
 
                string msg = MM.TranslateToDisplay(items[i].subType, items[i].mainType); 
                if (msg.EndsWith(" Component") || msg.EndsWith(" Ore") || 
                    msg.EndsWith(" Ingot") || msg.EndsWith(" AmmoMagazine") || 
                    msg.EndsWith(" Item")) 
                    msg = msg.Substring(0, msg.LastIndexOf(' ')); 
 
                ShowInventoryLine(panel, msg, num, -1, quota); 
            } 
        } 
    } 
 
    public void RunInvListing(MMPanel panel, MMCommand cmd) 
    { 
        MMBlockCollection blocks = new MMBlockCollection(); 
        bool noexpand = false; 
        if (cmd.command[cmd.command.Length - 1] == 'x') 
        { 
            cmd.command = cmd.command.Substring(0, cmd.command.Length - 1); 
            noexpand = true; 
        } 
 
        bool missing = (cmd.command == "missing"); 
        bool simple = (cmd.command == "invlist"); 
 
 
        if (cmd.nameLike == "" || cmd.nameLike == "*") 
            blocks.Blocks = MM._GridTerminalSystem.Blocks; 
        else 
            blocks.AddBlocksOfNameLike(cmd.nameLike); 
 
        MMItemAmounts amounts = new MMItemAmounts(); 
        MMList<MMArgument> args = cmd.arguments; 
        if (args.Count == 0) 
        { 
            args.Add(new MMArgument("all", false)); 
        } 
 
        for (int i = 0; i < args.Count; i++) 
        { 
            MMArgument arg = args[i]; 
            string mainType = arg.main.ToLower(); 
 
            MM.Debug("Processing sub args"); 
            for (int subi = 0; subi < arg.sub.Count; subi++) 
            { 
                string[] subs = arg.sub[subi].Split(':'); 
                double number = Double.NaN; 
                MM.Debug("Processing sub arg " + subs[0]); 
 
                if (subs[0] == "all") 
                    subs[0] = ""; 
 
                int min = 1; 
                int max = -1; 
                if (subs.Length > 1) 
                { 
                    if (Double.TryParse(subs[1], out number)) 
                    { 
                        if (missing) 
                            min = (int)Math.Ceiling(number); 
                        else 
                            max = (int)Math.Ceiling(number); 
                    } 
                } 
 
                string subfulltype = subs[0]; 
                if (mainType != "") 
                    subfulltype += ' ' + mainType; 
                amounts.AddSpec(subfulltype, (arg.op == "-"), min, max); 
            } 
            MM.Debug("Sub args processed"); 
        } 
        MM.Debug("All args processed"); 
 
        if (!noexpand) 
        { 
            amounts.ExpandSpecs(); 
            MM.Debug("Expanded specs"); 
        } 
        MM.Debug("Entering process function"); 
        amounts.ProcessItemsFromBlockCollection(blocks); 
        MM.Debug("Processed items from blocks"); 
 
        PrintItemsOfMain(panel, amounts, missing, simple, "Ore", "Ores"); 
        MM.Debug("Printed ores"); 
 
        MMList<MMAmountSpec> ingots = amounts.GetAmountsOfMain("Ingot"); 
        if (ingots.Count > 0) 
        { 
            if (!simple) 
            { 
                if (MMLCDTextManager.GetLCDText(panel).current_line > 0 && MMLCDTextManager.GetLCDText(panel).lines[0] != "") 
                    MMLCDTextManager.AddLine(panel, "");    // add empty line 
                MMLCDTextManager.AddCenter(panel, "<< Ingots summary >>", LCD_LINE_WIDTH / 2); 
                MMLCDTextManager.AddLine(panel, "");    // next line 
            } 
 
            for (int i = 0; i < ingots.Count; i++) 
            { 
                double num = ingots[i].current; 
 
                if (missing && num >= ingots[i].min) 
                    continue; 
 
                MM.Debug("Processing ingot " + ingots[i].subType); 
                double num_ores = Double.NaN; 
                if (ingots[i].subType != "Scrap") 
                    num_ores = amounts.GetAmountSpec(ingots[i].subType + " Ore", ingots[i].subType, "Ore").current; 
 
                int quota = ingots[i].max; 
                if (missing) 
                    quota = ingots[i].min; 
 
                string msg = MM.TranslateToDisplay(ingots[i].subType, ingots[i].mainType); 
                if (msg.EndsWith(" Component") || msg.EndsWith(" Ore") || 
                    msg.EndsWith(" Ingot") || msg.EndsWith(" AmmoMagazine") || 
                    msg.EndsWith(" Item")) 
                    msg = msg.Substring(0, msg.LastIndexOf(' ')); 
 
                ShowInventoryLine(panel, msg, num, num_ores, quota); 
            } 
        } 
        MM.Debug("Printed ingots"); 
         
        PrintItemsOfMain(panel, amounts, missing, simple, "Component", "Components"); 
        PrintItemsOfMain(panel, amounts, missing, simple, "AmmoMagazine", "Ammo"); 
        PrintItemsOfMain(panel, amounts, missing, simple, "PhysicalGunObject", "Tools"); 
    } 
} 
 
// MMAPI below (do not modify)   
public class MMCommand 
{ 
    // EXAMPLE: 
    // COMMAND NAMEFILTER ARGUMENTS 
    // Legend: 
    //      {MULTIWORD ARGUMENT} ARGUMENT +ARG/SUBARG:count,SUBARG2:count -ARG/SUBARG ARG/SUBARG 
    // Inventory {My Crago Container} 
    // Inventory {My Crago Container} +ore -stone 
    // Inventory {[Station Cargo]} +uranium 
    // Inventory [CARGO] +ingot/iron,platinum,gold 
    // Inventory Cargo +all -ore -ingot 
    // Inventory Cargo +component -steelplate +ammo +tool 
    // Inventory * +ore 
    // Inventory * 
    // Power 
    // Power [MAIN REACTORS] 
    // Cargospace [STORAGE] 
    // Blockstats [STATION] thruster reactor 
    // Blockstats * thruster reactor 
    public string command = ""; 
    public string nameLike = ""; 
    public string commandLine = ""; 
 
    public MMList<MMArgument> arguments = new MMList<MMArgument>(); 
 
    public MMCommand(string _commandLine) 
    { 
        commandLine = _commandLine; 
        if (commandLine == "") 
            return; 
 
        string[] args = commandLine.Split(' '); 
        string arg = ""; 
        string fullArg = ""; 
        bool multiWord = false; 
 
        command = args[0].ToLower(); 
 
        for (int i = 1; i < args.Length; i++) 
        { 
            arg = args[i]; 
            if (arg == "") 
                continue; 
 
            if (arg[0] == '{' && arg[arg.Length - 1] == '}') 
            { 
                arg = arg.Substring(1, arg.Length - 2); 
                if (arg == "") 
                    continue; 
                if (nameLike == "") 
                    nameLike = arg; 
                else 
                    arguments.Add(new MMArgument(arg.ToLower(), false)); 
                continue; 
            } 
            if (arg[0] == '{') 
            { 
                multiWord = true; 
                fullArg = arg.Substring(1); 
                continue; 
            } 
            if (arg[arg.Length - 1] == '}') 
            { 
                multiWord = false; 
                fullArg += ' ' + arg.Substring(0, arg.Length - 1); 
                if (nameLike == "") 
                    nameLike = fullArg; 
                else 
                    arguments.Add(new MMArgument(fullArg.ToLower(), false)); 
                continue; 
            } 
 
            if (multiWord) 
            { 
                if (fullArg.Length != 0) 
                    fullArg += ' '; 
                fullArg += arg; 
                continue; 
            } 
 
            if (nameLike == "") 
                nameLike = arg; 
            else 
                arguments.Add(new MMArgument(arg.ToLower())); 
        } 
    } 
} 
 
public class MMArgument 
{ 
    public string op = ""; 
    public string main = ""; 
    public List<string> sub = new List<string>(); 
 
    public MMArgument(string arg, bool parse = true) 
    { 
        if (!parse) 
        { 
            main = ""; 
            sub.Add(arg); 
            return; 
        } 
 
        string cur = arg.Trim(); 
        if (cur[0] == '+' || cur[0] == '-') 
        { 
            op += cur[0]; 
            cur = arg.Substring(1); 
        } 
 
        string[] parts = cur.Split('/'); 
        string subargs = parts[0]; 
 
        if (parts.Length > 1) 
        { 
            main = parts[0]; 
            subargs = parts[1]; 
        } 
        else 
        { 
            main = ""; 
        } 
 
        if (subargs.Length > 0) 
        { 
            string[] subs = subargs.Split(','); 
            for (int i = 0; i < subs.Length; i++) 
                if (subs[i] != "") 
                    sub.Add(subs[i]); 
        } 
    } 
} 
 
// IMyTerminal reactors collection with useful methods   
public class MMBlockCollection 
{ 
    public List<IMyTerminalBlock> Blocks = new List<IMyTerminalBlock>(); 
 
    // Return percent of cargo used by all reactors in collection   
    // usedAmount and totalAmount are filled with volumes from all reactors in collection   
    public double GetCargoSpaceSummary(out double usedAmount, out double totalAmount) 
    { 
        usedAmount = 0; 
        totalAmount = 0; 
 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            MMInventory inventory = new MMInventory(Blocks[i], 0); 
 
            if (!inventory.Exists()) 
                continue; 
 
            usedAmount += inventory.GetUsedVolume(); 
            totalAmount += inventory.GetTotalVolume(); 
        } 
 
        usedAmount *= 1000; 
        totalAmount *= 1000; 
        return MM.GetPercent(usedAmount, totalAmount); 
    } 
     
    public double GetPowerOutput(out double current, out double max) 
    { 
        max = 0; 
        current = 0; 
 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            IMyBatteryBlock bat = (Blocks[i] as IMyBatteryBlock); 
            List<double> vals = MM.GetDetailedInfoValues(Blocks[i]); 
            if ((bat != null && vals.Count < 6) ||  
                (bat == null && vals.Count < 2)) 
                continue; 
 
            max += vals[0]; 
 
            if (bat != null) 
                current += vals[4]; 
            else 
                current += vals[1]; 
        } 
        return MM.GetPercent(current, max); 
    } 
 
 
    // returns percent stored 
    public double GetBatteryStats(out double output, out double max_output, 
                                    out double input, out double max_input, 
                                    out double stored, out double max_stored) 
    { 
        max_output = 0; 
        output = 0; 
        max_input = 0; 
        input = 0; 
        max_stored = 0; 
        stored = 0; 
 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            List<double> vals = MM.GetDetailedInfoValues(Blocks[i]); 
            if (vals.Count < 6) 
                continue; 
 
            max_output += vals[0]; 
            max_input += vals[1]; 
            max_stored += vals[2]; 
            input += vals[3]; 
            output += vals[4]; 
            stored += vals[5]; 
 
        } 
        return MM.GetPercent(stored, max_stored); 
    } 
 
    // add Blocks with name containing nameLike   
    public void AddBlocksOfNameLike(string nameLike) 
    { 
        MM._GridTerminalSystem.SearchBlocksOfName(nameLike, Blocks); 
    } 
 
    // add Blocks of type T (optional: with name containing nameLike)   
    public void AddBlocksOfType(int type, string nameLike = "") 
    { 
        if (nameLike == "") 
        { 
            MM.GetBlocksOfType(ref Blocks, type); 
        } 
        else 
        { 
            List<IMyTerminalBlock> blocksOfType = new List<IMyTerminalBlock>(); 
            MM.GetBlocksOfType(ref blocksOfType, type); 
 
            for (int i = 0; i < blocksOfType.Count; i++) 
            { 
                IMyTerminalBlock block = blocksOfType[i]; 
                if (block == null) 
                    continue; 
 
                if (block.CustomName.Contains(nameLike)) 
                    Blocks.Add(block); 
            } 
        } 
    } 
 
    // add all Blocks from collection col to this collection   
    public void AddFromCollection(MMBlockCollection col) 
    { 
        Blocks.AddList(col.Blocks); 
    } 
 
    // Add Blocks from this collection containing item of subType and/or mainType to collection dest   
    // (optional: look in inventory index invID)   
    // Note: If both subType and mainType are "", adds Blocks with inventories   
    public void GetBlocksWithItem(MMBlockCollection dest, string subType = "", string mainType = "", int invID = 0) 
    { 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            IMyInventory inv = MM.GetBlockInventory(Blocks[i], invID); 
 
            if (inv == null) 
                continue; 
 
            subType = subType.ToLower(); 
            mainType = mainType.ToLower(); 
 
            List<IMyInventoryItem> items = inv.GetItems(); 
            for (int y = 0; y < items.Count; y++) 
            { 
                IMyInventoryItem item = items[y]; 
                if ((subType != "" && item.Content.SubtypeName.ToLower() != subType) || 
                    (mainType != "" && !item.Content.TypeId.ToString().ToLower().EndsWith(mainType))) 
                    continue; 
 
                // item was found add the block and break   
                dest.Blocks.Add(Blocks[i]); 
                break; 
            } 
        } 
    } 
 
    // Add Blocks from this collection with name containing nameLike to dest collection   
    public void GetBlocksWithNameLike(MMBlockCollection dest, string nameLike) 
    { 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            if (Blocks[i].CustomName.Contains(nameLike)) 
                dest.Blocks.Add(Blocks[i]); 
        } 
    } 
 
    // Get slim blocks 
    public MMList<IMySlimBlock> GetSlimBlocks() { 
        MMList<IMySlimBlock> slim = new MMList<IMySlimBlock>(); 
 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            IMyTerminalBlock block = Blocks[i]; 
            slim.Add(block.CubeGrid.GetCubeBlock(block.Position)); 
        } 
 
        return slim; 
    } 
     
    // Add Functional blocks from this collection that are Working to dest collection 
    public void GetWorkingBlocks(MMBlockCollection dest) 
    { 
        for (int i = 0; i < Blocks.Count; i++) 
        { 
            IMyFunctionalBlock block = Blocks[i] as IMyFunctionalBlock; 
 
            if (block != null && block.IsWorking) 
                dest.Blocks.Add(Blocks[i]); 
        } 
    } 
 
    // clear all reactors from this collection   
    public void Clear() 
    { 
        Blocks.Clear(); 
    } 
 
    // number of reactors in collection   
    public int Count() 
    { 
        return Blocks.Count; 
    } 
} 
 
public class MMAmountSpec 
{ 
    public int min = 0; 
    public int max = 0; 
    public string subType = ""; 
    public string mainType = ""; 
    public bool ignore = false; 
    public double current = 0; 
 
    public MMAmountSpec(bool _ignore = false, int _min = 1, int _max = -1) 
    { 
        min = _min; 
        ignore = _ignore; 
        max = _max; 
    } 
} 
 
// Item amounts class [WIP] 
public class MMItemAmounts 
{ 
    public MMDict<string, MMAmountSpec> specBySubLower; 
    public MMDict<string, MMAmountSpec> specByMainLower; 
    public MMDict<string, MMAmountSpec> specByFullLower; 
    public bool specAll; 
 
    public MMDict<string, MMAmountSpec> amountByFullType; 
 
    public MMItemAmounts(int size = 20) 
    { 
        specBySubLower = new MMDict<string, MMAmountSpec>(10); 
        specByMainLower = new MMDict<string, MMAmountSpec>(5); 
        specByFullLower = new MMDict<string, MMAmountSpec>(10); 
        specAll = false; 
        amountByFullType = new MMDict<string, MMAmountSpec>(size); 
    } 
 
    private bool IsMain(string subarg) 
    { 
        return (subarg == "ingot" || subarg == "ore" || 
            subarg == "component" || subarg == "ammo" || 
            subarg == "tool" || subarg == "physicalgunobject" || 
            subarg == "ammomagazine"); 
    } 
 
    public void AddSpec(string subfulltype, bool ignore = false, int min = 1, int max = -1) 
    { 
        if (subfulltype == "") 
        { 
            specAll = true; 
            return; 
        } 
 
        string[] parts = subfulltype.Split(' '); 
 
        string subType = ""; 
        string mainType = ""; 
        MMAmountSpec spec = new MMAmountSpec(ignore, min, max); 
 
        if (parts.Length == 2) 
        { 
            mainType = parts[1]; 
 
            if (mainType == "tool") 
                mainType = "physicalgunobject"; 
            else 
                if (mainType == "ammo") 
                    mainType = "ammomagazine"; 
        } 
         
        subType = parts[0].ToLower(); 
 
        if (IsMain(subType)) 
        { 
            if (subType == "tool") 
                subType = "physicalgunobject"; 
            else 
                if (subType == "ammo") 
                    subType = "ammomagazine"; 
            spec.mainType = subType; 
 
            specByMainLower.AddItem(spec.mainType, spec); 
            return; 
        } 
 
        MM.TranslateToInternal(ref subType, ref mainType); 
        if (mainType == "") 
        { 
            spec.subType = subType.ToLower(); 
            specBySubLower.AddItem(spec.subType, spec); 
            return; 
        } 
         
        spec.subType = subType; 
        spec.mainType = mainType; 
        specByFullLower.AddItem(subType.ToLower() + ' ' + mainType.ToLower(), spec); 
    } 
 
    public MMAmountSpec GetSpec(string fullType, string subType, string mainType) 
    { 
        MMAmountSpec spec; 
         
        fullType = fullType.ToLower(); 
        spec = specByFullLower.GetItem(fullType); 
        if (spec != null) 
            return spec; 
 
        subType = subType.ToLower(); 
        spec = specBySubLower.GetItem(subType); 
        if (spec != null) 
            return spec; 
 
        mainType = mainType.ToLower(); 
        spec = specByMainLower.GetItem(mainType); 
        if (spec != null) 
            return spec; 
 
        return null; 
    } 
 
    public bool IsIgnored(string fullType, string subType, string mainType) 
    { 
        MMAmountSpec spec; 
        bool found = false; 
 
        mainType = mainType.ToLower(); 
        spec = specByMainLower.GetItem(mainType); 
        if (spec != null) 
        { 
            if (spec.ignore) 
                return true; 
            found = true; 
        } 
        subType = subType.ToLower(); 
        spec = specBySubLower.GetItem(subType); 
        if (spec != null) 
        { 
            if (spec.ignore) 
                return true; 
            found = true; 
        } 
        fullType = fullType.ToLower(); 
        spec = specByFullLower.GetItem(fullType); 
        if (spec != null) 
        { 
            if (spec.ignore) 
                return true; 
            found = true; 
        } 
 
        return !(specAll || found); 
    } 
 
    public MMAmountSpec CreateAmountSpec(string fullType, string subType, string mainType) 
    { 
        MMAmountSpec amount = new MMAmountSpec(); 
 
        fullType = fullType.ToLower(); 
        MMAmountSpec spec = GetSpec(fullType, subType.ToLower(), mainType.ToLower()); 
        if (spec != null) 
        { 
            amount.min = spec.min; 
            amount.max = spec.max; 
        } 
        amount.subType = subType; 
        amount.mainType = mainType; 
 
        amountByFullType.AddItem(fullType, amount); 
 
        return amount; 
    } 
 
    public MMAmountSpec GetAmountSpec(string fullType, string subType, string mainType) 
    { 
        MMAmountSpec amount = amountByFullType.GetItem(fullType.ToLower()); 
        if (amount == null) 
            amount = CreateAmountSpec(fullType, subType, mainType); 
        return amount; 
    } 
 
    public MMList<MMAmountSpec> GetAmountsOfMain(string mainType) 
    { 
        MMList<MMAmountSpec> result = new MMList<MMAmountSpec>(); 
 
        for (int i = 0; i < amountByFullType.CountAll(); i++) 
        { 
            MMAmountSpec spec = amountByFullType.GetItemAt(i); 
            MM.Debug("AmountsOfMain: spec " + spec.subType + ' ' + spec.mainType); 
            if (IsIgnored((spec.subType + ' ' + spec.mainType).ToLower(),  
                    spec.subType, spec.mainType)) 
                continue; 
            if (spec.mainType == mainType) 
            { 
                result.Add(spec); 
                MM.Debug("Added"); 
            } 
        } 
 
        return result; 
    } 
 
    public void ExpandSpecs() 
    { 
        for (int i = 0; i < MMItems.items_keys.Count; i++) 
        { 
            MMItem item = MMItems.items[MMItems.items_keys[i]]; 
            string fullType = item.subType + ' ' + item.mainType; 
 
            if (IsIgnored(fullType, item.subType, item.mainType)) 
                continue; 
 
            MMAmountSpec amount = GetAmountSpec(fullType, item.subType, item.mainType); 
            if (amount.max == -1) 
                amount.max = item.defaultQuota; 
        } 
    } 
 
    public void ProcessItemsFromBlockCollection(MMBlockCollection col) 
    { 
        for (int i = 0; i < col.Count(); i++) 
        { 
            for (int invId = 0; invId < col.Blocks[i].GetInventoryCount(); invId++) 
            { 
                IMyInventory inv = MM.GetBlockInventory(col.Blocks[i], invId); 
                if (inv == null) 
                    continue; 
 
                List<IMyInventoryItem> items = inv.GetItems(); 
                for (int j = 0; j < items.Count; j++) 
                { 
                    IMyInventoryItem item = items[j]; 
                    string fullType = MM.GetItemFullType(item); 
                    string fullTypeL = fullType.ToLower(); 
                    string subType = ""; 
                    string mainType = ""; 
                    MM.ParseFullType(fullTypeL, out subType, out mainType); 
 
                    MM.Debug("Got full item type: " + fullType); 
 
                    if (mainType == "ore") 
                    { 
                        if (IsIgnored(subType + " ingot", subType, "Ingot") && 
                            IsIgnored(fullType, subType, mainType)) 
                            continue; 
                    } 
                    else 
                    { 
                        if (IsIgnored(fullType, subType, mainType)) 
                            continue; 
                    } 
 
                    MM.Debug("Item not ignored"); 
                    MM.ParseFullType(fullType, out subType, out mainType); 
                    MMAmountSpec amount = GetAmountSpec(fullTypeL, subType, mainType); 
                    amount.current += (double)item.Amount; 
                } 
            } 
        } 
    } 
} 
 
// Inventory management class  MMInventory 
// holds owner block of inventory, inventory id in owner inventories   
public class MMInventory 
{ 
    public IMyTerminalBlock OwnerBlock = null; 
    public IMyInventory Inventory = null; 
    public int InventoryId = -1; 
 
    // initialize with everything preset   
    public MMInventory(IMyTerminalBlock owner, IMyInventory inventory, int invID) 
    { 
        OwnerBlock = owner; 
        Inventory = inventory; 
        InventoryId = invID; 
    } 
 
    // initialize by getting inventory at invID from owner   
    public MMInventory(IMyTerminalBlock owner, int invID) 
    { 
        OwnerBlock = owner; 
        InventoryId = invID; 
        Inventory = MM.GetBlockInventory(OwnerBlock, InventoryId); 
    } 
 
    // get all items from this inventory (auto-cached)   
    public List<IMyInventoryItem> GetItems() 
    { 
        if (Inventory == null) 
            return null; 
 
        return Inventory.GetItems(); ; 
    } 
 
    // does this inventory exist?    
    // (does owner have inventory with this ID?)   
    public bool Exists() 
    { 
        return Inventory != null; 
    } 
 
    // return how full the inventory is in percent   
    public double PercentFull() 
    { 
        if (Inventory == null) 
            return 100.0f; 
 
        return 100.0f * (double)Inventory.CurrentVolume / (double)Inventory.MaxVolume; 
    } 
 
    // return used volume   
    public double GetUsedVolume() 
    { 
        if (Inventory == null) 
            return 0; 
        return (double)Inventory.CurrentVolume; 
    } 
 
    // return total volume   
    public double GetTotalVolume() 
    { 
        if (Inventory == null) 
            return 0; 
        return (double)Inventory.MaxVolume; 
    } 
} 
 
// MMAPI Helper functions   
public static class MM 
{ 
    public static bool EnableDebug = false; 
    public static IMyGridTerminalSystem _GridTerminalSystem = null; 
    public static MMBlockCollection _DebugTextPanels = null; 
 
    public static void Init(IMyGridTerminalSystem gridSystem, bool _EnableDebug) 
    { 
        _GridTerminalSystem = gridSystem; 
        EnableDebug = _EnableDebug; 
 
        _DebugTextPanels = new MMBlockCollection(); 
 
        MMStringFunc.InitCharSizes(); 
 
        // prepare debug panels 
        // select all text panels with [DEBUG] in name  
        if (_EnableDebug) 
        { 
            _DebugTextPanels.AddBlocksOfType(MM.TextPanel, "[DEBUG]"); 
            Debug("DEBUG Panel started.", false, "DEBUG PANEL"); 
        } 
 
        MMItems.Init(); 
    } 
 
    public static double GetPercent(double current, double max) 
    { 
        if (max > 0) 
            return (current / max) * 100; 
 
        return 100; 
    } 
 
    // Get inventory of block   
    public static IMyInventory GetBlockInventory(IMyTerminalBlock block, int invID = 0) 
    { 
        IMyInventoryOwner invOwner = (block as IMyInventoryOwner); 
        IMyInventory inv = null; 
 
        if (invOwner != null) 
            inv = invOwner.GetInventory(invID); 
 
        return inv; 
    } 
 
    public static List<double> GetDetailedInfoValues(IMyTerminalBlock block) 
    { 
        List<double> result = new List<double>(); 
 
        string di = block.DetailedInfo; 
        string[] attr_lines = block.DetailedInfo.Split('\n'); 
        string valstr = ""; 
 
        for (int i = 0; i < attr_lines.Length; i++) 
        { 
            string[] parts = attr_lines[i].Split(':'); 
            if (parts.Length < 2) 
            { 
                // broken line (try German) 
                parts = attr_lines[i].Split('r'); 
            } 
            if (parts.Length < 2) 
                valstr = parts[0]; 
            else 
                valstr = parts[1]; 
 
            string[] val_parts = valstr.Trim().Split(' '); 
            string str_val = val_parts[0]; 
            char str_unit = '.'; 
            if (val_parts.Length > 1) 
                str_unit = val_parts[1][0]; 
 
            double val = 0; 
            double final_val = 0; 
            if (Double.TryParse(str_val, out val)) 
            { 
                final_val = val * Math.Pow(1000.0, ".kMGTPEZY".IndexOf(str_unit)); 
                result.Add(final_val); 
            } 
        } 
 
        return result; 
    } 
 
    // Get laser antenna status 
    public static string GetLaserAntennaStatus(IMyLaserAntenna gear) 
    { 
        string[] info_lines = gear.DetailedInfo.Split('\n'); 
        string state = info_lines[info_lines.Length - 1].Split(' ')[0].ToUpper(); 
        return state; 
    } 
 
    // Get landing gear status 
    public static string GetLandingGearStatus(IMyLandingGear gear) 
    { 
        string unlockchars = "udoesnp"; 
 
        string[] info_lines = gear.DetailedInfo.Split('\n'); 
        string attr_line = info_lines[info_lines.Length - 1]; 
 
        string[] attr = attr_line.Split(':'); 
         
        string state = ""; 
        if (attr.Length < 2) 
            state = attr[0].Trim().ToLower(); 
        else 
            state = attr[1].Trim().ToLower(); 
 
        if (state == "") 
            return "UNLOCK"; 
 
        // hope it will be more words in other langs too 
        if (state.Split(' ').Length > 1) 
            return "READY"; 
 
        if ((unlockchars.IndexOf(state[0]) < 0) && !state.StartsWith("au")) 
            return "LOCK"; 
 
        return "UNLOCK"; 
    } 
 
    // return full type of item   
    public static string GetItemFullType(IMyInventoryItem item) 
    { 
        string typeid = item.Content.TypeId.ToString(); 
 
        typeid = typeid.Substring(typeid.LastIndexOf('_') + 1); 
 
        return item.Content.SubtypeName + " " + typeid; 
    } 
 
    // parse full type into subType and mainType   
    public static void ParseFullType(string fullType, out string subType, out string mainType) 
    { 
        string[] substr = fullType.Split(' '); 
 
        if (substr.Length == 2) 
        { 
            subType = substr[0]; 
            mainType = substr[1]; 
            return; 
        } 
 
        subType = fullType; 
        mainType = ""; 
    } 
 
    public static string TranslateToDisplay(string fullType) 
    { 
        string subType = ""; 
        string mainType = ""; 
        MM.ParseFullType(fullType, out subType, out mainType); 
 
        return TranslateToDisplay(subType, mainType); 
    } 
 
    public static string TranslateToDisplay(string subType, string mainType) 
    { 
        if ((mainType == "Ingot" && subType != "Stone") || mainType == "Ore") 
            return subType + " " + mainType; 
 
        MMList<MMItem> items = new MMList<MMItem>(); 
        if (MMItems.GetItemsOfType(items, subType, mainType)) 
        { 
            MMItem item = items[0]; 
            if (item.displayName != "") 
            { 
                return item.displayName; 
            } 
        } 
 
        return System.Text.RegularExpressions.Regex.Replace( 
            subType, "([a-z])([A-Z])", "$1 $2"); 
    } 
 
    public static void TranslateToInternal(ref string subType, ref string mainType) 
    { 
        string shortName = subType.ToLower(); 
        MMItem item = null; 
 
        if (MMItems.itemsByShortName.TryGetValue(shortName, out item)) { 
            subType = item.subType; 
            mainType = item.mainType; 
            return; 
        } 
 
        MMList<MMItem> matches = new MMList<MMItem>(); 
        if (MMItems.GetItemsOfType(matches, subType, mainType)) 
        { 
            subType = matches[0].subType; 
            if (matches.Count > 1) 
                return; 
            mainType = matches[0].mainType; 
        } 
    } 
 
    public const int Assembler = 0; 
    public const int BatteryBlock = Assembler + 1; 
    public const int Beacon = BatteryBlock + 1; 
    public const int ButtonPanel = Beacon + 1; 
    public const int CameraBlock = ButtonPanel + 1; 
    public const int CargoContainer = CameraBlock + 1; 
    public const int Cockpit = CargoContainer + 1; 
    public const int Collector = Cockpit + 1; 
    public const int ControlPanel = Collector + 1; 
    public const int Door = ControlPanel + 1; 
    public const int GravityGenerator = Door + 1; 
    public const int GravityGeneratorSphere = GravityGenerator + 1; 
    public const int Gyro = GravityGeneratorSphere + 1; 
    public const int InteriorLight = Gyro + 1; 
    public const int LandingGear = InteriorLight + 1; 
    public const int LargeGatlingTurret = LandingGear + 1; 
    public const int LargeInteriorTurret = LargeGatlingTurret + 1; 
    public const int LargeMissileTurret = LargeInteriorTurret + 1; 
    public const int LightingBlock = LargeMissileTurret + 1; 
    public const int LaserAntenna = LightingBlock + 1; 
    public const int MedicalRoom = LaserAntenna + 1; 
    public const int MotorStator = MedicalRoom + 1; 
    public const int OreDetector = MotorStator + 1; 
    public const int PistonBase = OreDetector + 1; 
    public const int ProgrammableBlock = PistonBase + 1; 
    public const int Projector = ProgrammableBlock + 1; 
    public const int RadioAntenna = Projector + 1; 
    public const int Reactor = RadioAntenna + 1; 
    public const int Refinery = Reactor + 1; 
    public const int ReflectorLight = Refinery + 1; 
    public const int RemoteControl = ReflectorLight + 1; 
    public const int SensorBlock = RemoteControl + 1; 
    public const int ShipConnector = SensorBlock + 1; 
    public const int ShipDrill = ShipConnector + 1; 
    public const int ShipGrinder = ShipDrill + 1; 
    public const int ShipMergeBlock = ShipGrinder + 1; 
    public const int ShipWelder = ShipMergeBlock + 1; 
    public const int SmallGatlingGun = ShipWelder + 1; 
    public const int SmallMissileLauncher = SmallGatlingGun + 1; 
    public const int SmallMissileLauncherReload = SmallMissileLauncher + 1; 
    public const int SolarPanel = SmallMissileLauncherReload + 1; 
    public const int SoundBlock = SolarPanel + 1; 
    public const int TextPanel = SoundBlock + 1; 
    public const int Thrust = TextPanel + 1; 
    public const int TimerBlock = Thrust + 1; 
    public const int VirtualMass = TimerBlock + 1; 
    public const int Warhead = VirtualMass + 1; 
    public const int FunctionalBlock = Warhead + 1; 
 
    public static string GetBlockTypeDisplayName(IMyTerminalBlock block) 
    { 
        return block.DefinitionDisplayNameText; 
    } 
 
    private static void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks) 
    { 
        _GridTerminalSystem.GetBlocksOfType<T>(blocks); 
    } 
 
    public static void GetBlocksOfType(ref List<IMyTerminalBlock> blocks, int type = int.MaxValue, string typeInStr = "") 
    { 
        typeInStr = typeInStr.ToLower(); 
 
        if (type == CargoContainer || typeInStr.StartsWith("carg") || typeInStr.StartsWith("conta")) 
            _GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(blocks); 
        else 
        if (type == TextPanel || typeInStr.StartsWith("text") || typeInStr.StartsWith("lcd")) 
            _GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(blocks); 
        else 
        if (type == Assembler || typeInStr.StartsWith("ass")) 
            _GridTerminalSystem.GetBlocksOfType<IMyAssembler>(blocks); 
        else 
        if (type == Refinery || typeInStr.StartsWith("refi")) 
            _GridTerminalSystem.GetBlocksOfType<IMyRefinery>(blocks); 
        else 
        if (type == Reactor || typeInStr.StartsWith("reac")) 
            _GridTerminalSystem.GetBlocksOfType<IMyReactor>(blocks); 
        else 
        if (type == SolarPanel || typeInStr.StartsWith("solar")) 
            _GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks); 
        else 
        if (type == BatteryBlock || typeInStr.StartsWith("bat")) 
            _GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(blocks); 
        else 
        if (type == Beacon || typeInStr.StartsWith("bea")) 
            _GridTerminalSystem.GetBlocksOfType<IMyBeacon>(blocks); 
        else 
        if (type == LaserAntenna || typeInStr == "laserantenna") 
            _GridTerminalSystem.GetBlocksOfType<IMyLaserAntenna>(blocks); 
        else 
        if (type == RadioAntenna || typeInStr.Contains("antenna")) 
            _GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(blocks); 
        else 
        if (type == Thrust || typeInStr.StartsWith("thrust")) 
            _GridTerminalSystem.GetBlocksOfType<IMyThrust>(blocks); 
        else 
        if (type == Gyro || typeInStr.StartsWith("gyro")) 
            _GridTerminalSystem.GetBlocksOfType<IMyGyro>(blocks); 
        else 
        if (type == SensorBlock || typeInStr.StartsWith("sensor")) 
            _GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(blocks); 
        else 
        if (type == ShipConnector || typeInStr.Contains("connector")) 
            _GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(blocks); 
        else 
        if (type == ReflectorLight || typeInStr.StartsWith("spot") || typeInStr.StartsWith("reflector")) 
            _GridTerminalSystem.GetBlocksOfType<IMyReflectorLight>(blocks); 
        else 
        if (type == InteriorLight || (typeInStr.StartsWith("inter") && typeInStr.EndsWith("light"))) 
            _GridTerminalSystem.GetBlocksOfType<IMyInteriorLight>(blocks); 
        else 
        if (type == LandingGear || typeInStr.StartsWith("land")) 
            _GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(blocks); 
        else 
        if (type == ProgrammableBlock || typeInStr.StartsWith("program")) 
            _GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(blocks); 
        else 
        if (type == TimerBlock || typeInStr.StartsWith("timer")) 
            _GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(blocks); 
        else 
        if (type == MotorStator || typeInStr == "rotor" || typeInStr.StartsWith("motor")) 
            _GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(blocks); 
        else 
        if (type == PistonBase || typeInStr.StartsWith("piston")) 
            _GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(blocks); 
        else 
        if (type == Projector || typeInStr.StartsWith("proj")) 
            _GridTerminalSystem.GetBlocksOfType<IMyProjector>(blocks); 
        else 
        if (type == ShipMergeBlock || typeInStr.Contains("merge")) 
            _GridTerminalSystem.GetBlocksOfType<IMyShipMergeBlock>(blocks); 
        else 
        if (type == SoundBlock || typeInStr.StartsWith("sound")) 
            _GridTerminalSystem.GetBlocksOfType<IMySoundBlock>(blocks); 
        else 
        if (type == Collector || typeInStr.StartsWith("col")) 
            _GridTerminalSystem.GetBlocksOfType<IMyCollector>(blocks); 
        else 
        if (type == Door || typeInStr == "door") 
            _GridTerminalSystem.GetBlocksOfType<IMyDoor>(blocks); 
        else 
        if (type == GravityGeneratorSphere || (typeInStr.Contains("grav") && typeInStr.Contains("sphe"))) 
            _GridTerminalSystem.GetBlocksOfType<IMyGravityGeneratorSphere>(blocks); 
        else 
        if (type == GravityGenerator || typeInStr.Contains("grav")) 
            _GridTerminalSystem.GetBlocksOfType<IMyGravityGenerator>(blocks); 
        else 
        if (type == ShipDrill || typeInStr.EndsWith("drill")) 
            _GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(blocks); 
        else 
        if (type == ShipGrinder || typeInStr.Contains("grind")) 
            _GridTerminalSystem.GetBlocksOfType<IMyShipGrinder>(blocks); 
        else 
        if (type == ShipWelder || typeInStr.EndsWith("welder")) 
            _GridTerminalSystem.GetBlocksOfType<IMyShipWelder>(blocks); 
        else 
        if (type == LargeGatlingTurret || (typeInStr.Contains("turret") && typeInStr.Contains("gatl"))) 
            _GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(blocks); 
        else 
        if (type == LargeInteriorTurret || (typeInStr.Contains("turret") && typeInStr.Contains("inter"))) 
            _GridTerminalSystem.GetBlocksOfType<IMyLargeInteriorTurret>(blocks); 
        else 
        if (type == LargeMissileTurret || (typeInStr.Contains("turret") && typeInStr.Contains("miss"))) 
            _GridTerminalSystem.GetBlocksOfType<IMyLargeMissileTurret>(blocks); 
        else 
        if (type == SmallGatlingGun || (typeInStr.Contains("gun") || typeInStr.Contains("gatl"))) 
            _GridTerminalSystem.GetBlocksOfType<IMySmallGatlingGun>(blocks); 
        else 
        if (type == SmallMissileLauncherReload || (typeInStr.Contains("launcher") && typeInStr.Contains("reload"))) 
            _GridTerminalSystem.GetBlocksOfType<IMySmallMissileLauncherReload>(blocks); 
        else 
        if (type == SmallMissileLauncher || (typeInStr.Contains("laucher"))) 
            _GridTerminalSystem.GetBlocksOfType<IMySmallMissileLauncher>(blocks); 
        else 
        if (type == VirtualMass || typeInStr.Contains("mass")) 
            _GridTerminalSystem.GetBlocksOfType<IMyVirtualMass>(blocks); 
        else 
        if (type == Warhead || typeInStr == "warhead") 
            _GridTerminalSystem.GetBlocksOfType<IMyWarhead>(blocks); 
        else 
        if (type == FunctionalBlock || typeInStr.StartsWith("func")) 
            _GridTerminalSystem.GetBlocksOfType<IMyFunctionalBlock>(blocks); 
        else 
        if (type == LightingBlock || typeInStr.StartsWith("light")) 
            _GridTerminalSystem.GetBlocksOfType<IMyLightingBlock>(blocks); 
        else 
        if (type == ControlPanel || typeInStr.StartsWith("contr")) 
            _GridTerminalSystem.GetBlocksOfType<IMyControlPanel>(blocks); 
        else 
        if (type == Cockpit || typeInStr.StartsWith("coc")) 
            _GridTerminalSystem.GetBlocksOfType<IMyCockpit>(blocks); 
        else 
        if (type == MedicalRoom || typeInStr.StartsWith("medi")) 
            _GridTerminalSystem.GetBlocksOfType<IMyMedicalRoom>(blocks); 
        else 
        if (type == RemoteControl || typeInStr.StartsWith("remote")) 
            _GridTerminalSystem.GetBlocksOfType<IMyRemoteControl>(blocks); 
        else 
        if (type == ButtonPanel || typeInStr.StartsWith("but")) 
            _GridTerminalSystem.GetBlocksOfType<IMyButtonPanel>(blocks); 
        else 
        if (type == CameraBlock || typeInStr.StartsWith("cam")) 
            _GridTerminalSystem.GetBlocksOfType<IMyCameraBlock>(blocks); 
        else 
        if (type == OreDetector || typeInStr.Contains("detect")) 
            _GridTerminalSystem.GetBlocksOfType<IMyOreDetector>(blocks); 
    } 
 
    public static string FormatLargeNumber(double number, bool compress = true) 
    { 
        if (!compress) 
            return number.ToString( 
                "#,###,###,###,###,###,###,###,###,###"); 
 
        string ordinals = " kMGTPEZY"; 
        double compressed = number; 
 
        var ordinal = 0; 
 
        while (compressed >= 1000) 
        { 
            compressed /= 1000; 
            ordinal++; 
        } 
 
        string res = Math.Round(compressed, 1, MidpointRounding.AwayFromZero).ToString(); 
 
        if (ordinal > 0) 
            res += " " + ordinals[ordinal]; 
 
        return res; 
    } 
 
    public static void WriteLine(IMyTextPanel textpanel, string message, bool append = true, string title = "") 
    { 
        textpanel.WritePublicText(message + "\n", append); 
        if (title != "") 
            textpanel.WritePublicTitle(title); 
        textpanel.ShowTextureOnScreen(); 
        textpanel.ShowPublicTextOnScreen(); 
    } 
 
    public static void Debug(string message, bool append = true, string title = "") 
    { 
        if (!EnableDebug) 
            return; 
        if (_DebugTextPanels == null || _DebugTextPanels.Count() == 0) 
            DebugAntenna(message, append, title); 
        else 
            DebugTextPanel(message, append, title); 
    } 
 
    public static void DebugAntenna(string message, bool append = true, string title = "") 
    { 
        List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>(); 
 
        _GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(blocks); 
        IMyRadioAntenna ant = blocks[0] as IMyRadioAntenna; 
        if (append) 
            ant.SetCustomName(ant.CustomName + message + "\n"); 
        else 
            ant.SetCustomName("PROG: " + message + "\n"); 
    } 
 
    public static void DebugTextPanel(string message, bool append = true, string title = "") 
    { 
        for (int i = 0; i < _DebugTextPanels.Count(); i++) 
        { 
            IMyTextPanel debugpanel = _DebugTextPanels.Blocks[i] as IMyTextPanel; 
            debugpanel.SetCustomName("[DEBUG] Prog: " + message); 
            WriteLine(debugpanel, message, append, title); 
        } 
    } 
} 
 
public class MMPanel 
{ 
    public MMDict<string, IMyTextPanel> panels = new MMDict<string, IMyTextPanel>(); 
    public MMLCDTextManager.MMLCDText text = null; 
 
    public IMyTextPanel GetFirst() 
    { 
        if (panels.CountAll() <= 0) 
            return null; 
 
        return panels.GetItemAt(0); 
    } 
 
    public string GetPosition() { 
        return panels.GetItemAt(0).GetPosition().ToString("F0"); 
    } 
 
    public string GetPublicTitle() 
    { 
        if (panels.CountAll() <= 0) 
            return ""; 
 
        return panels.GetItemAt(0).GetPublicTitle(); 
    } 
 
    public string GetCustomName() 
    { 
        if (panels.CountAll() <= 0) 
            return ""; 
 
        return panels.GetItemAt(0).CustomName; 
    } 
 
    public float GetFontSize() 
    { 
        if (panels.CountAll() <= 0) 
            return 0.8f; 
 
        return panels.GetItemAt(0).GetValueFloat("FontSize"); 
    } 
 
    public void SetFontSize(float size) 
    { 
        for (int i = 0; i < panels.CountAll(); i++) 
            panels.GetItemAt(i).SetValueFloat("FontSize", size); 
    } 
 
    public void SortPanels() 
    { 
        panels.SortAll(); 
    } 
 
    public bool IsWide() 
    { 
        if (panels.CountAll() <= 0) 
            return false; 
 
        if (panels.GetItemAt(0).DefinitionDisplayNameText.Contains("Wide")) 
            return true; 
 
        return false; 
    } 
 
    public void Update() 
    { 
        if (text == null) 
            return; 
 
        int cnt = panels.CountAll(); 
 
        if (cnt > 1) 
            SetFontSize(GetFontSize()); 
 
        for (int i = 0; i < panels.CountAll(); i++) 
        { 
            panels.GetItemAt(i).WritePublicText(text.GetDisplayString(i)); 
            panels.GetItemAt(i).ShowTextureOnScreen(); 
            panels.GetItemAt(i).ShowPublicTextOnScreen(); 
        } 
    } 
 
} 
 
public static class MMLCDTextManager 
{ 
    private static Dictionary<IMyTextPanel, MMLCDText> panelTexts = new Dictionary<IMyTextPanel, MMLCDText>(); 
    public static float widthMod = 1.0f; 
 
    public static MMLCDText GetLCDText(MMPanel p) 
    { 
        MMLCDText lcdText = null; 
        IMyTextPanel panel = p.GetFirst(); 
 
        if (!panelTexts.TryGetValue(panel, out lcdText)) 
        { 
            lcdText = new MMLCDText(); 
            p.text = lcdText; 
            panelTexts.Add(panel, lcdText); 
        } 
 
        p.text = lcdText; 
        return lcdText; 
    } 
 
    public static void AddLine(MMPanel panel, string line) 
    { 
        MMLCDText lcd = GetLCDText(panel); 
        lcd.AddLine(line); 
    } 
 
    public static void Add(MMPanel panel, string text) 
    { 
        MMLCDText lcd = GetLCDText(panel); 
 
        lcd.AddFast(text); 
        lcd.current_width += MMStringFunc.GetStringSize(text); 
    } 
 
    public static void AddRightAlign(MMPanel panel, string text, float end_screen_x) 
    { 
        MMLCDText lcd = GetLCDText(panel); 
         
        float text_width = MMStringFunc.GetStringSize(text); 
        end_screen_x *= widthMod; 
        end_screen_x -= lcd.current_width; 
 
        if (end_screen_x < text_width) 
        { 
            lcd.AddFast(text); 
            lcd.current_width += text_width; 
            return; 
        } 
 
        end_screen_x -= text_width; 
        int fillchars = (int)Math.Round(end_screen_x / MMStringFunc.WHITESPACE_WIDTH, MidpointRounding.AwayFromZero); 
        float fill_width = fillchars * MMStringFunc.WHITESPACE_WIDTH; 
 
        string filler = new String(' ', fillchars); 
        lcd.AddFast(filler + text); 
        lcd.current_width += fill_width + text_width; 
    } 
 
    public static void AddCenter(MMPanel panel, string text, float screen_x) 
    { 
        MMLCDText lcd = GetLCDText(panel); 
        float text_width = MMStringFunc.GetStringSize(text); 
        screen_x *= widthMod; 
        screen_x -= lcd.current_width; 
 
        if (screen_x < text_width / 2) 
        { 
            lcd.AddFast(text); 
            lcd.current_width += text_width; 
            return; 
        } 
 
        screen_x -= text_width / 2; 
        int fillchars = (int)Math.Round(screen_x / MMStringFunc.WHITESPACE_WIDTH, MidpointRounding.AwayFromZero); 
        float fill_width = fillchars * MMStringFunc.WHITESPACE_WIDTH; 
 
        string filler = new String(' ', fillchars); 
        lcd.AddFast(filler + text); 
        lcd.current_width += fill_width + text_width; 
    } 
 
    public static void AddProgressBar(MMPanel panel, double percent, int width = 22) 
    { 
        MMLCDText lcd = GetLCDText(panel); 
        int totalBars = width - 2; 
        int fill = (int)(percent * totalBars) / 100; 
        if (fill > totalBars) 
            fill = totalBars; 
        string progress = "[" + new String('|', fill) + new String('\'', totalBars - fill) + "]"; 
 
        lcd.AddFast(progress); 
        lcd.current_width += MMStringFunc.PROGRESSCHAR_WIDTH * width; 
    } 
 
    public static void ClearText(MMPanel panel) 
    { 
        GetLCDText(panel).ClearText(); 
    } 
 
    public static void UpdatePanel(MMPanel panel) 
    { 
        panel.Update(); 
        GetLCDText(panel).ScrollNextLine(); 
    } 
 
    public class MMLCDText 
    { 
        public float fontSize = 0.8f; 
        public int scrollPosition = 0; 
        public int scrollDirection = 1; 
        public int DisplayLines = 22; // 22 for font size 0.8 
        public int screens = 1; 
 
        public List<string> lines = new List<string>(); 
        public int current_line = 0; 
        public float current_width = 0; 
 
        public MMLCDText(float _fontSize = 0.8f) 
        { 
            SetTextFontSize(_fontSize); 
        } 
 
        public void SetTextFontSize(float _fontSize) 
        { 
            fontSize = _fontSize; 
            DisplayLines = (int)Math.Round(22 * (0.8 / fontSize) * screens); 
        } 
 
        public void SetNumberOfScreens(int _screens) 
        { 
            screens = _screens; 
            DisplayLines = (int)Math.Round(22 * (0.8 / fontSize) * screens); 
        } 
 
        public void CheckCurLine() 
        { 
            if (current_line >= lines.Count) 
                lines.Add(""); 
        } 
 
        public void AddFast(string text) 
        { 
            CheckCurLine(); 
            lines[current_line] += text; 
        } 
 
        public void AddLine(string line) 
        { 
            AddFast(line); 
            current_line++; 
            current_width = 0; 
        } 
 
        public void ClearText() 
        { 
            lines.Clear(); 
            current_width = 0; 
            current_line = 0; 
        } 
 
        public string GetFullString() 
        { 
            return String.Join("\n", lines); 
        } 
 
        // Display only X lines from scrollPos 
        public string GetDisplayString(int screenidx = 0) 
        { 
            MM.Debug("DisplayString: " + DisplayLines.ToString() + " / " + screens.ToString()); 
            if (lines.Count < DisplayLines / screens) 
            { 
                if (screenidx == 0) 
                { 
                    scrollPosition = 0; 
                    scrollDirection = 1; 
                    MM.Debug("DisplayString: full"); 
                    return GetFullString(); 
                } 
                return ""; 
            } 
 
             
            int scrollPos = scrollPosition + screenidx * (DisplayLines / screens); 
            MM.Debug("DisplayStringPos: " + scrollPos + " / " + Math.Min(lines.Count - scrollPos, DisplayLines / screens).ToString()); 
 
            List<string> display = 
                lines.GetRange(scrollPos, 
                    Math.Min(lines.Count - scrollPos, DisplayLines / screens)); 
 
            return String.Join("\n", display); 
        } 
 
        public void ScrollNextLine() 
        { 
            int lines_cnt = lines.Count; 
            if (lines_cnt < DisplayLines) 
            { 
                scrollPosition = 0; 
                scrollDirection = 1; 
                return; 
            } 
 
            if (scrollDirection > 0) 
            { 
                if (scrollPosition + LCDsProgram.SCROLL_LINES + DisplayLines > lines_cnt) 
                { 
                    scrollDirection = -1; 
                    scrollPosition = lines_cnt - DisplayLines; 
                    return; 
                } 
 
                scrollPosition += LCDsProgram.SCROLL_LINES; 
            } 
            else 
            { 
                if (scrollPosition - LCDsProgram.SCROLL_LINES < 0) 
                { 
                    scrollPosition = 0; 
                    scrollDirection = 1; 
                    return; 
                } 
 
                scrollPosition -= LCDsProgram.SCROLL_LINES; 
            } 
        } 
    } 
} 
 
public static class MMStringFunc 
{ 
    private static Dictionary<char, float> charSize = new Dictionary<char, float>(); 
 
    public const float WHITESPACE_WIDTH = 8f; 
    public const float PROGRESSCHAR_WIDTH = 6f; 
 
    public static void InitCharSizes() 
    { 
        if (charSize.Count > 0) 
            return; 
 
        AddCharsSize("3FKTabdeghknopqsuy", 17f); 
        AddCharsSize("#0245689CXZ", 19f); 
        AddCharsSize("$&GHPUVY", 20f); 
        AddCharsSize("ABDNOQRS", 21f); 
        AddCharsSize("(),.1:;[]ft{}", 9f); 
        AddCharsSize("+<=>E^~", 18f); 
        AddCharsSize(" !I`ijl", 8f); 
        AddCharsSize("7?Jcz", 16f); 
        AddCharsSize("L_vx", 15f); 
        AddCharsSize("\"-r", 10f); 
        AddCharsSize("mw", 27f); 
        AddCharsSize("M", 26f); 
        AddCharsSize("W", 31f); 
        AddCharsSize("'|", 6f); 
        AddCharsSize("*", 11f); 
        AddCharsSize("\\", 12f); 
        AddCharsSize("/", 14f); 
        AddCharsSize("%", 24f); 
        AddCharsSize("@", 25f); 
        AddCharsSize("\n", 0f); 
    } 
 
    private static void AddCharsSize(string chars, float size) 
    { 
        for (int i = 0; i < chars.Length; i++) 
            charSize.Add(chars[i], size); 
    } 
 
    public static float GetCharSize(char c) 
    { 
        float width = 17f; 
        charSize.TryGetValue(c, out width); 
 
        return width; 
    } 
 
    public static float GetStringSize(string str) 
    { 
        float sum = 0; 
        for (int i = 0; i < str.Length; i++) 
            sum += GetCharSize(str[i]); 
 
        return sum; 
    } 
 
    public static string GetStringTrimmed(string text, float pixel_width) 
    { 
        pixel_width *= MMLCDTextManager.widthMod; 
        int trimlen = Math.Min((int)pixel_width / 14, text.Length-2); 
        float stringSize = GetStringSize(text); 
        if (stringSize <= pixel_width) 
            return text; 
 
        while (stringSize > pixel_width - 20) 
        { 
            text = text.Substring(0, trimlen); 
            stringSize = GetStringSize(text); 
            trimlen -= 2; 
        } 
        return text + ".."; 
    } 
} 
 
public class MMItem 
{ 
    public string subType = ""; 
    public string mainType = ""; 
 
    public double mass = 0; 
    public double volume = 0; 
 
    public int defaultQuota = 0; 
    public string displayName = ""; 
    public string shortName = ""; 
 
    public MMItem(string _subType, string _mainType, double _mass, double _volume,  
        int _defaultQuota = 0, string _displayName = "", string _shortName = "") 
    { 
        subType = _subType; 
        mainType = _mainType; 
        mass = _mass; 
        volume = _volume; 
        defaultQuota = _defaultQuota; 
        displayName = _displayName; 
        shortName = _shortName; 
    } 
} 
// Dictionary helper 
public class MMDict<TKey, TValue> 
{ 
    public Dictionary<TKey, TValue> dict; 
    public List<TKey> keys; 
 
    public MMDict(int size = 10) 
    { 
        dict = new Dictionary<TKey, TValue>(size); 
        keys = new List<TKey>(size); 
    } 
 
    public void AddItem(TKey key, TValue item) 
    { 
        if (!dict.ContainsKey(key)) 
            keys.Add(key); 
        dict.Add(key, item); 
    } 
 
    public void RemoveKey(TKey key) 
    { 
        keys.Remove(key); 
        dict.Remove(key); 
    } 
 
    public TValue GetItem(TKey key) 
    { 
        if (dict.ContainsKey(key)) 
            return dict[key]; 
        else 
            return default(TValue); 
    } 
 
    public TValue GetItemAt(int index) 
    { 
        return dict[keys[index]]; 
    } 
 
    public int CountAll() 
    { 
        return dict.Count; 
    } 
 
    public void ClearAll() 
    { 
        keys.Clear(); 
        dict.Clear(); 
    } 
 
    public void SortAll() 
    { 
        keys.Sort(); 
    } 
} 
// List implementation using dictionary to allow List with custom class 
public class MMList<T> 
{ 
    private Dictionary<int, T> _dictionary; 
    private List<int> _keys; 
 
    public MMList(int size = 20) 
    { 
        _dictionary = new Dictionary<int, T>(size); 
        _keys = new List<int>(size); 
    } 
 
    public void RemoveAt(int index) 
    { 
        _dictionary.Remove(_keys[index]); 
        _keys.RemoveAt(index); 
    } 
 
    public T this[int index] 
    { 
        get { return _dictionary[_keys[index]]; } 
        set { _dictionary[_keys[index]] = value; } 
    } 
 
    public void Add(T item) 
    { 
        int index = _keys.Count == 0 ? 0 : _keys[_keys.Count - 1] + 1; 
        _dictionary.Add(index, item); 
        _keys.Add(index); 
    } 
 
    public void ClearItems() 
    { 
        _dictionary.Clear(); 
        _keys.Clear(); 
    } 
 
    public int Count { get { return _dictionary.Count; } } 
 
    public bool Remove(T item) 
    { 
        for (int i = 0; i < _keys.Count; i++) 
        { 
            if (_dictionary[_keys[i]].Equals(item)) 
            { 
                _dictionary.Remove(_keys[i]); 
                _keys.RemoveAt(i); 
                return true; 
            } 
        } 
        return false; 
    } 