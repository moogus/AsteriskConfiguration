namespace ModelRepository.ModelInterfaces
{
  public interface IQueueMember : IModel
  {
    int Id { get; }
    IQueue ParentQueue { get; set; }
    int Penalty { get; set; }
    int Paused { get; set; }
    QueueMemberType Type { get; }
    IExtension Extension { get; set; }
    IQueue Queue { get; set; }
  }
}