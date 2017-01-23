using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CompanyDetailService
{
  public class CompanyDetail : ICompanyDetail
  {
    public string GetCompanyFromNumber(string value)
    {
      var company = string.Empty;

      if (IsNumber(value) && !string.IsNullOrEmpty(value))
      {
        company = new ComDb().Company.First(c => c.Phone.Equals(value)).Name;
      }
      
      return company ?? "Unknown company";
    }

    private bool IsNumber(string val)
    {
      return val.All(char.IsNumber);
    }
  }
}
