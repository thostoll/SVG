using System;
using System.Collections.Generic;
using Svg;
using System.IO;

namespace Entities
{
    internal class Program
    {
        private static void Main()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

            var sampleDoc = SvgDocument.Open(filePath, new Dictionary<string, string> 
                {
                    {"entity1", "fill:red" },
                    {"entity2", "fill:yellow" }
                });

            sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.png"));
        }
    }
}
