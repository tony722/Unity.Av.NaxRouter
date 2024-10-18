# Unity.Av.NaxRouter (Beta)	
## Part of the open source Unity.Framework for Crestron
### Version 1.0.0
Make your life _super easy_ when working with the NAX Switches like the DM-NAX-8ZSA! Drop a switcher module on each NAX and use the single Unity NAX Router module to route any source to any zone without having to worry about switching between analog router signals, starting and stopping multicast streams, or figuring out multicast addresses. 

Automatically handles starting and stopping streaming, local vs streamed sources, and everything. Just send an analog value of which source you want to route to which zone and everything magically routes!


## To Use In SIMPL Windows:
Download the latest release: [AET Unity AV NaxRouter v1.0.0](https://github.com/tony722/Unity.Av.NaxRouter/releases/download/v1.0.0/Unity.Av.NaxRouter.Demo.1.0.0_compiled.zip)

* Add a single Unity Av Nax Router.umc to your program.
   ![image](https://github.com/tony722/Unity.Av.NaxRouter/blob/master/Documentation/NAX%20Router%20module.png?raw=true)
* Add one Unity Av Nax-****  module for each NAX Switcher in your program (NAX-8ZSA, NAX-4ZSA, or NAX-4ZSP). The NAX-8ZSA supports import of device logic in SIMPL Windows Nax-8zsa making programming super easy!*

   ![image](https://github.com/tony722/Unity.Av.NaxRouter/blob/master/Documentation/NAX%204ZSA%20module.png?raw=true)
* For the NAX-$ZSA/4ZSP be sure to put a 1 on AutomaticInitializationEnabled on each Source in NAX > TX and each Zone in NAX > RX.
* Configure the start zone and start source parameters. For example, 3x DM-NAX-8ZSAs would be as follows:
  * The first Nax will be Zone 1, Source 1. 
  * The second Nax will be Zone 9, Source 17.
  * The third Nax will be Zone 17, Source 33.
  
* See the demo program for examples. Note, if it makes more sense to you not to use sequential source (or zone) numbers, that is not a problem.

\* *Waiting on Crestron to add support for device-import on the 4ZSA, and other NAX switchers.*



## License: Apache License 2.0 - Commercial Use Freely Permitted.
 Please freely use this library in any Crestron application, including for-profit Crestron SIMPL Windows programs that you charge money to sell, and in SIMPL# libraries of your own, including ones you sell. _Please contact me through GitHub if you use it and like it—that will help motivate me to keep releasing modules like these._

 If you modify the code in this library and feel those changes will enhance the usability of this library to other Crestron programmers, it would be greatly appreciated if you would branch this repository, and issue a pull request back to this repository. Thank you!
