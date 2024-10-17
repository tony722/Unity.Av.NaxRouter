using System;
using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.Concurrent;

namespace AET.Unity.Av.NaxRouter {
  //Todo: Startup assertions of ports and multicast addresses

  public static class Router {
    private static string multicastBaseAddress;
    private static readonly ConcurrentDictionary<int, NaxZone> zones = new ConcurrentDictionary<int, NaxZone>();
    private static readonly ConcurrentDictionary<int, NaxSource> sources = new ConcurrentDictionary<int, NaxSource>();

    #region Constructors
    static Router() { AddZeroSource(); }
    private static void AddZeroSource() { sources[0] = new NaxSource(0); }

    /// <summary>
    /// Initializes this router and all switches controlled by this module.
    /// </summary>
    public static void Initialize() {
      if (Initialized != null) Initialized(null, EventArgs.Empty);
    }
    #endregion 

    #region Properties

    /// <summary>
    /// The base address used for NAX multicast.  While only the first two octets are necessary, the entire subnet can
    /// be specified. (e.g., 239.95.0.0 or 239.95.x.x or 239.95)
    ///
    /// This address serves as a starting point for assigning multicast addresses
    /// to the Crestron NAX device managed by this module. It should be within the 239.x.x.x subnet
    /// and must be unique on the network to avoid conflicts with other devices using broadcast addresses.
    /// 
    /// Note: For simplicity all addresses in this subnet (x.x.0.0 to x.x.255.255) should be reserved for the
    /// NAX devices controlled by this module, and addresses in this class B should not be used by any other multicast devices.
    /// <remarks>
    /// In reality a smaller range of addresses could be reserved if you wanted to. Here are the details to determine the
    /// minimum number of addresses in this subnet to reserve, and which will be used: The addresses are calculated based
    /// on the source numbers. Each NAX-8ZSA has 16 sources, so it will use 16 addresses.
    /// 
    /// So for example, if you specify a MulticastBaseAddress of "239.95.x.x" the addresses are assigned as follows:
    /// Sources 1-99:    239.95.0.1 to 239.95.0.99.
    /// Sources 100-199: 239.95.1.0 to 239.95.1.99
    /// Sources 200-299: 239.95.2.0 to 239.95.2.99
    /// and so on.
    /// </remarks>
    /// </summary>
    public static string MulticastBaseAddress {
      get {
        return multicastBaseAddress;
      }
      set {
        var octets = value.Split('.');
        if (octets.Length < 2) throw new ArgumentException("AvNaxRouter: Invalid MulticastBaseAddress format. Expected at least two octets.");
        multicastBaseAddress = string.Format("{0}.{1}",octets[0], octets[1]);
      }
    }

    /// <summary>
    /// Returns the number of zones that have been registered in this router.
    /// </summary>
    public static int ZoneCount { get { return zones.Count; } }

    /// <summary>
    /// Returns the number of sources that have been registered in this router.
    /// </summary>
    public static int SourceCount { get { return sources.Count; } }

    public static ReadOnlyIndexedProperty<int, NaxZone> Zones = new ReadOnlyIndexedProperty<int, NaxZone>(i => {
      NaxZone zone;
      if (!zones.TryGetValue(i, out zone)) {
        zone = new NaxZone();
        ErrorMessage.Warn("NaxRouter tried to get Zone {0}, which has not been registered.", i);
      }

      return zone;
    });

    public static ReadOnlyIndexedProperty<int, NaxSource> Sources = new ReadOnlyIndexedProperty<int, NaxSource>(i => {
      NaxSource source;
      if (!sources.TryGetValue(i, out source)) {
        source = sources[0];
        ErrorMessage.Warn("NaxRouter tried to get Source {0}, which has not been registered.", i);
      }

      return source;
    });
    public static string NoSignalMulticastAddress { get { return "0.0.0.0"; } }

    #endregion

    public static void RegisterZone(int zoneId, NaxZone zone) {
      if (zone == null) return;
      zones[zoneId] = zone;
    }

    public static void RegisterSource(NaxSource source) {
      if ((source == null).WarnIf("NaxRouter: Tried to register null source.")) return;
      if ((source.SourceNumber == 0).ErrorIf("NaxRouter: Tried to register source with SourceNumber = 0.")) return;
      sources.ContainsKey(source.SourceNumber).WarnIf("NaxRouter: Overwrote source with SourceNumber = {0}", source.SourceNumber);
      sources[source.SourceNumber] = source;
    }

    public static void Clear() {
      zones.Clear();
      sources.Clear();
      AddZeroSource();
    }

    public static void RouteSourceToZone(ushort sourceNumber, ushort zoneNumber) {
      if (zoneNumber == 0) return;
      zones[zoneNumber].Route(sources[sourceNumber]);
    }

    public static event EventHandler Initialized;
  }
}