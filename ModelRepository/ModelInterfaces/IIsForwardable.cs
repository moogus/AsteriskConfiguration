using System.IO;

namespace ModelRepository.ModelInterfaces
{
  public interface IIsForwardable
  {
    Stream ForwardableStream { get; }
  }
}