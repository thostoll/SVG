using System;

namespace Svg
{
    /// <inheritdoc />
    /// <summary>
    /// Specifies the SVG name of an <see cref="T:Svg.SvgElement" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SvgElementAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the SVG element.
        /// </summary>
        public string ElementName { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Svg.SvgElementAttribute" /> class with the specified element name;
        /// </summary>
        /// <param name="elementName">The name of the SVG element.</param>
        public SvgElementAttribute(string elementName)
        {
            ElementName = elementName;
        }
    }
}