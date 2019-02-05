using System;

namespace Svg.Text
{
    [SvgElement("font-face-uri")]
    public class SvgFontFaceUri : SvgElement
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement { get; set; }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFontFaceUri>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (!(base.DeepCopy<T>() is SvgFontFaceUri newObj)) return null;
            newObj.ReferencedElement = ReferencedElement;

            return newObj;

        }
    }
}
