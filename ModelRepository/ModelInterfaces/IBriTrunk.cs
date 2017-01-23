using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
    public interface IBriTrunk : ITrunk
    {
        IEnumerable<string> DahdiChannels { get; }
        void SetDahdiChannels(string channels);
    }
}