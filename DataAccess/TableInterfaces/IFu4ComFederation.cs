namespace DataAccess.TableInterfaces
{
    public interface IFu4ComFederation :IDatabaseTable
    {
        string Description { get; set; }
        int ComTrunkId { get; set; }
        string FuFederationName { get; set; }
    }
}