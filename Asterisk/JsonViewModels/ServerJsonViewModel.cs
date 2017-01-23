using System.Collections.Generic;
using System.Globalization;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
    public class ServerJsonViewModel
    {
        public List<string[]> aaData;
        public List<JsonServerSettings> aoData;

        public ServerJsonViewModel(IEnumerable<IServer> serverData)
        {
            aaData = new List<string[]>();
            aoData = new List<JsonServerSettings>();

            foreach (IServer s in serverData)
            {
                var line = new string[10];
                line[0] = s.Id.ToString(CultureInfo.InvariantCulture);
                line[1] = s.IpAddress;
                line[2] = s.Credentials.UserName;
                line[3] = s.Credentials.Password;
                line[4] = s.MailServer.Host;
                line[5] = s.VoicemailDialNumber;
                line[6] = s.AdminExtension != null ? s.AdminExtension.Number : "";
                line[7] = s.ExtensionIpRange;
                line[8] = "";
                line[9] = "";
                aaData.Add(line);

                aoData.Add(new JsonServerSettings { Id = s.Id, IpAddress = s.IpAddress });
            }
        }

        public class JsonServerSettings
        {
            public int Id { get; set; }
            public string IpAddress { get; set; }
        }
    }
}