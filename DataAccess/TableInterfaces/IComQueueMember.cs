namespace DataAccess.TableInterfaces
{
  public interface IComQueueMember : IDatabaseTable
  {
    int ParentQueueId { get; set; }
    int Penalty { get; set; }
    int Paused { get; set; }
    int Type { get; set; }
    int ExtensionId { get; set; }
    int QueueId { get; set; }
  }
}