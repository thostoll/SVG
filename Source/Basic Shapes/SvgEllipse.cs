using System.Drawing.Drawing2D;
using Svg.DataTypes;
using Svg.Rendering;

namespace Svg.Basic_Shapes
{
    /// <inheritdoc />
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    [SvgElement("ellipse")]
    public class SvgEllipse : SvgPathBasedElement
    {
        private SvgUnit _radiusX;
        private SvgUnit _radiusY;
        private SvgUnit _centerX;
        private SvgUnit _centerY;
        private GraphicsPath _path;

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get => _centerX;
            set
            {
            	if(_centerX != value)
            	{
            		_centerX = value;
            		IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "cx", Value = value });
            	}
            }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
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

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
        	get => _radiusX;
            set
        	{
                if (_radiusX == value) return;
                _radiusX = value;
                IsPathDirty = true;
                OnAttributeChanged(new AttributeEventArgs{ Attribute = "rx", Value = value });
            }
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
        	get => _radiusY;
            set
        	{
                if (_radiusY == value) return;
                _radiusY = value;
                IsPathDirty = true;
                OnAttributeChanged(new AttributeEventArgs{ Attribute = "ry", Value = value });
            }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path != null && !IsPathDirty) return _path;
            var halfStrokeWidth = base.StrokeWidth / 2;

            // If it is to render, don't need to consider stroke width.
            // i.e stroke width only to be considered when calculating boundary
            if (renderer != null)
            {
                halfStrokeWidth = 0;
                IsPathDirty = false;
            }

            var center = SvgUnit.GetDevicePoint(_centerX, _centerY, renderer, this);
            var radiusX = RadiusX.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
            var radiusY = RadiusY.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;

            _path = new GraphicsPath();
            _path.StartFigure();
            _path.AddEllipse(center.X - radiusX, center.Y - radiusY, 2 * radiusX, 2 * radiusY);
            _path.CloseFigure();
            return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object used for rendering.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (_radiusX.Value > 0.0f && _radiusY.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgEllipse"/> class.
        /// </summary>
        public SvgEllipse()
        {
        }



		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgEllipse>();
		}

		public override SvgElement DeepCopy<T>()
		{
            if (base.DeepCopy<T>() is SvgEllipse newObj)
            {
                newObj.CenterX = CenterX;
                newObj.CenterY = CenterY;
                newObj.RadiusX = RadiusX;
                newObj.RadiusY = RadiusY;
                return newObj;
            }

            return null;
        }
    }
}