//using Remotion.Data.Linq.Collections;
using System.Collections;
using System.Collections.ObjectModel;

namespace FSTECParser_Light
{
    public class ShortThreat
    {
        private static ObservableCollection<ShortThreat> allShortThreats;
        public static ObservableCollection<ShortThreat> AllShortThreats
        {
            get
            {
                if (allShortThreats == null)
                    allShortThreats = GetShortThreatsFromThreats(ExcelFile.Threats);
                return allShortThreats;
            }
        }
        public int Id { get; set; }
        public string VisibleId { get; set; }
        public string Name { get; set; }
        public Threat FullThreat { get; }
        public ShortThreat(Threat threat)
        {
            this.Id = threat.Id;
            this.VisibleId = "УБИ." + threat.Id;
            this.Name = threat.Name;
            this.FullThreat = threat;
        }
        public static ObservableCollection<ShortThreat> GetShortThreatsFromThreats(IEnumerable threats)
        {
            ObservableCollection<ShortThreat> oc = new ObservableCollection<ShortThreat>();
            foreach (var threat in threats)
                oc.Add(new ShortThreat(threat as Threat));
            return oc;
        }
    }
}
