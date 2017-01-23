using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

  public class FuDDI : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string DDI { get; set; }
  }
}