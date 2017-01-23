namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IPresenceManagerModel
  {
    IPresenceItem GetAsteriskPresenceItem(string extension);
  }
}