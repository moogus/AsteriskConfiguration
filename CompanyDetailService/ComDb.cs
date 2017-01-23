using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CompanyDetailService
{
  public class ComDb : DbContext
  {
    public DbSet<Company> Company { get; set; }
  }
}
