using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class StageOptionsRoot
    {
        [XmlElement("StageOption")] public List<StageOptionRoot> StageOption;
    }

    public class StageOptionRoot
    {
        [XmlElement("Id")] public List<int> Ids;
        [XmlElement("StageColorOptions")] public ColorOptionsRoot StageColorOptions;
        [XmlElement("CustomDiceColorOptions")] public CustomDiceColorOptionRoot CustomDiceColorOptions;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}