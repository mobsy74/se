### SE Script Code

#### Setting up Visual Studio with IntelliSense

http://forum.keenswh.com/threads/guide-setting-up-visual-studio-for-programmable-block-scripting.7225319/

* Download and install VS Community edition
* Start new project
* Select Class Library type
* Select directory use for repo
* Inside VS, on the Solution Explorer window, right click References and click Add Reference
* Click Browse
* Browse to C:\Program Files (x86)\Steam\steamapps\common\SpaceEngineers\Bin
* Add AT LEAST the following DLLs:

```
Sandbox.Common.dll
Sandbox.Game.dll
VRage.dll
VRage.Game.dll
VRage.Math.dll
VRage.Library.dll
```


* Create new class
* Use the following template for each class, changing the namespace for each
* Paste script code inside the CodeEditor region

```
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;

namespace SEScripts_SolarAlignment
{
    public class ExampleScipt : MyGridProgram
    {
        #region CodeEditor
        
                #endregion
    }
}
```

#### API Reference:

https://steamcommunity.com/sharedfiles/filedetails/?id=360966557
