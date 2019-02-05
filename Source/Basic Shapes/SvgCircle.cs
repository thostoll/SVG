using System.Drawing.Drawing2D;
using Svg.DataTypes;
using Svg.Rendering;

namespace Svg.Basic_Shapes
{
    /// <inheritdoc />
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    [SvgElement("circle")]
    public sealed class SvgCircle : SvgPathBasedElement
    {
        private GraphicsPath _path;
        
        private SvgUnit _radius;
        private SvgUnit _centerX;
        private SvgUnit _centerY;

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public SvgPoint Center => new SvgPoint(CenterX, CenterY);

        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get => _centerX;
            set
            {
                if (_centerX == value) return;
                _centerX = value;
                IsPathDirty = true;
                OnAttributeChanged(new AttributeEventArgs{ Attribute = "cx", Value = value });
            }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
        	get => _centerY;
            set
        	{
                if (_centerY == value) return;
                _centerY = value;
                IsPathDirty = true;
                OnAttributeChanged(new AttributeEventArgs{ Attribute = "cy", Value = value });
            }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
        	get => _radius;
            set
        	{
                if (_radius == value) return;
                _radius = value;
                IsPathDirty = true;
                OnAttributeChanged(new AttributeEventArgs{ Attribute = "r", Value = value });
            }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> representing this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path != null && !IsPathDirty) return _path;
            var halfStrokeWidth = StrokeWidth / 2;

            // If it is to render, don't need to consider stroke width.
            // i.e stroke width only to be considered when calculating boundary
            if (renderer != null)
            {
                halfStrokeWidth = 0;
                IsPathDirty = false;
            }

            _path = new GraphicsPath();
            _path.StartFigure();
            var center = Center.ToDeviceValue(renderer, this);
            var radius = Radius.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
            _path.AddEllipse(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
            _path.CloseFigure();
            return _path;
        }

        /// <summary>
        /// Renders the circle using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The renderer object.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Don't draw if there is no radius set
            if (Radius.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgCircle"/> class.
        /// </summary>
        public SvgCircle()
        {
            CenterX = new SvgUnit(0.0f);
            CenterY = new SvgUnit(0.0f);
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgCircle>();
		}

		public override SvgElement DeepCopy<T>()
		{
            if (!(base.DeepCopy<T>() is SvgCircle newObj)) return null;
            newObj.CenterX = CenterX;
            newObj.CenterY = CenterY;
            newObj.Radius = Radius;
            return newObj;

        }
    }
}