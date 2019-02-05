namespace Svg.Text
{
    [SvgElement("font-face-src")]
    public class SvgFontFaceSrc : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFaceSrc>();
        }
    }
}
