﻿using System;
using System.Drawing.Drawing2D;
using Svg.DataTypes;
using Svg.Paths;
using Svg.Rendering;

namespace Svg.Text
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("textPath")]
    public class SvgTextPath : SvgTextBase
    {
        private Uri _referencedPath;

        public override SvgUnitCollection Dx
        {
            get { return null; }
            set { /* do nothing */ }
        }

        [SvgAttribute("startOffset")]
        public virtual SvgUnit StartOffset
        {
            get => (_dx.Count < 1 ? SvgUnit.None : _dx[0]);
            set 
            {
                if (_dx.Count < 1)
                {
                    _dx.Add(value);
                }
                else
                {
                    _dx[0] = value;
                }
            }
        }

        [SvgAttribute("method")]
        public virtual SvgTextPathMethod Method
        {
            get { return (Attributes["method"] == null ? SvgTextPathMethod.Align : (SvgTextPathMethod)Attributes["method"]); }
            set { Attributes["method"] = value; }
        }

        [SvgAttribute("spacing")]
        public virtual SvgTextPathSpacing Spacing
        {
            get { return (Attributes["spacing"] == null ? SvgTextPathSpacing.Exact : (SvgTextPathSpacing)Attributes["spacing"]); }
            set { Attributes["spacing"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedPath
        {
            get { return _referencedPath; }
            set { _referencedPath = value; }
        }

        protected override GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            var path = OwnerDocument.IdManager.GetElementById(ReferencedPath) as Basic_Shapes.SvgVisualElement;
            if (path == null) return null;
            var pathData = (GraphicsPath)path.Path(renderer).Clone();
            if (path.Transforms.Count > 0)
            {
                Matrix transformMatrix = new Matrix(1, 0, 0, 1, 0, 0);

                foreach (var transformation in path.Transforms)
                {
                    transformMatrix.Multiply(transformation.Matrix);
                }

                pathData.Transform(transformMatrix);
            }
            return pathData;
        }
        protected override float GetAuthorPathLength()
        {
            var path = OwnerDocument.IdManager.GetElementById(ReferencedPath) as SvgPath;
            if (path == null) return 0;
            return path.PathLength;
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgTextPath>();
        }

        


    }
}
