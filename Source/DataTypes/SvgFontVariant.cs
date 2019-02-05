using System.ComponentModel;
using Svg.Painting;

namespace Svg.DataTypes
{
    [TypeConverter(typeof(SvgFontVariantConverter))]
    public enum SvgFontVariant
    {
        Normal,
        Smallcaps,
        Inherit
    }
}
