namespace ModelRepository.ModelInterfaces
{
  public interface ISipTrunk : ITrunk
  {
    string SipUserName { get; set; }
    string SipPassword { get; set; }
    string SipHost { get; set; }
    int SipAllowedChannles { get; set; }
  }
}