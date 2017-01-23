using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.DatabaseTables
{
  internal class FuPermissionPattern : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Pattern { get; set; }
    public virtual string Name { get; set; }
  }
}
