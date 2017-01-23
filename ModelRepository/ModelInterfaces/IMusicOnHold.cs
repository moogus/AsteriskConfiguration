namespace ModelRepository.ModelInterfaces
{
  public interface IMusicOnHold : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Directory { get; }
    string Application { get; set; }
    MusicOnHoldMode Mode { get; }
    char Digit { get; set; }
    bool Sort { get; set; }
    string Format { get; set; }
    bool Random { get; set; }
  }
}