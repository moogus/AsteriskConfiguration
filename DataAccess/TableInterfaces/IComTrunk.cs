namespace DataAccess.TableInterfaces
{
  public interface IComTrunk : IDatabaseTable
  {
    string Type { get; set; }
    string ComTrunkName { get; set; }
    string DefaultDestination { get; set; }
    string CLIPresentationType1 { get; set; }
    string CLIPresentationValue1 { get; set; }
    string CLIPresentationType2 { get; set; }
    string CLIPresentationValue2 { get; set; }
  }
}