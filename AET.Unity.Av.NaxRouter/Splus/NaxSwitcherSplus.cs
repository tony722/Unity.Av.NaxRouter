using AET.Unity.SimplSharp;

namespace AET.Unity.Av.NaxRouter.Splus {
  public class NaxSwitcherSplus {
    private readonly NaxSwitcher nax = new NaxSwitcher();
    private bool initialized;
    
    #region Constructors
    public NaxSwitcherSplus() {
      if (initialized) return;
      initialized = true;
      Router.Initialized += (_, __) => TriggerInit();
      AddPlaceholderDelegates();
      HookUpNaxDelegates();
    }
    #endregion


    public ushort NumberOfSources { set { nax.NumberOfSources = value;  } }
    public ushort NumberOfZones { set { nax.NumberOfZones = value;  } }
    public ushort StreamingInputNumber { set { nax.StreamingInputNumber = value; } }
    public ushort ZoneNumberOffset { set { nax.ZoneNumberOffset = value; } }
    public ushort SourceNumberOffset { set { nax.SourceNumberOffset = value; } }


    private void AddPlaceholderDelegates() {
      SetRouting = delegate { };
      SetRxZonePort =  delegate { };
      SetRxZoneMulticastAddress =  delegate { };
      SetTxSourcePort =  delegate { };
      SetTxSourceMulticastAddress =  delegate { };
    }
    private void HookUpNaxDelegates() {
      nax.SetRouting = (zoneNumber, sourceNumber) => SetRouting((ushort)zoneNumber, (ushort)sourceNumber);
      nax.SetZonePort = (index, port) => SetRxZonePort((ushort)index, port);
      nax.SetZoneMulticastAddress = (index, address) => SetRxZoneMulticastAddress((ushort)index, address);
      nax.SetSourcePort = (index, port) => SetTxSourcePort((ushort)index, port);
      nax.SetSourceMulticastAddress = (index, address) => SetTxSourceMulticastAddress((ushort)index, address);
    }


    #region Simpl+ Outputs
    public TriggerDelegate TriggerInit { get; set; }
    public SetUshortOutputArrayDelegate SetRouting { get; set; }
    public SetUshortOutputArrayDelegate SetRxZonePort { get; set; }
    public SetStringOutputArrayDelegate SetRxZoneMulticastAddress { get; set; }
    public SetUshortOutputArrayDelegate SetTxSourcePort { get; set; }
    public SetStringOutputArrayDelegate SetTxSourceMulticastAddress { get; set; }
    #endregion
  }
}
