using System.Collections.Generic;

namespace FSTECParser_Light
{
    public class Threat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public bool PrivacyViolation { get; set; }
        public bool IntegrityViolation { get; set; }
        public bool AccessViolation { get; set; }
        public Threat(Dictionary<string, string> values)
        {
            Id = int.Parse(values["Идентификатор УБИ"]);
            Name = values["Наименование УБИ"];
            Description = values["Описание"];
            Source = values["Источник угрозы (характеристика и потенциал нарушителя)"];
            Target = values["Объект воздействия"];
            PrivacyViolation = values["Нарушение конфиденциальности"] == "1" ? true : false;
            IntegrityViolation = values["Нарушение целостности"] == "1" ? true : false;
            AccessViolation = values["Нарушение доступности"] == "1" ? true : false;
        }
        public Threat() { }
        public override string ToString()
        {
            return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nSource: {Source}\nTarget: {Target}\nPrivacy: {PrivacyViolation}";
        }
        public override bool Equals(object obj)
        {
            Threat inputThreat = obj as Threat;
            if (this.AccessViolation    == inputThreat.AccessViolation &&
                this.Description        == inputThreat.Description &&
                this.Id                 == inputThreat.Id &&
                this.IntegrityViolation == inputThreat.IntegrityViolation &&
                this.Name               == inputThreat.Name &&
                this.PrivacyViolation   == inputThreat.PrivacyViolation &&
                this.Source             == inputThreat.Source &&
                this.Target             == inputThreat.Target)
                return true;
            return false;
        }

    }
}
