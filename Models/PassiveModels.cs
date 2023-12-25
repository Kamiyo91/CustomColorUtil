using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class PassiveOptionsRoot
    {
        [XmlElement("PassiveOption")] public List<PassiveOptionRoot> PassiveColorOptions;
    }

    public class PassiveOptionRoot
    {
        [XmlElement("Id")] public List<int> Ids;
        [XmlElement("PassiveColorOptions")] public ColorOptionsRoot PassiveColorOptions;
        [XmlElement("CustomDiceColorOptions")] public CustomDiceColorOptionRoot CustomDiceColorOptions;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}