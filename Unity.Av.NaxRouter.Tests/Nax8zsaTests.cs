using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using AET.Unity.Av.NaxRouter;
using AET.Unity.Av.NaxRouter.Splus;
using AET.Unity.SimplSharp;
using FluentAssertions;

namespace Unity.Av.NaxRouter.Tests {
  [TestClass]
  public class Nax8zsaTests {
    private readonly Nax8zsa nax = new Nax8zsa();

    [TestInitialize]
    public void Init() {
      nax.ZoneNumberOffset = 8;
      nax.SourceNumberOffset = 16;
      Router.Initialize();
    }

    [TestMethod]
    public void RouterInitialized_ZonesAndSourcesCreated() {
      Router.ZoneCount.Should().Be(8);
      Router.SourceCount.Should().Be(17, "because of the zero source 0.0.0.0");
      Router.Zones[15].Name.Should().Be("Zone 15");
      Router.Sources[30].Name.Should().Be("Source 30");
    }

    [TestMethod]
    public void RouterInitialized_SourcesAddressed() {
      var source = Router.Sources[30];
      var source2 = new NaxSource(400);

      source.MulticastAddress.Should().Be("239.95.0.30");
      source.Port.Should().Be(10030);
      source2.MulticastAddress.Should().Be("239.95.4.0");
    }

    [TestMethod]
    public void Route_LocalSource_RoutingSetToSourceNumber() {
      var source = Router.Sources[30];
      var zone = Router.Zones[15];
      var route = new AnyKeyDictionary<int, int>();
      nax.SetRouting = (index, value) => route[index] = value;
      zone.Route(source);
      route[7].Should().Be(14);
    }

    [TestMethod]
    public void Route_ExternalSource_RoutingSetTo17() {
      var source = Router.Sources[1];
      var zone = Router.Zones[15];
      var route = new AnyKeyDictionary<int, int>();
      nax.SetRouting = (index, value) => route[index] = value;
      zone.Route(source);
      route[7].Should().Be(17);
    }

    [TestMethod]
    public void Route_PortSet() {
      var source = Router.Sources[30];
      var zone = Router.Zones[15];
      var port = new AnyKeyDictionary<int, int>();
      nax.SetRxZonePort = (index, value) => port[index] = value;
      zone.Route(source);
      port[7].Should().Be(10030, "because port is always set whether external or internal source because it shouldn't hurt anything if it's set and unused");      
    }

    [TestMethod]
    public void Route_LocalSource_MulticastAddressCleared() {
      var source = Router.Sources[30];
      var zone = Router.Zones[15];
      var multicastAddress = new AnyKeyDictionary<int, string>();
      nax.SetRxZoneMulticastAddress = (index, value) => multicastAddress[index] = value.ToString();
      zone.Route(source);
      multicastAddress[7].Should().Be("0.0.0.0");
    }

    [TestMethod]
    public void Route_ExternalSource_MulticastAddressSet() {
      var source = new NaxSource(5);
      Router.RegisterSource(source);
      var zone = Router.Zones[15];
      var multicastAddress = new AnyKeyDictionary<int, string>();
      nax.SetRxZoneMulticastAddress = (index, value) => multicastAddress[index] = value.ToString();
      zone.Route(source);
      multicastAddress[7].Should().Be("239.95.0.5");
    }  }
}
