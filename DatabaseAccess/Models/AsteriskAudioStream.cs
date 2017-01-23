using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatabaseAccess.Models
{
    internal class AsteriskAudioStream : IAsteriskAudioStream
  {
    private readonly byte[] _data;

    internal AsteriskAudioStream(byte[] data)
    {
      _data = data;
    }

    Stream IIsForwardable.ForwardableStream { get { return Stream; } }
    public Stream Stream { get { return new StreamReader(new MemoryStream(_data)).BaseStream; } }

  }
}
