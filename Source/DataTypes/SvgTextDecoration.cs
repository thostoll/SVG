﻿using System;
using System.ComponentModel;
using Svg.Painting;

namespace Svg.DataTypes
{
    /// <summary>This property describes decorations that are added to the text of an element. Conforming SVG Viewers are not required to support the blink value.</summary>
    [TypeConverter(typeof(SvgTextDecorationConverter))]
    [Flags]
    public enum SvgTextDecoration
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit = 0,

        /// <summary>The text is not decorated</summary>
        None = 1,

        /// <summary>The text is underlined.</summary>
        Underline = 2,

        /// <summary>The text is overlined.</summary>
        Overline = 4,

        /// <summary>The text is struck through.</summary>
        LineThrough = 8,

        /// <summary>The text will blink.</summary>
        Blink = 16
    }
}
