using System.ComponentModel;

namespace Svg.Painting
{
	[TypeConverter(typeof(SvgFillRuleConverter))]
    public enum SvgFillRule
    {
        NonZero,
        EvenOdd,
        Inherit
    }
}