namespace DataAccess.TableInterfaces
{
  public interface IFuRingtone : IDatabaseTable
  {
    string SipHeader { get; set; }
    string FuRingtoneName { get; set; }
  }
}