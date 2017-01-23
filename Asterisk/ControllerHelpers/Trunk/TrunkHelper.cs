using System;
using System.Collections.Generic;
using System.Linq;
using Asterisk.ControllerHelpers.Trunk.Interfaces;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ControllerHelpers.Trunk
{
    public class TrunkHelper : ITrunkHelper
    {
        private readonly IRepository _modelRepository;

        public TrunkHelper(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public TrunkType GetTrunkType(string info)
        {
            string type = info.Substring(0, 3);
            return (type.Equals("Sip") || type.Equals("Bri") || type.Equals("Iax"))
                     ? (TrunkType)Enum.Parse(typeof(TrunkType), type)
                     : TrunkType.Sip;
        }

        public IEnumerable<ITrunk> GetValidTrunks()
        {
            var rtnTrunk = new List<ITrunk>();
            var allFedTrunks = GetAllFedTrunks();

            var sumList = new List<int>();
            if (_modelRepository.GetList<ISipTrunk>().Any(s => s.TrunkType == TrunkType.Sip))
            {
                var sip = _modelRepository.GetList<ISipTrunk>().Where(s => s.TrunkType == TrunkType.Sip).ToList();
                rtnTrunk.AddRange(sip);
                sumList = sip.Select(s => s.Id).ToList();
            }

            if (_modelRepository.GetList<IBriTrunk>().Any(b => b.TrunkType == TrunkType.Bri))
            {
                var bri = _modelRepository.GetList<IBriTrunk>().Where(b => b.TrunkType == TrunkType.Bri).ToList();
                rtnTrunk.AddRange(bri);
                sumList = sumList.Union(bri.Select(b => b.Id)).ToList();
            }

            if (_modelRepository.GetList<IIaxTrunk>().Any(i => i.TrunkType == TrunkType.Iax))
            {
                var iax = _modelRepository.GetList<IIaxTrunk>().Where(i => i.TrunkType == TrunkType.Iax).ToList();
                rtnTrunk.AddRange(iax);
                sumList = sumList.Union(iax.Select(i => i.Id)).ToList();
            }

            if (!allFedTrunks.Any())
            {
                return rtnTrunk;
            }

            var fedIds = allFedTrunks.Select(t => t.Id);
            List<int> trunksIds = GetTrunkData(sumList, fedIds).ToList();

            return (from trunk in rtnTrunk from ids in trunksIds where trunk.Id == ids select trunk).ToList();
        }

        private static IEnumerable<int> GetTrunkData(IEnumerable<int> trunkData, IEnumerable<int> federatedTrunks)
        {
            return trunkData.Except(federatedTrunks);
        }

        public ITrunk SetSipTrunkValues(string info)
        {
            var trunk = _modelRepository.Add<ISipTrunk>();
            List<string> credentials = info.Split(',').ToList();
            trunk.SipUserName = credentials[1].Trim();
            trunk.SipPassword = credentials[2].Trim();
            trunk.SipHost = credentials[3].Trim();
            trunk.SipAllowedChannles = int.Parse(credentials[4].Trim());
            trunk.TrunkInPresentationType1 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[5].Trim());
            trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
            trunk.TrunkInPresentationType2 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[7].Trim());
            trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
            return trunk;
        }

        public ITrunk GetSipTrunkValues(string info, int id)
        {
            var trunk = _modelRepository.GetFromId<ISipTrunk>(id);
            List<string> credentials = info.Split(',').ToList();
            trunk.SipUserName = credentials[1].Trim();
            trunk.SipPassword = credentials[2].Trim();
            trunk.SipHost = credentials[3].Trim();
            trunk.SipAllowedChannles = int.Parse(credentials[4].Trim());
            trunk.TrunkInPresentationType1 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[5].Trim());
            trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
            trunk.TrunkInPresentationType2 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[7].Trim());
            trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
            return trunk;
        }

        public ITrunk SetBriTrunkValues(string info)
        {
            var trunk = _modelRepository.Add<IBriTrunk>();
            trunk.SetDahdiChannels(info);
            return trunk;
        }

        public ITrunk GetBriTrunkValues(string info, int id)
        {
            var trunk = _modelRepository.GetFromId<IBriTrunk>(id);
            trunk.SetDahdiChannels(info);
            return trunk;
        }

        public ITrunk SetIaxTrunkValues(string info)
        {
            var trunk = _modelRepository.Add<IIaxTrunk>();
            List<string> credentials = info.Split(',').ToList();
            trunk.IaxName = credentials[1];
            trunk.HostAddress = credentials[2];
            trunk.IaxAllowedChannles = int.Parse(credentials[4].Trim());
            trunk.TrunkInPresentationType1 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[5].Trim());
            trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
            trunk.TrunkInPresentationType2 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[7].Trim());
            trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
            return trunk;
        }

        public ITrunk GetIaxTrunkValues(string info, int id)
        {
            var trunk = _modelRepository.GetFromId<IIaxTrunk>(id);
            List<string> credentials = info.Split(',').ToList();
            trunk.IaxName = credentials[1];
            trunk.HostAddress = credentials[3];
            trunk.IaxAllowedChannles = int.Parse(credentials[4].Trim());
            trunk.TrunkInPresentationType1 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[5].Trim());
            trunk.TrunkInPresentationValue1 = credentials[6] == "null" ? string.Empty : credentials[6];
            trunk.TrunkInPresentationType2 =
              (TrunkInPresentationType)Enum.Parse(typeof(TrunkInPresentationType), credentials[7].Trim());
            trunk.TrunkInPresentationValue2 = credentials[6] == "null" ? string.Empty : credentials[8];
            return trunk;
        }

        private List<ITrunk> GetAllFedTrunks()
        {
            var federated4ComTrunks =
                _modelRepository.GetList<IFourComFederatedLink>().Where(f => f.Trunk != null).Select(f => f.Trunk).ToList();
            var samsungTrunks =
                _modelRepository.GetList<ISamsungFederatedLink>().Where(f => f.Trunk != null).Select(f => f.Trunk).ToList();

            var allFedTrunks = samsungTrunks.ToList();
            allFedTrunks.AddRange(federated4ComTrunks);
            return allFedTrunks;
        }
    }
}