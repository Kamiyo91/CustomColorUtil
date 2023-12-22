using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class CardOptionsRoot
    {
        [XmlElement("CardOption")] public List<CardOptionRoot> CardOption;
    }

    public class CardOptionRoot
    {
        [XmlElement("Id")] public List<int> Ids = new List<int>();
        [XmlElement("CardColorOptions")] public CardColorOptionRoot CardColorOptions;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }

    public class CardColorOptionRoot
    {
        [XmlElement("LeftFrame")] public string LeftFrame = "";
        [XmlElement("RightFrame")] public string RightFrame = "";
        [XmlElement("FrontFrame")] public string FrontFrame = "";
        [XmlElement("ApplyFrontColor")] public bool ApplyFrontColor;
        [XmlElement("ApplySideFrontColors")] public bool ApplySideFrontColors;
        [XmlElement("CardColor")] public ColorRoot CardColor;
        [XmlElement("CustomIcon")] public string CustomIcon = "";
        [XmlElement("CustomIconColor")] public ColorRoot CustomIconColor;
        [XmlElement("CustomDiceIcon")] public List<CustomDiceIconRoot> CustomDiceIcon = new List<CustomDiceIconRoot>();


        [XmlElement("IconColor")] public HsvColorRoot IconColor;


        [XmlElement("UseHSVFilter")] public bool UseHSVFilter = true;
    }

    public class CustomDiceIconRoot
    {
        [XmlElement("KeywordIconId")] public string KeywordIconId = "";
        [XmlElement("TextColor")] public ColorRoot TextColor;
        [XmlElement("DiceColor")] public ColorRoot DiceColor;
        [XmlAttribute("DiceNumber")] public int DiceNumber;


        [XmlElement("KeywordIconIdClash")] public string KeywordIconIdClash = "";
        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}