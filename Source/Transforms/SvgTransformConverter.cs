using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Svg.Transforms
{
    public class SvgTransformConverter : TypeConverter
    {
        private static IEnumerable<string> SplitTransforms(string transforms)
        {
            var transformEnd = 0;

            for (var i = 0; i < transforms.Length; i++)
            {
                if (transforms[i] != ')') continue;
                yield return transforms.Substring(transformEnd, i - transformEnd + 1).Trim();
                while (i < transforms.Length && !char.IsLetter(transforms[i])) i++;
                transformEnd = i;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string)) return base.ConvertFrom(context, culture, value);
            var transformList = new SvgTransformCollection();

            foreach (var transform in SplitTransforms((string)value))
            {
                if (string.IsNullOrEmpty(transform))
                    continue;

                var parts = transform.Split('(', ')');
                var transformName = parts[0].Trim();
                var contents = parts[1].Trim();

                switch (transformName)
                {
                    case "translate":
                        var coords = contents.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (coords.Length == 0 || coords.Length > 2)
                        {
                            throw new FormatException("Translate transforms must be in the format 'translate(x [,y])'");
                        }

                        var x = float.Parse(coords[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                        if (coords.Length > 1)
                        {
                            float y = float.Parse(coords[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            transformList.Add(new SvgTranslate(x, y));
                        }
                        else
                        {
                            transformList.Add(new SvgTranslate(x));
                        }
                        break;
                    case "rotate":
                        var args = contents.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (args.Length != 1 && args.Length != 3)
                        {
                            throw new FormatException("Rotate transforms must be in the format 'rotate(angle [cx cy ])'");
                        }

                        var angle = float.Parse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture);

                        if (args.Length == 1)
                        {
                            transformList.Add(new SvgRotate(angle));
                        }
                        else
                        {
                            var cx = float.Parse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture);
                            var cy = float.Parse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture);

                            transformList.Add(new SvgRotate(angle, cx, cy));
                        }
                        break;
                    case "scale":
                        var scales = contents.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (scales.Length == 0 || scales.Length > 2)
                        {
                            throw new FormatException("Scale transforms must be in the format 'scale(x [,y])'");
                        }

                        var sx = float.Parse(scales[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);

                        if (scales.Length > 1)
                        {
                            var sy = float.Parse(scales[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            transformList.Add(new SvgScale(sx, sy));
                        }
                        else
                        {
                            transformList.Add(new SvgScale(sx));
                        }

                        break;
                    case "matrix":
                        var points = contents.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (points.Length != 6)
                        {
                            throw new FormatException("Matrix transforms must be in the format 'matrix(m11, m12, m21, m22, dx, dy)'");
                        }

                        var mPoints = points.Select(point => float.Parse(point.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture)).ToList();

                        transformList.Add(new SvgMatrix(mPoints));
                        break;
                    case "shear":
                        var shears = contents.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (shears.Length == 0 || shears.Length > 2)
                        {
                            throw new FormatException("Shear transforms must be in the format 'shear(x [,y])'");
                        }

                        var hx = float.Parse(shears[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);

                        if (shears.Length > 1)
                        {
                            var hy = float.Parse(shears[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            transformList.Add(new SvgShear(hx, hy));
                        }
                        else
                        {
                            transformList.Add(new SvgShear(hx));
                        }

                        break;
                    case "skewX":
                        var ax = float.Parse(contents, NumberStyles.Float, CultureInfo.InvariantCulture);
                        transformList.Add(new SvgSkew(ax, 0));
                        break;
                    case "skewY":
                        var ay = float.Parse(contents, NumberStyles.Float, CultureInfo.InvariantCulture);
                        transformList.Add(new SvgSkew(0, ay));
                        break;
                }
            }

            return transformList;

        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);

            if (value is SvgTransformCollection transforms)
            {
                return string.Join(" ", transforms.Select(t => t.WriteToString()).ToArray());
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
