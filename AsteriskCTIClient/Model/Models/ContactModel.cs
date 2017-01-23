using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.Model.Models
{
  public class ContactModel : IContactModel
  {
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public string Extension { get; set; }
    public string DDI { get; set; }
    public string Mobile { get; set; }
    public bool IsAsteriskContact { get; set; }
  }
}