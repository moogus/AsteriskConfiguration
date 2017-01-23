namespace Asterisk.ControllerHelpers.Directory.Entry.Interfaces
{
  public interface IDirectoryEntry
  {
    string FirstName { get; }
    string LastName { get; }
    string Name { get; }
    string MainNumber { get; }
    string Email { get; }
    string Mobile { get; }
    string Department { get; }
    string DDI { get; }
    DirectoryEntrytype Entrytype { get; }
  }
}