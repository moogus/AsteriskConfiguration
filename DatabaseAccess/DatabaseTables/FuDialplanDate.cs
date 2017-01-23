using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
    internal class FuDialplanDate : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual int FuDialplanId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}
