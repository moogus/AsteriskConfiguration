using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
  public interface IQueue : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Notes { get; set; }
    string QueueName { get; set; }
    string Department { get; set; }
    QueueStrategy Strategy { get; set; }
    bool RingOnBusy { get; set; }
    IDDI DDI { get; set; }
    ICLI CLI { get; set; }
    IVoiceMail VoiceMail { get; set; }
    int VoicemailDelay { get; set; }
    IMusicOnHold MusicOnHold { get; set; }

    IEnumerable<IQueueMember> QueueMembers { get; }
    bool IncludeInDirectory { get; set; }

    void AddExtensionAsQueueMember(IExtension extension, int queueId);
    void AddQueueAsQueueMember(IQueue queueToAdd, int queueId);
  }
}