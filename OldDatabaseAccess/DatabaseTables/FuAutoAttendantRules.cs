using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

  public class FuAutoAttendantRules : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string AaName { get; set; }
    public virtual string Entry { get; set; }
    public virtual string Destination { get; set; }
    public virtual string DestinationType { get; set; }
  }
}