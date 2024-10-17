using System;
using AET.Unity.Av.NaxRouter;
using AET.Unity.SimplSharp;


namespace Unity.Av.NaxRouter.Tests {
  internal class TestNax8zsaSwitcher {
    private readonly NaxSwitcher nax = new NaxSwitcher { NumberOfSources = 16, NumberOfZones = 8, StreamingInputNumber = 17};

    public void RouteSourceToZone(int sourceNumber, int zoneOutputNumber) {
      var source = Router.Sources[sourceNumber];
      var zone = Router.Zones[ZoneNumberFromOutputNumber(zoneOutputNumber)];
      zone.Route(source);
    }

    private int ZoneNumberFromOutputNumber(int zoneOutputNumber) {
      return zoneOutputNumber + nax.ZoneNumberOffset;
    }

    public TestNax8zsaSwitcher(ushort zoneOffset, ushort sourceOffset) {
      nax.ZoneNumberOffset = zoneOffset;
      nax.SourceNumberOffset = sourceOffset;
      nax.SetRouting = (zoneNumber, sourceNumber) => Routes[zoneNumber] = sourceNumber;
      nax.SetZonePort = (index, port) => ZonePorts[index] = port;
      nax.SetZoneMulticastAddress = (index, address) => ZoneMulticastAddresses[index] = address;
      nax.SetSourcePort = (index, port) => SourcePorts[index] = port;
      nax.SetSourceMulticastAddress = (index, address) => SourceMulticastAddresses[index] = address;
    }

    public AnyKeyDictionary<int, int> Routes { get; } = new AnyKeyDictionary<int, int>();
    public AnyKeyDictionary<int, int> ZonePorts { get; } = new AnyKeyDictionary<int, int>();
    public AnyKeyDictionary<int, string> ZoneMulticastAddresses { get; } = new AnyKeyDictionary<int, string>();
    public AnyKeyDictionary<int, int> SourcePorts { get; } = new AnyKeyDictionary<int, int>();
    public AnyKeyDictionary<int, string> SourceMulticastAddresses { get; } = new AnyKeyDictionary<int, string>();
  }
}
