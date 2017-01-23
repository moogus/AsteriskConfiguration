namespace DataAccess.TableInterfaces
{
  public interface IComMusicOnHold :IDatabaseTable
  {
    string Directory { get; set; }
    string Application { get; set; }
    string ComMusicOnHoldName { get; set; }
    string Mode { get; set; }
    char Digit { get; set; }
    string Sort { get; set; }
    string Format { get; set; }
    string Random { get; set; }
  }
}