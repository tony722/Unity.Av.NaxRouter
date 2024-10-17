using System;

namespace AET.Unity.Av.NaxRouter {
  public class NaxSourceEventArgs : EventArgs {
    public NaxSourceEventArgs(NaxSource source) {
      Source = source;
    }
    public NaxSource Source { get; private set; }
  }

  public class NaxZoneEventArgs : EventArgs {
    public NaxZoneEventArgs(NaxZone zone) {
      Zone = zone;
    }
    public NaxZone Zone { get; private set; }
  }

  public class MulticastAddressEventArgs : EventArgs {
    public MulticastAddressEventArgs(string multicastAddress, int port) {
      MulticastAddress = multicastAddress;
      Port = port;
    }
    public string MulticastAddress { get; private set; }
    public int Port { get; private set; }
  }

  public class ZoneRouteEventArgs : EventArgs {
    public ZoneRouteEventArgs(NaxZone zone, NaxSource source) {
      Zone = zone;
      Source = source;
    }
    public NaxZone Zone { get; set; }
    public NaxSource Source { get; set; }
  }

  public class IntArrayEventArgs : EventArgs {
    public IntArrayEventArgs(int index, int value) {
      Index = index;
      Value = value;
    }
    public int Index { get; set; }
    public int Value { get; set; }
  }

  public class StringArrayEventArgs : EventArgs {
    public StringArrayEventArgs(int index, string value) {
      Index = index;
      Value = value;
    }
    public int Index { get; set; }
    public string Value { get; set; }
  }
}