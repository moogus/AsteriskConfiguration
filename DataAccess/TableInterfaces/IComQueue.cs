namespace DataAccess.TableInterfaces
{
  public interface IComQueue : IDatabaseTable
  {
    string Number { get; set; }
    string Notes { get; set; }
    string QueueName { get; set; }
    string Department { get; set; }
    string Strategy { get; set; }
    bool RingOnBusy { get; set; }
    string DDINumber { get; set; }
    int VoiceMailId { get; set; }
    int VoicemailDelay { get; set; }
    int CLIId { get; set; }
    int ComMusicOnHoldId { get; set; }
    bool IncludeInDirectory { get; set; }
  }
}