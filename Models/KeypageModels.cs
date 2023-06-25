using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class KeypageOptionsRoot
    {
        [XmlElement("KeypageOption")] public List<KeypageOptionRoot> KeypageOption;
    }

    public class KeypageOptionRoot
    {
        [XmlElement("KeypageColorOptions")] public ColorOptionsRoot KeypageColorOptions;
        [XmlElement("CustomDiceColorOptions")] public CustomDiceColorOptionRoot CustomDiceColorOptions;


        [XmlElement("Id")] public List<int> Ids;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}