using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class EmotionCardOptionsRoot
    {
        [XmlElement("EmotionCardOption")] public List<EmotionCardOptionRoot> EmotionCardOption;
    }

    public class EmotionCardOptionRoot
    {
        [XmlElement("Id")] public List<int> Id;
        [XmlElement("ColorOptions")] public EmotionCardColorOptionRoot ColorOptions;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}