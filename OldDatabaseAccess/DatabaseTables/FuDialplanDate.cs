using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
    public class FuDialplanDate : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int FuDialplanId { get; set; }
        public virtual bool FuIsRange { get; set; }
        public virtual int FuRangeId { get; set; }
    }
}
