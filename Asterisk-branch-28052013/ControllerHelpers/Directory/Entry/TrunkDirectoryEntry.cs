using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using DatabaseAccess;

namespace Asterisk.ControllerHelpers.Directory.Entry
{
  public class TrunkDirectoryEntry : IDirectoryEntry
  {
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Name { get; private set; }
    public string MainNumber { get; private set; }
    public string Email { get; private set; }
    public string Mobile { get; private set; }
    public string Department { get; private set; }
    public string DDI { get; private set; }
    public DirectoryEntrytype Entrytype { get; private set; }

    public TrunkDirectoryEntry(ITrunk trunk)
    {
      FirstName = "";
      LastName = "";
      Name = trunk.Name;
      Email = "";
      MainNumber = trunk.Name;
      Mobile = "";
      Department = "";
      DDI = "";
      Entrytype = DirectoryEntrytype.Trunk;
    }
  }
}