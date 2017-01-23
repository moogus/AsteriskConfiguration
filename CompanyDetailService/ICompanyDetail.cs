using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CompanyDetailService
{
  [ServiceContract]
  public interface ICompanyDetail
  {
    [OperationContract]
    string GetCompanyFromNumber(string value);
  }
}
