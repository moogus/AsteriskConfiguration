using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
  public class FuUserConfig : IDatabaseTable
  {
    public virtual int Id { get; set; }
    public virtual string Password { get; set; }
    public virtual string ExtensionNumber { get; set; }
    public virtual string Role { get; set; }
  }
}