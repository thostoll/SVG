using System;
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgScale : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                var matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Scale(X, Y);
                return matrix;
            }
        }

        public override string WriteToString()
        {
            return Math.Abs(X - Y) < 1 ? string.Format(CultureInfo.InvariantCulture, "scale({0})", X) : string.Format(CultureInfo.InvariantCulture, "scale({0}, {1})", X, Y);
        }

        public SvgScale(float x) : this(x, x) { }

        public SvgScale(float x, float y)
        {
            X = x;
            Y = y;
        }

		public override object Clone()
		{
			return new SvgScale(X, Y);
		}
    }
}
