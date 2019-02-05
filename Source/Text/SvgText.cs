namespace Svg.Text
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("text")]
    public sealed class SvgText : SvgTextBase
    {
        /// <summary>
        /// Initializes the <see cref="SvgText"/> class.
        /// </summary>
        public SvgText()
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Svg.Text.SvgText" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SvgText(string text)
            : this()
        {
            Text = text;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgText>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (base.DeepCopy<T>() is SvgText newObj)
            {
                newObj.TextAnchor = TextAnchor;
                newObj.WordSpacing = WordSpacing;
                newObj.LetterSpacing = LetterSpacing;
                newObj.Font = Font;
                newObj.FontFamily = FontFamily;
                newObj.FontSize = FontSize;
                newObj.FontWeight = FontWeight;
                newObj.X = X;
                newObj.Y = Y;
                return newObj;
            }

            return null;
        }
    }
}