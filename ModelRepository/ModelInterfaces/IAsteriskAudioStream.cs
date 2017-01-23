using System.IO;

namespace ModelRepository.ModelInterfaces
{
  public interface IAsteriskAudioStream : IAsteriskStream
  {
  }

  public interface IAsteriskStream : IIsForwardable
  {
    Stream Stream { get; }
  }
}