using Asterisk.ControllerHelpers.Directory.Entry.Interfaces;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Directory.Entry
{
  public class QueueDirectoryEntry : IDirectoryEntry
  {
    public QueueDirectoryEntry(IQueue queue)
    {
      FirstName = "";
      LastName = "";
      Name = "";
      MainNumber = queue.Number;
      Email = "";
      Mobile = "";
      Department = queue.Department;
      DDI = "";
      Entrytype = DirectoryEntrytype.Queue;
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