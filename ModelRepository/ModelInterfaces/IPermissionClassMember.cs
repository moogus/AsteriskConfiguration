namespace ModelRepository.ModelInterfaces
{
  public interface IPermissionClassMember : IModel
  {
    int Id { get; }
    IPermisionClass ParentPermissionClass { get; set; }
    IPermissionPattern Pattern { get; set; }
    IDialplan Dialplan { get; set; }
  }
}