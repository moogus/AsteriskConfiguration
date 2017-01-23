using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{

  public class FuRingtone : IDatabaseTable
  {
      public virtual int Id { get; set; }
      public virtual string Name { get; set; }
      public virtual string SipHeader { get; set; }
  }
}