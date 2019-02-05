using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.DataTypes;
using Svg.Painting;
using Svg.Rendering;

namespace Svg.Basic_Shapes
{
    /// <inheritdoc />
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    [SvgElement("line")]
    public class SvgLine : SvgMarkerElement
    {
        private SvgUnit _startX;
        private SvgUnit _startY;
        private SvgUnit _endX;
        private SvgUnit _endY;
        private GraphicsPath _path;

        [SvgAttribute("x1")]
        public SvgUnit StartX
        {
            get => _startX;
            set 
            { 
            	if(_startX != value)
            	{
            		_startX = value;
            		IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x1", Value = value });
            	}
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get => _startY;
            set 
            { 
            	if(_startY != value)
            	{
            		_startY = value;
            		IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y1", Value = value });
            	}
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get => _endX;
            set 
            { 
            	if(_endX != value)
            	{
            		_endX = value;
            		IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x2", Value = value });
            	}
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get => _endY;
            set 
            { 
            	if(_endY != value)
            	{
            		_endY = value;
            		IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y2", Value = value });
            	}
            }
        }

        public override SvgPaintServer Fill
        {
            get => null;
            set
            {
                // Do nothing
            }
        }

        public SvgLine()
        {
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((_path == null || IsPathDirty) && base.StrokeWidth > 0)
            {
                PointF start = new PointF(StartX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                          StartY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                PointF end = new PointF(EndX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                        EndY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                _path = new GraphicsPath();

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                  _path.AddLine(start, end);
                  IsPathDirty = false;
                }
                else
                {	 // only when calculating boundary 
                  _path.StartFigure();
                  var radius = base.StrokeWidth / 2;
                  _path.AddEllipse(start.X - radius, start.Y - radius, 2 * radius, 2 * radius);
                  _path.AddEllipse(end.X - radius, end.Y - radius, 2 * radius, 2 * radius);
                  _path.CloseFigure();
                }
            }
            return _path;
        }

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgLine>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgLine;
			newObj.StartX = StartX;
			newObj.EndX = EndX;
			newObj.StartY = StartY;
			newObj.EndY = EndY;
			if (Fill != null)
				newObj.Fill = Fill.DeepCopy() as SvgPaintServer;

			return newObj;
		}

    }
}
