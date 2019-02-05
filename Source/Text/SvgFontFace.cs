using Svg.DataTypes;

namespace Svg.Text
{
    [SvgElement("font-face")]
    public class SvgFontFace : SvgElement
    {
        [SvgAttribute("alphabetic")]
        public float Alphabetic
        {
            get => (Attributes["alphabetic"] == null ? 0 : (float)Attributes["alphabetic"]);
            set => Attributes["alphabetic"] = value;
        }

        [SvgAttribute("ascent")]
        public float Ascent
        {
            get 
            { 
                if (Attributes["ascent"] == null) 
                {
                    var font = Parent as SvgFont;
                    return (font == null ? 0 : UnitsPerEm - font.VertOriginY);
                }
                else
                {
                    return (float)Attributes["ascent"];
                }
            }
            set => Attributes["ascent"] = value;
        }

        [SvgAttribute("ascent-height")]
        public float AscentHeight
        {
            get => (Attributes["ascent-height"] == null ? Ascent : (float)Attributes["ascent-height"]);
            set => Attributes["ascent-height"] = value;
        }

        [SvgAttribute("descent")]
        public float Descent
        {
            get 
            { 
                if (Attributes["descent"] == null) 
                {
                    var font = Parent as SvgFont;
                    return font?.VertOriginY ?? 0;
                }
                else 
                {
                    return (float)Attributes["descent"];
                }
            }
            set => Attributes["descent"] = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get => Attributes["font-family"] as string;
            set => Attributes["font-family"] = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get => (SvgUnit?) Attributes["font-size"] ?? SvgUnit.Empty;
            set => Attributes["font-size"] = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Refers to the style of the font.
        /// </summary>
        [SvgAttribute("font-style")]
        public override SvgFontStyle FontStyle
        {
            get => (SvgFontStyle?) Attributes["font-style"] ?? SvgFontStyle.All;
            set => Attributes["font-style"] = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Refers to the varient of the font.
        /// </summary>
        [SvgAttribute("font-variant")]
        public override SvgFontVariant FontVariant
        {
            get => (SvgFontVariant?) Attributes["font-variant"] ?? SvgFontVariant.Inherit;
            set => Attributes["font-variant"] = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public override SvgFontWeight FontWeight
        {
            get => (SvgFontWeight?) Attributes["font-weight"] ?? SvgFontWeight.Inherit;
            set => Attributes["font-weight"] = value;
        }

        [SvgAttribute("panose-1")]
        public string Panose1
        {
            get => Attributes["panose-1"] as string;
            set => Attributes["panose-1"] = value;
        }

        [SvgAttribute("units-per-em")]
        public float UnitsPerEm
        {
            get => (float?) Attributes["units-per-em"] ?? 1000;
            set => Attributes["units-per-em"] = value;
        }

        [SvgAttribute("x-height")]
        public float XHeight
        {
            get => (float?) Attributes["x-height"] ?? float.MinValue;
            set => Attributes["x-height"] = value;
        }


        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFace>();
        }
    }
}
