﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.DatabaseTables
{
    internal class FuDialplan : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}