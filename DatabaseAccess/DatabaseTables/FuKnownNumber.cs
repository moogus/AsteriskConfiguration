using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
    internal class FuKnownNumber : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Number { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsInternal { get; set; }
  }
}