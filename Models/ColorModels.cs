using System.Xml.Serialization;

namespace CustomColorUtil.Models
{
    public class ColorRoot
    {
        [XmlAttribute("A")] public float A;
        [XmlAttribute("B")] public float B;
        [XmlAttribute("G")] public float G;
        [XmlAttribute("R")] public float R;
    }

    public class HsvColorRoot
    {
        [XmlAttribute("H")] public float H;
        [XmlAttribute("S")] public float S;
        [XmlAttribute("V")] public float V;
    }

    public class ColorOptionsRoot
    {
        [XmlElement("TextColor")] public ColorRoot TextColor;
        [XmlElement("FrameColor")] public ColorRoot FrameColor;
    }

    public class EmotionCardColorOptionRoot
    {
        [XmlElement("FrameHSVColor")] public HsvColorRoot FrameHSVColor;
        [XmlElement("FrameColor")] public ColorRoot FrameColor;


        [XmlElement("TextColor")] public ColorRoot TextColor;
    }

    public class HSVColor
    {
        public HSVColor(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }
    }

    public static class HSVColors
    {
        public static HSVColor Black = new HSVColor(0, 0, 0.05f);
        public static HSVColor White = new HSVColor(0, 0, 1.5f);
        public static HSVColor Purple = new HSVColor(60, 1, 1);
        public static HSVColor Blue = new HSVColor(145, 1, 1);
        public static HSVColor Green = new HSVColor(220, 1, 1);
        public static HSVColor Yellow = new HSVColor(305, 1.5f, 1.2f);
    }
}