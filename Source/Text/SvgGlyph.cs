using System.Drawing.Drawing2D;
using System.Linq;
using Svg.Basic_Shapes;
using Svg.Paths;
using Svg.Rendering;

namespace Svg.Text
{
    [SvgElement("glyph")]
    public class SvgGlyph : SvgPathBasedElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d", true)]
        public SvgPathSegmentList PathData
        {
            get => Attributes.GetAttribute<SvgPathSegmentList>("d");
            set => Attributes["d"] = value;
        }

        [SvgAttribute("glyph-name", true)]
        public virtual string GlyphName
        {
            get => Attributes["glyph-name"] as string;
            set => Attributes["glyph-name"] = value;
        }
        [SvgAttribute("horiz-adv-x", true)]
        public float HorizAdvX
        {
            get => (float?) Attributes["horiz-adv-x"] ?? Parents.OfType<SvgFont>().First().HorizAdvX;
            set => Attributes["horiz-adv-x"] = value;
        }
        [SvgAttribute("unicode", true)]
        public string Unicode
        {
            get => Attributes["unicode"] as string;
            set => Attributes["unicode"] = value;
        }
        [SvgAttribute("vert-adv-y", true)]
        public float VertAdvY
        {
            get => (float?) Attributes["vert-adv-y"] ?? Parents.OfType<SvgFont>().First().VertAdvY;
            set => Attributes["vert-adv-y"] = value;
        }
        [SvgAttribute("vert-origin-x", true)]
        public float VertOriginX
        {
            get => (float?) Attributes["vert-origin-x"] ?? Parents.OfType<SvgFont>().First().VertOriginX;
            set => Attributes["vert-origin-x"] = value;
        }
        [SvgAttribute("vert-origin-y", true)]
        public float VertOriginY
        {
            get => (float?) Attributes["vert-origin-y"] ?? Parents.OfType<SvgFont>().First().VertOriginY;
            set => Attributes["vert-origin-y"] = value;
        }


        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path != null && !IsPathDirty) return _path;
            _path = new GraphicsPath();

            foreach (SvgPathSegment segment in PathData)
            {
                segment.AddToPath(_path);
            }

            IsPathDirty = false;
            return _path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGlyph"/> class.
        /// </summary>
        public SvgGlyph()
        {
            var pathData = new SvgPathSegmentList();
            Attributes["d"] = pathData;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGlyph>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGlyph;
            foreach (var pathData in PathData)
            {
                newObj?.PathData.Add(pathData.Clone());
            }

            return newObj;

        }
    }
}
