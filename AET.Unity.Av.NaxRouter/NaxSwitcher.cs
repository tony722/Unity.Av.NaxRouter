using System;
using AET.Unity.SimplSharp;
using Crestron.SimplSharp.CrestronIO;

namespace AET.Unity.Av.NaxRouter {
  public class NaxSwitcher {
    private bool initialized = false;
    public NaxSwitcher() {
      CreatePlaceholderDelegates();
      Router.Initialized += RouterOnInitialized;
    }

    #region Properties
    /// <summary>
    /// Gets or sets the offset for the zone number in the master zone list. 
    /// </summary>
    /// <remarks>
    /// Each <see cref="NaxSwitcher"/> has one or more zones (e.g., 1-8). In the master zone list in the <see cref="Router"/>,
    /// each zone has a unique number. The <see cref="ZoneNumberOffset"/> is used to determine the starting number of the zones
    /// for this switcher in the master list. For example, the first switch might have an offset of 0, so its 8 zones would be
    /// numbered 1-8. The second switch might have an offset of 8, so its zones would be numbered 9-16 in the master list.
    ///
    /// Note: If two zones have the same number, the last zone to be registered will overwrite the first zone.
    /// </remarks>
    public ushort ZoneNumberOffset { get; set; }

    /// <summary>
    /// Gets or sets the offset for the source number in the master source list.
    /// </summary>
    /// <remarks>
    /// Each <see cref="NaxSwitcher"/> has one or more sources (e.g., 1-16). In the master source list in the <see cref="Router"/>,
    /// each source has a unique number. The <see cref="SourceNumberOffset"/> is used to determine the starting number of the sources
    /// for this switcher in the master list. For example, the first switch might have an offset of 0, so its 16 sources would be
    /// numbered 1-16. The second switch might have an offset of 16, so its sources would be numbered 17-32 in the master list.
    ///
    /// Note: If two sources have the same number, the last source to be registered will overwrite the first source.
    /// </remarks>
    public ushort SourceNumberOffset { get; set; }

    /// <summary>
    /// Gets or sets the number of zones the physical switch has.
    /// </summary>
    public ushort NumberOfZones { get; set; }

    /// <summary>
    /// Gets or sets the number of sources the physical switch has.
    /// </summary>
    public ushort NumberOfSources { get; set; }

    /// <summary>
    /// Gets or sets the number of the input on the switcher that is used for streamed sources.
    /// </summary>
    public ushort StreamingInputNumber { get; set; }
    #endregion

    #region Initialization

    private void CreatePlaceholderDelegates() {
      SetRouting = delegate { };
      SetZonePort = delegate { };
      SetZoneMulticastAddress = delegate { };

      SetSourcePort = delegate { };
      SetSourceMulticastAddress = delegate { };
    }
    private void RouterOnInitialized(object sender, EventArgs e) {
      if(initialized) return;
      initialized = true;
      RegisterZones();
      RegisterSources();
    }

    private void RegisterZones() {
      for (var i = 1; i <= NumberOfZones; i++) {
        var zoneNumber = i + (ZoneNumberOffset);
        var output = (ushort)i;
        var zone = new NaxZone(zoneNumber, output) { RouteLocally = RoutedLocally };
        zone.MulticastAddressChanged += (_, e) => {
          SetZoneMulticastAddress(output, e.MulticastAddress);
          SetZonePort(output, (ushort)e.Port);
        };
        Router.RegisterZone(zoneNumber, zone);
      }
    }

    private void RegisterSources() {
      for (var i = 1; i <= NumberOfSources; i++) {
        var sourceNumber = i + (SourceNumberOffset);
        var input = (ushort)i;
        var source = new NaxSource(sourceNumber, input);
        source.MulticastAddressChanged += (_, e) => {
          SetSourceMulticastAddress(input, e.MulticastAddress);
          SetSourcePort(input, (ushort)e.Port);
        };
        Router.RegisterSource(source);
      }
    }
    #endregion

    #region Routing
    private bool RoutedLocally(NaxSource source, NaxZone zone) {
      if (IsLocalSource(source)) {
        SetRouting(zone.SwitcherOutputNumber, source.SwitcherInputNumber);
        return true;
      }
      SetRouting(zone.SwitcherOutputNumber, StreamingInputNumber);
      return false;
    }

    private bool IsLocalSource(NaxSource source) {
      return source.SourceNumber > SourceNumberOffset && source.SourceNumber <= (SourceNumberOffset + NumberOfSources);
    }
    #endregion

    #region Delegates
    public SetRoutingDelegate SetRouting { get; set; }
    public SetPortDelegate SetZonePort { get; set; }
    public SetMulticastAddressDelegate SetZoneMulticastAddress { get; set; }

    public SetPortDelegate SetSourcePort { get; set; }
    public SetMulticastAddressDelegate SetSourceMulticastAddress { get; set; }

    public delegate void SetRoutingDelegate(int zoneNumber, int sourceNumber);
    public delegate void SetNameDelegate(int index, string name);
    public delegate void SetPortDelegate(int index, ushort port);
    public delegate void SetMulticastAddressDelegate(int index, string multicastAddress);
  }

  #endregion

}
