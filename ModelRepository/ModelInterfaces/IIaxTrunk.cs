namespace ModelRepository.ModelInterfaces
{
  public interface IIaxTrunk : ITrunk
  {
    string IaxName { get; set; }
    string HostAddress { get; set; }
    int IaxAllowedChannles { get; set; }
  }
}