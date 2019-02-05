namespace Svg.Text
{
    [SvgElement("tspan")]
    public class SvgTextSpan : SvgTextBase
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextSpan>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (!(base.DeepCopy<T>() is SvgTextSpan newObj)) return null;
            newObj.X = X;
            newObj.Y = Y;
            newObj.Dx = Dx;
            newObj.Dy = Dy;
            newObj.Text = Text;

            return newObj;

        }


    }
}