using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AET.Unity.Av.NaxRouter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Av.NaxRouter.Tests {
  public class TestBase {
    [TestInitialize]
    public void TestInit() {
      Router.Clear();
      Test.ErrorMessageHandler.Clear();
    }
  }
}
