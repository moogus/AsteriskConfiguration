using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.DatabaseTables
{
    internal class ComDahdiChannel : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual int TrunkId { get; set; }
    public virtual string ChannelName { get; set; }
  }
}
