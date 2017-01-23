using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
    public class QueuesJsonViewModel
    {
        public List<string[]> aaData;
        public List<JsonQueue> aoData;

        public QueuesJsonViewModel(IEnumerable<IQueue> queues)
        {
            aaData = new List<string[]>();
            aoData = new List<JsonQueue>();

            foreach (IQueue queue in queues)
            {
                var line = new string[14];
                line[0] = queue.Id.ToString(CultureInfo.InvariantCulture);
                line[1] = queue.Number;
                line[2] = queue.Notes;
                line[3] = queue.QueueName;
                line[4] = string.Join(",", queue.QueueMembers.Select(member =>
                  {
                      switch (member.Type)
                      {
                          case QueueMemberType.Extension:
                              return member.Extension.Number;

                          case QueueMemberType.Queue:
                              return member.Queue.Number;
                      }
                      return "";
                  }));

                line[5] = GetStartegyStringFromEnum(queue.Strategy);

                line[6] = queue.RingOnBusy ? "yes" : "no";

                line[7] = queue.VoiceMail == null ? "" : queue.VoiceMail.Number;
                line[8] = queue.VoicemailDelay.ToString(CultureInfo.InvariantCulture);
                line[9] = queue.MusicOnHold == null ? "" : queue.MusicOnHold.Name;

                line[10] = queue.IncludeInDirectory ? "include" : "exclude";
                line[11] = queue.Id.ToString(CultureInfo.InvariantCulture);
                line[12] = "";
                line[13] = "";

                aaData.Add(line);

                aoData.Add(new JsonQueue
                  {
                      Number = queue.Number,
                      QueueName = queue.QueueName,
                      Strategy = GetStartegyStringFromEnum(queue.Strategy)
                  });
            }
        }

        private static string GetStartegyStringFromEnum(QueueStrategy strategy)
        {
            switch (strategy)
            {
                case QueueStrategy.Ringall:
                    return "ring all";

                case QueueStrategy.Leastrecent:
                    return "least recent";

                case QueueStrategy.Fewestcalls:
                    return "fewest calls";

                case QueueStrategy.Random:
                    return "random";

                case QueueStrategy.Rrmemory:
                    return "round robin";

                case QueueStrategy.Linear:
                    return "linear";

                case QueueStrategy.Wrandom:
                    return "weighted random";

                default:
                    return "ring all";
            }
        }

        public class JsonQueue
        {
            public string Number { get; set; }
            public string QueueName { get; set; }
            public string Strategy { get; set; }
        }
    }
}