using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class CategoryOptionsRoot
    {
        [XmlElement("CategoryOption")] public List<CategoryOptionRoot> CategoryOption;
    }

    public class CategoryOptionRoot
    {
        [XmlAttribute("CategoryNumber")] public string AdditionalValue = "";
        [XmlElement("BookDataColor")] public ColorOptionsRoot BookDataColor;
        [XmlElement("PackageId")] public string PackageId = "";
    }
}