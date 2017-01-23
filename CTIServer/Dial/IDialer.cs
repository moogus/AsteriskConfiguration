namespace CTIServer.Dial
{
  public interface IDialer
  {
    void AutoDial(string extension, string numberToDial);
  }
}