using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AET.Unity.Av.NaxRouter;
using AET.Unity.SimplSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Av.NaxRouter.Tests {
  [TestClass]
  public static class Test {
    private static readonly TestErrorMessageHandler errorMessageHandler = new TestErrorMessageHandler();

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context) {
      ErrorMessage.ErrorMessageHandler = errorMessageHandler;
      Router.MulticastBaseAddress = "239.95.0.0/16";
    }


    public static TestErrorMessageHandler ErrorMessageHandler { get { return errorMessageHandler; } }
  }
}
