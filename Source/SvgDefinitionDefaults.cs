using System.Collections.Generic;

namespace Svg
{
    /// <summary>
    /// Holds a dictionary of the default values of the SVG specification 
    /// </summary>
    public static class SvgDefaults
    {
        //internal dictionary for the defaults
        private static readonly Dictionary<string, string> Defaults = new Dictionary<string, string>();

        static SvgDefaults()
        {
            Defaults["d"] = "";

            Defaults["viewBox"] = "0, 0, 0, 0";

            Defaults["visibility"] = "visible";
            Defaults["opacity"] = "1";
            Defaults["clip-rule"] = "nonzero";

            Defaults["transform"] = "";
            Defaults["rx"] = "0";
            Defaults["ry"] = "0";
            Defaults["cx"] = "0";
            Defaults["cy"] = "0";

            Defaults["fill"] = "";
            Defaults["fill-opacity"] = "1";
            Defaults["fill-rule"] = "nonzero";

            Defaults["stroke"] = "none";
            Defaults["stroke-opacity"] = "1";
            Defaults["stroke-width"] = "1";
            Defaults["stroke-miterlimit"] = "4";
            Defaults["stroke-linecap"] = "butt";
            Defaults["stroke-linejoin"] = "miter";
            Defaults["stroke-dasharray"] = "none";
            Defaults["stroke-dashoffset"] = "0";
        }

        /// <summary>
        /// Checks whether the property value is the default value of the svg definition.
        /// </summary>
        /// <param name="attributeName">Name of the svg attribute</param>
        /// <param name="value">.NET value of the attribute</param>
        public static bool IsDefault(string attributeName, string value)
        {
            if (!Defaults.ContainsKey(attributeName)) return false;
            return Defaults[attributeName] == value;
        }
    }
}
