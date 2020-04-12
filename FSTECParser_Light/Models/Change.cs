using FSTECParser_Light.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSTECParser_Light.Models
{
    class Change
    {
        public int Id { get; private set; }
        public string Param { get; private set; }
        public string LastValue { get; set; }
        public string NewValue { get; set; }
        public Change(int id, string param, string lastValue, string newValue)
        {
            Id = id;
            Param = param;
            LastValue = lastValue;
            NewValue = newValue;
        }
        public static List<Change> GetChanges(List<Threat> lastThreats, List<Threat> newThreats)
        {
            List<Change> changes = new List<Change>();
            foreach (Threat threat in lastThreats)
            {
                Threat temp = newThreats.Where(el => el.Id == threat.Id).Select(el => el).First();
                changes = changes.Union(GetChangesByOneCoupleThreat(threat, temp)).ToList();
            }
            return changes;
        }
        private static List<Change> GetChangesByOneCoupleThreat(Threat lastThreat, Threat newThreat)
        {
            List<Change> changes = new List<Change>();
            if (lastThreat.Name != newThreat.Name)
                changes.Add(new Change(lastThreat.Id, "Наименование УБИ", lastThreat.Name.ToString(), newThreat.Name.ToString()));
            if (lastThreat.Description != newThreat.Description)
                changes.Add(new Change(lastThreat.Id, "Описание", lastThreat.Description.ToString(), newThreat.Description.ToString()));
            if (lastThreat.Source != newThreat.Source)
                changes.Add(new Change(lastThreat.Id, "Источник угрозы (характеристика и потенциал нарушителя)", lastThreat.Source.ToString(), newThreat.Source.ToString()));
            if (lastThreat.Target != newThreat.Target)
                changes.Add(new Change(lastThreat.Id, "Объект воздействия", lastThreat.Target.ToString(), newThreat.Target.ToString()));
            if (lastThreat.PrivacyViolation != newThreat.PrivacyViolation)
                changes.Add(new Change(lastThreat.Id, "Нарушение конфиденциальности", BoolToStringConverter(lastThreat.PrivacyViolation), BoolToStringConverter(newThreat.PrivacyViolation)));
            if (lastThreat.IntegrityViolation != newThreat.IntegrityViolation)
                changes.Add(new Change(lastThreat.Id, "Нарушение целостности", BoolToStringConverter(lastThreat.IntegrityViolation), BoolToStringConverter(newThreat.IntegrityViolation)));
            if (lastThreat.AccessViolation != newThreat.AccessViolation)
                changes.Add(new Change(lastThreat.Id, "Нарушение доступности", BoolToStringConverter(lastThreat.AccessViolation), BoolToStringConverter(newThreat.AccessViolation)));
            return changes;
        }
        private static string BoolToStringConverter(bool b)
        {
            return b ? "Да" : "Нет";
        }
    }
}
