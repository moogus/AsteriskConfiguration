using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

    internal class ComCLI : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string CLIName { get; set; }
    public virtual string CLINumber { get; set; }
    public virtual int TrunkId { get; set; }
  }
}