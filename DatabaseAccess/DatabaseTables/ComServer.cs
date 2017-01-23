using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

    internal class ComServer : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string IpAddress { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public virtual string MailServer { get; set; }
    public virtual string VoicemailDialNumber { get; set; }
    public virtual string AdminAccount { get; set; }
    public virtual string ExtensionIpRange { get; set; }
  }
}