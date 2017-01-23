using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
    public class FuDialplanRange : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string DaysOfWeek { get; set; }
        public virtual string TimeRange { get; set; }
        public virtual int Priority { get; set; }
        public virtual int FuDialplanId { get; set; }
    }
}