using System.IO;

namespace ModelUtilities.Internal
{
  internal class AsteriskAudioStream : IAsteriskAudioStream
  {
    public Stream ForwardableStream { get; private set; }
    public Stream Stream(byte[] data)
    {
      ForwardableStream= new StreamReader(new MemoryStream(data)).BaseStream;
      return ForwardableStream;
    }
  }
}