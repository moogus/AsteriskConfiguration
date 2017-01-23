namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IValidNumber : INumber
  {
   bool IsValidNumber(string number);
  }
}