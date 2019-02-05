using System.ComponentModel;
using Svg.Painting;

namespace Svg.Filter_Effects.feColourMatrix
{
    [TypeConverter(typeof(EnumBaseConverter<SvgColourMatrixType>))]
	public enum SvgColourMatrixType
	{
		Matrix,
		Saturate,
		HueRotate,
		LuminanceToAlpha
	}
}
