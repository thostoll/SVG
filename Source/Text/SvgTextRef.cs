using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg.Text
{
    [SvgElement("tref")]
    public class SvgTextRef : SvgTextBase
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement { get; set; }

        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = OwnerDocument.IdManager.GetElementById(ReferencedElement) as SvgTextBase;
            IEnumerable<ISvgNode> contentNodes;

            if (refText == null)
            {
                contentNodes = base.GetContentNodes();
            }
            else
            {
                contentNodes = refText.GetContentNodes();
            }

            contentNodes = contentNodes.Where(o => !(o is ISvgDescriptiveElement));

            return contentNodes;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (!(base.DeepCopy<T>() is SvgTextRef newObj)) return null;
            newObj.X = X;
            newObj.Y = Y;
            newObj.Dx = Dx;
            newObj.Dy = Dy;
            newObj.Text = Text;
            newObj.ReferencedElement = ReferencedElement;

            return newObj;

        }


    }
}
