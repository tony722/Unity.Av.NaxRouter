using System;
using System.Collections.Generic;

namespace AET.Unity.Av.NaxRouter {
  public class NaxZone {
    #region Constructors
    public NaxZone() { }
    public NaxZone(int zoneNumber) {
      ZoneNumber = zoneNumber;
    }

    public NaxZone(int zoneNumber, int switcherOutputNumber) : this(zoneNumber) {
      SwitcherOutputNumber = switcherOutputNumber;
    }
    #endregion

    #region Properties
 public int ZoneNumber { get; private set; }
    public int SwitcherOutputNumber { get; private set; }
    public NaxSource CurrentSource { get; private set; }
    public bool CurrentSourceIsStreamed { get; private set; }
    #endregion

    public void Route(NaxSource source) {
      if(CurrentSource != null && CurrentSourceIsStreamed) CurrentSource.Disconnect(this);
      CurrentSource = source;
      if (RouteLocally(source, this)) {
        CurrentSourceIsStreamed = false;
        MulticastAddressChanged(this, new MulticastAddressEventArgs(Router.NoSignalMulticastAddress, source.Port));
      } else {
        CurrentSourceIsStreamed = true;
        source.Connect(this);
        MulticastAddressChanged(this, new MulticastAddressEventArgs(source.MulticastAddress, source.Port));
      }
    }

    #region Delegates
    private RoutesLocallyDelegate routesLocally;
    public RoutesLocallyDelegate RouteLocally { get { return routesLocally ?? (RouteLocally = (_, __)  => true); } set { routesLocally = value; } }

    public delegate bool RoutesLocallyDelegate(NaxSource source, NaxZone zone);
    #endregion

    #region Events
    public event EventHandler<MulticastAddressEventArgs> MulticastAddressChanged = delegate { };
    #endregion
  }
}
