namespace ModelRepository.ModelInterfaces
{
  public interface IDefault : IModel
  {
    int Id { get; }
    string Type { get; set; }
    int Index { get; set; }
    string JavascriptColumnType { get; set; }
    string ColumnTitle { get; set; }
    string JavascriptProperty { get; set; }
    string DefaultValue { get; set; }
    string Picker { get; set; }
  }
}