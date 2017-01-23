namespace DataAccess.TableInterfaces
{
  public interface IAstSipReg : IDatabaseTable
  {
    string IpAddress { get; set; }
    string Number { get; set; }
    double StatusTime { get; set; }
    string Model { get; set; }
  }
}