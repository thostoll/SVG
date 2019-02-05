using System;
using System.Collections.Generic;

namespace Svg
{
    public static class Extensions
    {
        public static IEnumerable<SvgElement> Descendants<T>(this IEnumerable<T> source) where T : SvgElement
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return GetDescendants(source, false);
        }
        private static IEnumerable<SvgElement> GetAncestors<T>(IEnumerable<T> source, bool self) where T : SvgElement
        {
            foreach (var start in source)
            {
                if (start != null)
                {
                    for (var elem = self ? start : start.Parent; elem != null; elem = (elem.Parent as SvgElement))
                    {
                        yield return elem;
                    }
                }
            }
            yield break;
        }
        private static IEnumerable<SvgElement> GetDescendants<T>(IEnumerable<T> source, bool self) where T : SvgElement
        {
            var positons = new Stack<int>();
            foreach (var start in source)
            {
                if (start != null)
                {
                    if (self) yield return start;

                    positons.Push(0);
                    SvgElement currParent = start;

                    while (positons.Count > 0)
                    {
                        var currPos = positons.Pop();
                        if (currPos < currParent.Children.Count)
                        {
                            yield return currParent.Children[currPos];
                            currParent = currParent.Children[currPos];
                            positons.Push(currPos + 1);
                            positons.Push(0);
                        }
                        else
                        {
                            currParent = currParent.Parent;
                        }
                    }
                }
            }
            yield break;
        }
    }
}