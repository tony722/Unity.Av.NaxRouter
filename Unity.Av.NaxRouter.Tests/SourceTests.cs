using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AET.Unity.Av.NaxRouter;
using AET.Unity.SimplSharp;
using FluentAssertions;

namespace Unity.Av.NaxRouter.Tests {
  [TestClass]
  public class SourceTests {
    [TestMethod]
    public void Route_ZoneAddedToSourcesListOfConnectedZones() {
      var source = new NaxSource(4);
      source.Connect(new NaxZone());
      source.RoutedZones.Count.Should().Be(1);
    }

    [TestMethod]
    public void Disconnect_ZoneRemovedFromSourcesList_RoutedZonesCountShouldBeZero() {
      var source = new NaxSource(4);
      var zone = new NaxZone(10);
      source.Connect(zone);
      source.Disconnect(zone);
      source.RoutedZones.Count.Should().Be(0);
    }

    [TestMethod]
    public void Disconnect_ZoneNotConnected_GeneratesWarning() {
      var source = new NaxSource(4);
      var zone = new NaxZone(10); 
      source.Disconnect(zone);
      source.RoutedZones.Count.Should().Be(0);
      Test.ErrorMessageHandler.LastErrorMessage.Should().Be("NaxSource #4 tried to disconnect from zone #10, but it was not currently connected");
    }

  }
}
