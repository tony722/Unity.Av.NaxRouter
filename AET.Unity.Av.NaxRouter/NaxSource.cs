using System;
using System.Collections.Generic;
using System.Diagnostics;
using AET.Unity.SimplSharp;

namespace AET.Unity.Av.NaxRouter {
  public class NaxSource {
    private readonly List<NaxZone> routedZones = new List<NaxZone>();

    #region Constructors
    public NaxSource() { }

    public NaxSource(int sourceNumber) : this() {
      SourceNumber = sourceNumber;
    }

    public NaxSource(int sourceNumber, ushort switcherInputNumber) : this(sourceNumber) { SwitcherInputNumber = switcherInputNumber; }

    #endregion 
    
    #region Properties
    public int Port { get { return 10000 + SourceNumber; } }

    public string MulticastAddress { get { return SourceNumber == 0 ? "0.0.0.0" : string.Format("{0}.{1}.{2}", Router.MulticastBaseAddress, (int)(SourceNumber / 100), SourceNumber % 100); } 
    }

    public IList<NaxZone> RoutedZones { get { return routedZones.AsReadOnly(); } }
    public int SourceNumber { get; private set; }
    public int SwitcherInputNumber { get; private set; }
    #endregion

    public void Connect(NaxZone zone) {
      if (!routedZones.Contains(zone)) routedZones.Add(zone);
      if(routedZones.Count == 0) MulticastAddressChanged(this, new MulticastAddressEventArgs(Router.NoSignalMulticastAddress, Port));
      else if (routedZones.Count == 1) MulticastAddressChanged(this, new MulticastAddressEventArgs(MulticastAddress, Port));
    }

    public void Disconnect(NaxZone zone) {
      if (routedZones.Contains(zone).WarnIfNot("NaxSource #{0} tried to disconnect from zone #{1}, but it was not currently connected", SourceNumber, zone.ZoneNumber)) {
        routedZones.Remove(zone);
      }
      if (routedZones.Count == 0 && MulticastAddressChanged != null) MulticastAddressChanged(this, new MulticastAddressEventArgs(Router.NoSignalMulticastAddress, Port));
    }
    #region Events
    public event EventHandler<MulticastAddressEventArgs> MulticastAddressChanged = delegate { };
    #endregion  

  }
}
