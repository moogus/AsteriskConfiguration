using System;

namespace ModelRepository.ModelInterfaces
{
  public interface IDialplan : IModel, IEquatable<IDialplan>
  {
    int Id { get; }
    string Name { get; set; }
  }
}