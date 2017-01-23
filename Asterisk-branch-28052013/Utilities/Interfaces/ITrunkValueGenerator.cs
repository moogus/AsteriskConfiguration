using DatabaseAccess;

namespace Asterisk.Utilities.Interfaces
{
  public interface ITrunkValueGenerator
  {
    string GetCredentialString();
    string GetAccessCodeString();
    void SetTrunk(ITrunk trunk);
  }
}