using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Directory.Entry
{
  public class ExtensionDirectoryEntry : IDirectoryEntry
  {
    public ExtensionDirectoryEntry(IExtension ext)
    {
      FirstName = ext.FirstName;
      LastName = ext.LastName;
      Name = string.Format("{0} {1}", FirstName, LastName);
      Email = ext.Email;
      MainNumber = ext.Number;
      Mobile = "";
      Department = ext.Department;
      DDI = ext.DDI != null ? ext.DDI.DDINumber : "";
      Entrytype = DirectoryEntrytype.Extension;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Name { get; private set; }
    public string MainNumber { get; private set; }
    public string Email { get; private set; }
    public string Mobile { get; private set; }
    public string Department { get; private set; }
    public string DDI { get; private set; }
    public DirectoryEntrytype Entrytype { get; private set; }
  }
}