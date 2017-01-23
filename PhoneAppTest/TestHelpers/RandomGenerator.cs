using System;
using System.Globalization;
using System.Linq;
using DatabaseAccess;

namespace PhoneAppTests.TestHelpers
{
  internal  class RandomGenerator
  {
    internal  ForwardingDestination RandomForwrdingDestination { get { return GetRandomForwardingDestinationType(); } }
    internal string RandomString { get { return GetRandomString(); } }

    private readonly Random _random;

    public RandomGenerator()
    {
      _random = new Random();
    }

    private  ForwardingDestination GetRandomForwardingDestinationType()
    {
      switch (_random.Next(0, Enum.GetNames(typeof(ForwardingDestination)).Count()))
      {
        default:
          return ForwardingDestination.Extension;
        case 1:
          return ForwardingDestination.Group;
        case 2:
          return ForwardingDestination.Voicemail;
        case 3:
          return ForwardingDestination.External;
      }
    }

    private  string GetRandomString()
    {
      return (_random).Next().ToString(CultureInfo.InvariantCulture);
    }
  }
}
