namespace Svg.Text
{
    public abstract class SvgKern : SvgElement
    {
        [SvgAttribute("g1")]
        public string Glyph1
        {
            get => Attributes["g1"] as string;
            set => Attributes["g1"] = value;
        }
        [SvgAttribute("g2")]
        public string Glyph2
        {
            get => Attributes["g2"] as string;
            set => Attributes["g2"] = value;
        }
        [SvgAttribute("u1")]
        public string Unicode1
        {
            get => Attributes["u1"] as string;
            set => Attributes["u1"] = value;
        }
        [SvgAttribute("u2")]
        public string Unicode2
        {
            get => Attributes["u2"] as string;
            set => Attributes["u2"] = value;
        }
        [SvgAttribute("k")]
        public float Kerning
        {
            get => (float?) Attributes["k"] ?? 0;
            set => Attributes["k"] = value;
        }
    }

    [SvgElement("vkern")]
    public class SvgVerticalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgVerticalKern>();
        }
    }
    [SvgElement("hkern")]
    public class SvgHorizontalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgHorizontalKern>();
        }
    }
}
