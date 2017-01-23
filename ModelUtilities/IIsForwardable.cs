using System.IO;

namespace ModelUtilities
{
  public interface IIsForwardable
  {
    Stream ForwardableStream { get; }
  }
}