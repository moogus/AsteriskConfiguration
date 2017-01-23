using System.Collections.Generic;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.ModelUtilities.TrunkManager
{
  internal interface ITrunkManager
  {
    void RemoveAccessCodes( int trunkId);
    bool CreateDefaults(string defaultDestination, int trunkId);
    void UpdateAccessCodes(List<AccessCodeAndPriority> accessCodes, int trunkId);
    void ResetDdiUsedValue( int trunkId);
    List<ComAccessCode> SetAccessCodes(string accessCodes, int trunkId);
    List<AccessCodeAndPriority> GetAccessCodes(List<ComAccessCode> underAccessCodes);
  }
}
