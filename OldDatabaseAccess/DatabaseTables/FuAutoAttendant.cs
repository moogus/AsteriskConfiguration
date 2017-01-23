using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

  public class FuAutoAttendant : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Announcement { get; set; }
    public virtual int Timeout { get; set; }
  }
}