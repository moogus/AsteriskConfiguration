using System.IO;

namespace ModelUtilities
{
  public interface IAsteriskAudioStream : IIsForwardable
  {
    Stream Stream(byte[] data);
  }
}
