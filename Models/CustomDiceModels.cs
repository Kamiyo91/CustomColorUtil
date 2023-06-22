using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class CustomDiceColorOptionRoot
    {
        [XmlElement("IconId")] public string IconId = "";
        [XmlElement("TextColor")] public ColorRoot TextColor;
    }
}