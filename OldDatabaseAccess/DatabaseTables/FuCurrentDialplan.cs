﻿namespace DatabaseAccess.DatabaseTables
{
  public class FuCurrentDialplan : IDatabaseTable
  {
        public virtual int Id { get; set; }
        public virtual int CurrentDialplan { get; set; }
        public virtual bool AutomaticallyChange { get; set; }
    }
}
