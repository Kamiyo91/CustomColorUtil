using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class DropBookOptionsRoot
    {
        [XmlElement("DropBookOption")] public List<DropBookOptionRoot> DropBookOption;
    }

    public class DropBookOptionRoot
    {
        [XmlElement("Id")] public List<int> Ids;
        [XmlElement("DropBookColorOptions")] public ColorOptionsRoot DropBookColorOptions;


        [XmlAttribute("PackageId")] public string PackageId = "";
    }
}