using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.DatabaseTables
{
 internal class FuPermissionClass : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
   }
}
