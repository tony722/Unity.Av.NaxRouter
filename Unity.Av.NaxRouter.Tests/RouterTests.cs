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
  public class RouterTests : TestBase {

    [TestInitialize]
    public void TestInitialize() {
    }

    [TestMethod]
    public void RegisterZone_ZoneIsNull_DoesNotRegisterZone() {
      Action act = () => Router.RegisterZone(1, null);
      act.Should().NotThrow();
      Router.ZoneCount.Should().Be(0);
    }

    [TestMethod]
    public void RegisterZone_ZoneIsRegistered() {
      Router.RegisterZone(1, new NaxZone());
      Router.ZoneCount.Should().Be(1);
    }
  }
}