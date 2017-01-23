namespace DatabaseAccess.ModelUtilities.QueueStrategy
{
  public static class QueueStrategyAddOns
  {
    public static string ToDisplayString(this DatabaseAccess.QueueStrategy strategy)
    {
      switch (strategy)
      {
        case DatabaseAccess.QueueStrategy.Ringall:
          return "ring all";
        case DatabaseAccess.QueueStrategy.Leastrecent:
          return "least recent";
        case DatabaseAccess.QueueStrategy.Fewestcalls:
          return "fewest calls";
        case DatabaseAccess.QueueStrategy.Random:
          return "random";
        case DatabaseAccess.QueueStrategy.Rrmemory:
          return "round robin";
        case DatabaseAccess.QueueStrategy.Linear:
          return "linear";
        case DatabaseAccess.QueueStrategy.Wrandom:
          return "weighted random";
      }
      return "";
    }

    public static DatabaseAccess.QueueStrategy ToQueueStrategy(this string strategyString)
    {
      switch (strategyString)
      {
        case "ring all":
          return DatabaseAccess.QueueStrategy.Ringall;
        case "least recent":
          return DatabaseAccess.QueueStrategy.Leastrecent;
        case "fewest calls":
          return DatabaseAccess.QueueStrategy.Fewestcalls;
        case "random":
          return DatabaseAccess.QueueStrategy.Random;
        case "round robin":
          return DatabaseAccess.QueueStrategy.Rrmemory;
        case "linear":
          return DatabaseAccess.QueueStrategy.Linear;
        case "weighted random":
          return DatabaseAccess.QueueStrategy.Wrandom;
        default:
          return DatabaseAccess.QueueStrategy.Ringall;
      }
    }
  }
}
