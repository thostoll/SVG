﻿using System;
using System.ComponentModel;
using Svg.Painting;

namespace Svg.DataTypes
{
    /// <summary>This is the descriptor for the style of a font and takes the same values as the 'font-style' property, except that a comma-separated list is permitted.</summary>
    [TypeConverter(typeof(SvgFontStyleConverter))]
    [Flags]
    public enum SvgFontStyle
    {
        /// <summary>Indicates that the font-face supplies all styles (normal, oblique and italic).</summary>
        All = (Normal | Oblique | Italic),

        /// <summary>Specifies a font that is classified as 'normal' in the UA's font database.</summary>
        Normal = 1,

        /// <summary>Specifies a font that is classified as 'oblique' in the UA's font database. Fonts with Oblique, Slanted, or Incline in their names will typically be labeled 'oblique' in the font database. A font that is labeled 'oblique' in the UA's font database may actually have been generated by electronically slanting a normal font.</summary>
        Oblique = 2,

        /// <summary>Specifies a font that is classified as 'italic' in the UA's font database, or, if that is not available, one labeled 'oblique'. Fonts with Italic, Cursive, or Kursiv in their names will typically be labeled 'italic'</summary>
        Italic = 4
    }
}
