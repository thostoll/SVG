namespace Svg
{
    public class SvgContentNode : ISvgNode
    {
        public string Content { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Create a deep copy of this <see cref="T:Svg.ISvgNode" />.
        /// </summary>
        /// <returns>A deep copy of this <see cref="T:Svg.ISvgNode" /></returns>
        public ISvgNode DeepCopy()
        {
            // Since strings are immutable in C#, we can just use the same reference here.
            return new SvgContentNode { Content = Content };
        }
    }
}
