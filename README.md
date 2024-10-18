# Unity.Av.NaxRouter (Beta)
## Part of the open source Unity.Framework for Crestron
### Version 1.0.0
Make your life super easy when working with the NAX Switches, like the creatively named DM-NAX-8ZSA! Drop a switcher module on each NAX and use the single Router module to route sources to zones. 

Automatically handles starting and stopping streaming, local vs streamed sources, and everything. Just send an analog value of which source you want to route to which zone and everything magically routes!


## To Use In SIMPL Windows:
Download the latest release: [AET Unity AV NaxRouter v1.0.0](https://github.com/tony722/Unity.Relays/releases/download/v1.0.5/AET.Unity.Relays.v1.0.5.zip)

* Add a single Unity Av Nax Router.umc to your program.
* Add one Unity Av Nax-8zsa.umc module for each Nax-8zsa, Nax-4zsa, Nax-4zsp in your program. (Supports import of device logic on the Nax-8zsa making programming super easy!*)
* Configure the start zone and start source parameters: 
  * The first Nax will be Zone 1, Source 1. 
  * The second Nax will be Zone 9, Source 17.
  * The third Nax will be Zone 17, Source 33, etc.

\* *Waiting on Crestron to add support for device-import on the 4zsa, and other NAX switchers.*

## License: Apache License 2.0 - Commercial Use Freely Permitted.
 Please freely use this library in any Crestron application, including for-profit Crestron SIMPL Windows programs that you charge money to sell, and in SIMPL# libraries of your own, including ones you sell.

 If you modify the code in this library and feel those changes will enhance the usability of this library to other Crestron programmers, it would be greatly appreciated if you would branch this repository, and issue a pull request back to this repository. Thank you!
