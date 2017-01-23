using System.Collections.Generic;
using System.Linq;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.JsonViewModels
{
    public class RouteJsonViewModel
    {
        private const int NumberOfDestinations = 4;
        public List<string[]> aaData;

        public RouteJsonViewModel(IDialplan dialplan, IRepository modelRepository)
        {
            aaData = new List<string[]>();

            var routes = modelRepository.GetList<IRoutingRule>().Where(r => r.Dialplan != null && r.Dialplan.Equals(dialplan)).ToList();

            var numbers = routes.Select(r => r.Number).Distinct();

            foreach (var dialledNumber in numbers)
            {
                var line = new List<string> { dialledNumber, dialledNumber };

                line.AddRange(CreateDestinations(CreateStringDestinations(dialledNumber, routes)));

                line.Add("");
                line.Add("");
                aaData.Add(line.ToArray());
            }
        }

        private static List<string> CreateStringDestinations(string dialledNumber, IEnumerable<IRoutingRule> routes)
        {
            IOrderedEnumerable<IRoutingRule> destinations = routes.Where(r => r.Number == dialledNumber).OrderBy(r => r.Order);
            List<string> stringDestinations = destinations.Select(
              dest =>
              string.Format("{0}, {1} for {2}", dest.DestinationType, dest.DestinationNumber, dest.Time.ToString())
              ).Take(5).ToList();

            return stringDestinations;
        }

        private static List<string> CreateDestinations(List<string> rulesAsString)
        {
            return (rulesAsString.Count > NumberOfDestinations)
                     ? rulesAsString
                     : CreateDestinations(AddBlankString(rulesAsString));
        }

        private static List<string> AddBlankString(List<string> stringList)
        {
            stringList.Add("");
            return stringList;
        }
    }
}