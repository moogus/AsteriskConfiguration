namespace DataAccess.TableInterfaces
{
  public interface IFuDefaults : IDatabaseTable
  {
    string Type { get; set; }
    int ColumnIndex { get; set; }
    string ColumnType { get; set; }
    string ColumnTitle { get; set; }
    string Property { get; set; }
    string Default { get; set; }
    string Picker { get; set; }
  }
}