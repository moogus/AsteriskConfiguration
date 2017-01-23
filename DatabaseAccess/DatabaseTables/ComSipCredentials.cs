using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.DatabaseTables
{
    internal class ComSipCredentials : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public virtual string Host { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual int AllowedChannels { get; set; }
  }
}
