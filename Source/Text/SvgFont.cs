using System.Linq;
using Svg.Rendering;

namespace Svg.Text
{
    [SvgElement("font")]
    public class SvgFont : SvgElement
    {
        [SvgAttribute("horiz-adv-x")]
        public float HorizAdvX
        {
            get => (float?) Attributes["horiz-adv-x"] ?? 0;
            set => Attributes["horiz-adv-x"] = value;
        }
        [SvgAttribute("horiz-origin-x")]
        public float HorizOriginX
        {
            get => (float?) Attributes["horiz-origin-x"] ?? 0;
            set => Attributes["horiz-origin-x"] = value;
        }
        [SvgAttribute("horiz-origin-y")]
        public float HorizOriginY
        {
            get => (float?) Attributes["horiz-origin-y"] ?? 0;
            set => Attributes["horiz-origin-y"] = value;
        }
        [SvgAttribute("vert-adv-y")]
        public float VertAdvY
        {
            get => (float?) Attributes["vert-adv-y"] ?? Children.OfType<SvgFontFace>().First().UnitsPerEm;
            set => Attributes["vert-adv-y"] = value;
        }
        [SvgAttribute("vert-origin-x")]
        public float VertOriginX
        {
            get => (float?) Attributes["vert-origin-x"] ?? HorizAdvX / 2;
            set => Attributes["vert-origin-x"] = value;
        }
        [SvgAttribute("vert-origin-y")]
        public float VertOriginY
        {
            get =>
                (float?) Attributes["vert-origin-y"] ?? (Children.OfType<SvgFontFace>().First().Attributes["ascent"] == null ? 0 : Children.OfType<SvgFontFace>().First().Ascent);
            set => Attributes["vert-origin-y"] = value;
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFont>();
        }

        protected override void Render(ISvgRenderer renderer)
        { }
	}
}
