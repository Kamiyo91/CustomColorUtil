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
        [XmlAttribute("PackageId")] public string PackageId = "";
        [XmlElement("PassiveColorOptions")] public ColorOptionsRoot PassiveColorOptions;
    }
}