using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.Security.Policy;
using AET.Unity.Av.NaxRouter;
using AET.Unity.Av.NaxRouter.Splus;
using AET.Unity.SimplSharp;
using FluentAssertions;

namespace Unity.Av.NaxRouter.Tests {
  [TestClass]
  public class NaxSwitcherTests : TestBase {
    private TestNax8zsaSwitcher nax1 = new TestNax8zsaSwitcher(0, 0);
    private TestNax8zsaSwitcher nax2 = new TestNax8zsaSwitcher(8, 16);
    private bool ignoreCleanup = false;

    [TestInitialize]
    public void TestInitialize() {
      Router.Initialize();
    }

    [TestCleanup]
    public void TestCleanup() {
      if (ignoreCleanup) return;
      Test.ErrorMessageHandler.Messages.Count.Should().Be(0, "because we shouldn't have any errors at this point");
    }

    [TestMethod]
    public void RouteSourceToZone_LocalSource_DoesLocalRoute() {
      nax2.RouteSourceToZone(18, 7);
      nax2.Routes[7].Should().Be(2);
      nax2.ZoneMulticastAddresses[7].Should().Be("0.0.0.0", "because it's not streaming a multicast source");
      //nax2.SourceMulticastAddresses[18].Should().Be("0.0.0.0", "because this source is not routed via multicast anywhere");
    }

    [TestMethod]
    public void RouteSourceToZone_Zero_RoutesToZero() {
      nax1.RouteSourceToZone(0,1);
      nax1.Routes[1].Should().Be(0);
      nax1.ZoneMulticastAddresses[1].Should().Be("0.0.0.0");

    }

    [TestMethod]
    public void RouteSourceToZone_RemoteSource_DoesRemoteRoute() {
      nax2.RouteSourceToZone(2, 7);
      nax2.Routes[7].Should().Be(17);
      nax1.SourceMulticastAddresses[2].Should().Be("239.95.0.2");
      nax1.SourcePorts[2].Should().Be(10002);
      nax2.ZoneMulticastAddresses[7].Should().Be("239.95.0.2");
      nax2.ZonePorts[7].Should().Be(10002);
    }



    [TestMethod]
    public void Route_ExternalSource_RoutesCorrectly() {
      nax2.RouteSourceToZone(1, 8);
      nax1.SourceMulticastAddresses[1].Should().Be("239.95.0.1");
      nax1.SourcePorts[1].Should().Be(10001);
      nax2.ZoneMulticastAddresses[8].Should().Be("239.95.0.1");
      nax2.ZonePorts[8].Should().Be(10001);
    }

    [TestMethod]
    public void Route_UnregisteredIndexSupplied_RoutesTo0_0_0_0() {
      nax1.RouteSourceToZone(99, 1);
      nax1.ZoneMulticastAddresses[1].Should().Be("0.0.0.0");
      Test.ErrorMessageHandler.LastErrorMessage.Should().Be("NaxRouter tried to get Source 99, which has not been registered.");
      ignoreCleanup = true;
    }
    
    [TestMethod]
    public void Route_ZoneHasExistingSourceStreamedRouted_ZoneRemovedFrom_SourcesListOfConnectedZones() {
      nax2.RouteSourceToZone(2,1);
      nax2.RouteSourceToZone(3,1);
      Router.Sources[2].RoutedZones.Count.Should().Be(0);
      Router.Sources[3].RoutedZones.Count.Should().Be(1);
    }

    [TestMethod]
    public void Route_SourceConnectsToFirstZone_FiresMulticastAddressChangedEvent() {
      nax2.RouteSourceToZone(4,1);
      nax1.SourceMulticastAddresses[4].Should().Be("239.95.0.4");
    }

    [TestMethod]
    public void Route_SourceAlreadyConnectedToAZone_DoesNotFireMulticastAddressChangedEvent() {
      nax2.RouteSourceToZone(4,1);
      nax1.SourceMulticastAddresses[4].Should().Be("239.95.0.4");
      nax1.SourceMulticastAddresses[4] = ""; 
      nax2.RouteSourceToZone(4,2);
      nax1.SourceMulticastAddresses[4].Should().Be("", "because it shouldn't be set again since it's already broadcasting");
    }

    
    [TestMethod]
    public void Route_DisconnectsLastZone_FiresMulticastAddressChangedEvent() {
      nax2.RouteSourceToZone(4,1);
      nax2.RouteSourceToZone(4,2);
      nax1.SourceMulticastAddresses[4].Should().Be("239.95.0.4");
      nax2.RouteSourceToZone(0,1);
      nax1.SourceMulticastAddresses[4].Should().Be("239.95.0.4");
      nax2.RouteSourceToZone(0,2);
      nax1.SourceMulticastAddresses[4].Should().Be("0.0.0.0");
    }

    [TestMethod]
    public void Route_FromRouter_RoutesCorrectly() {
      Router.Zones[9].Route(Router.Sources[4]);
      nax1.SourceMulticastAddresses[4].Should().Be("239.95.0.4");
      nax1.SourcePorts[4].Should().Be(10004);
      nax2.ZoneMulticastAddresses[1].Should().Be("239.95.0.4");
      nax2.ZonePorts[1].Should().Be(10004);
    }
  }
}