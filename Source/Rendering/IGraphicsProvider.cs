using System.Drawing;

namespace Svg.Rendering
{
    public interface IGraphicsProvider
    {
        Graphics GetGraphics();
    }
}
