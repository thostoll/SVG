using System;
using System.Drawing;

namespace Svg.Filter_Effects.feGaussianBlur
{
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    [SvgElement("feGaussianBlur")]
    public class SvgGaussianBlur : SvgFilterPrimitive
    {
        private float _stdDeviation;

        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;

        public SvgGaussianBlur()
            : this(1, BlurType.Both)
        {
        }

        public SvgGaussianBlur(float stdDeviation)
            : this(stdDeviation, BlurType.Both)
        {
        }

        public SvgGaussianBlur(float stdDeviation, BlurType blurType)
        {
            _stdDeviation = stdDeviation;
            BlurType = blurType;
            PreCalculate();
        }



        private void PreCalculate()
        {
            var sz = (int)(_stdDeviation * 2 + 1);
            _kernel = new int[sz];
            _multable = new int[sz, 256];
            for (var i = 1; i <= _stdDeviation; i++)
            {
                var szi = (int)(_stdDeviation - i);
                var szj = (int)(_stdDeviation + i);
                _kernel[szj] = _kernel[szi] = (szi + 1) * (szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (var j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj] * j;
                }
            }
            _kernel[(int)_stdDeviation] = (int)((_stdDeviation + 1) * (_stdDeviation + 1));
            _kernelSum += _kernel[(int)_stdDeviation];
            for (var j = 0; j < 256; j++)
            {
                _multable[(int)_stdDeviation, j] = _kernel[(int)_stdDeviation] * j;
            }
        }

        public Bitmap Apply(Image inputImage)
        {
            var bitmapSrc = inputImage as Bitmap ?? new Bitmap(inputImage);

            using (var src = new RawBitmap(bitmapSrc))
            {
                using (var dest = new RawBitmap(new Bitmap(inputImage.Width, inputImage.Height)))
                {
                    var pixelCount = src.Width * src.Height;
                    var b = new int[pixelCount];
                    var g = new int[pixelCount];
                    var r = new int[pixelCount];
                    var a = new int[pixelCount];

                    var b2 = new int[pixelCount];
                    var g2 = new int[pixelCount];
                    var r2 = new int[pixelCount];
                    var a2 = new int[pixelCount];

                    var ptr = 0;
                    for (var i = 0; i < pixelCount; i++)
                    {
                        b[i] = src.ArgbValues[ptr];
                        g[i] = src.ArgbValues[++ptr];
                        r[i] = src.ArgbValues[++ptr];
                        a[i] = src.ArgbValues[++ptr];
                        ptr++;
                    }

                    int bsum;
                    int gsum;
                    int rsum;
                    int asum;
                    int read;

                    var start = 0;
                    var index = 0;
                    if (BlurType != BlurType.VerticalOnly)
                    {
                        for (var i = 0; i < pixelCount; i++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = (int)(i - _stdDeviation);
                            for (var z = 0; z < _kernel.Length; z++)
                            {
                                if (read < start)
                                {
                                    ptr = start;
                                }
                                else if (read > start + src.Width - 1)
                                {
                                    ptr = start + src.Width - 1;
                                }
                                else
                                {
                                    ptr = read;
                                }
                                bsum += _multable[z, b[ptr]];
                                gsum += _multable[z, g[ptr]];
                                rsum += _multable[z, r[ptr]];
                                asum += _multable[z, a[ptr]];

                                ++read;
                            }
                            b2[i] = (bsum / _kernelSum);
                            g2[i] = (gsum / _kernelSum);
                            r2[i] = (rsum / _kernelSum);
                            a2[i] = (asum / _kernelSum);

                            if (BlurType == BlurType.HorizontalOnly)
                            {
                                dest.ArgbValues[index] = (byte)(bsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(gsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(rsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(asum / _kernelSum);
                                index++;
                            }

                            if (i > 0 && i % src.Width == 0)
                            {
                                start += src.Width;
                            }
                        }
                    }

                    if (BlurType == BlurType.HorizontalOnly)
                    {
                        return dest.Bitmap;
                    }

                    int tempy;
                    index = 0;
                    for (var i = 0; i < src.Height; i++)
                    {
                        var y = (int)(i - _stdDeviation);
                        start = y * src.Width;
                        for (var j = 0; j < src.Width; j++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = start + j;
                            tempy = y;
                            for (var z = 0; z < _kernel.Length; z++)
                            {
                                if (BlurType == BlurType.VerticalOnly)
                                {
                                    if (tempy < 0)
                                    {
                                        ptr = j;
                                    }
                                    else if (tempy > src.Height - 1)
                                    {
                                        ptr = pixelCount - (src.Width - j);
                                    }
                                    else
                                    {
                                        ptr = read;
                                    }
                                    bsum += _multable[z, b[ptr]];
                                    gsum += _multable[z, g[ptr]];
                                    rsum += _multable[z, r[ptr]];
                                    asum += _multable[z, a[ptr]];
                                }
                                else
                                {
                                    if (tempy < 0)
                                    {
                                        ptr = j;
                                    }
                                    else if (tempy > src.Height - 1)
                                    {
                                        ptr = pixelCount - (src.Width - j);
                                    }
                                    else
                                    {
                                        ptr = read;
                                    }
                                    bsum += _multable[z, b2[ptr]];
                                    gsum += _multable[z, g2[ptr]];
                                    rsum += _multable[z, r2[ptr]];
                                    asum += _multable[z, a2[ptr]];
                                }
                                read += src.Width;
                                ++tempy;
                            }

                            dest.ArgbValues[index] = (byte)(bsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(gsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(rsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(asum / _kernelSum);
                            index++;
                        }
                    }
                    return dest.Bitmap;
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius of the blur (only allows for one value - not the two specified in the SVG Spec)
        /// </summary>
        [SvgAttribute("stdDeviation")]
        public float StdDeviation
        {
            get => _stdDeviation;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Radius must be greater then 0");
                }
                _stdDeviation = value;
                PreCalculate();
            }
        }


        public BlurType BlurType { get; set; }


        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[Input];
            var result = Apply(inputImage);
            buffer[Result] = result;
        }



        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (!(base.DeepCopy<T>() is SvgGaussianBlur newObj)) return null;
            newObj.StdDeviation = StdDeviation;
            newObj.BlurType = BlurType;
            return newObj;

        }
    }
}