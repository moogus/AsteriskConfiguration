using System;
using System.Linq;
using System.Text;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IContactModel
  {
    string UserName { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Department { get; set; }
    string Position { get; set; }
    string Email { get; set; }
    string Extension { get; set; }
    string DDI { get; set; }
    string Mobile { get; set; }
    bool IsAsteriskContact { get; set; }
  }
}
