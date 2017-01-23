using ModelUtilities.Internal;
using StructureMap.Configuration.DSL;

namespace ModelUtilities
{
  public class UtilityIOC :Registry
  {
    public UtilityIOC()
    {
      For<IAsteriskAudioStream>().Use<AsteriskAudioStream>();
      For<IIsForwardable>().Use<AsteriskAudioStream>();
      For<IMessageFolderManager>().Use<MessageFolderManager>();
      For<IFolderInformation>().Use<FolderInformation>();
    }
  }
}
