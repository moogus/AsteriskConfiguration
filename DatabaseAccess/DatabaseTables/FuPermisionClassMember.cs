using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.DatabaseTables
{
  internal class FuPermisionClassMember : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual int PermissionClassId { get; set; }
    public virtual int PermissionPatternId { get; set; }
    public virtual int DialplanId { get; set; }
  }
}
