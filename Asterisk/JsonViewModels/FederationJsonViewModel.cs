using System.Collections.Generic;
using System.Globalization;
using Asterisk.Utilities.Interfaces;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
    public class FederationJsonViewModel
    {

        private readonly ITrunkValueGenerator _trunkValueGenerator;
        public List<string[]> aaData;

        public FederationJsonViewModel(IEnumerable<IFourComFederatedLink> fourComFederationData,
                                        IEnumerable<ISamsungFederatedLink> samsungFederationData,
                                        ITrunkValueGenerator trunkValueGenerator)
        {
            _trunkValueGenerator = trunkValueGenerator;
            aaData = new List<string[]>();

            foreach (IFourComFederatedLink f in fourComFederationData)
            {

                _trunkValueGenerator.SetTrunk(f.Trunk);

                var line = new string[8];
                line[0] = f.Id.ToString(CultureInfo.InvariantCulture);
                line[1] = f.Name;
                line[2] = f.Description;
                        line[3] = "4Com";

                line[4] = GetAccesCode(_trunkValueGenerator.GetAccessCodeString());
                line[5] = _trunkValueGenerator.GetCredentialString().Substring(4);
                line[6] = "";
                line[7] = "";

                aaData.Add(line);
            }

            foreach (ISamsungFederatedLink f in samsungFederationData)
            {

                _trunkValueGenerator.SetTrunk(f.Trunk);

                var line = new string[8];
                line[0] = f.Id.ToString(CultureInfo.InvariantCulture);
                line[1] = f.Name;
                line[2] = f.Description;
                line[3] = "Samsung";
                line[4] = GetAccesCode(_trunkValueGenerator.GetAccessCodeString());
                line[5] = _trunkValueGenerator.GetCredentialString().Substring(4);
                line[6] = "";
                line[7] = "";

                aaData.Add(line);
            }

        }



        private static string GetAccesCode(string code)
        {
            return code.Split(':')[0];
        }
    }
}