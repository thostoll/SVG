
using Svg.ExCSS;

namespace Svg.External.ExCSS.Model.TextBlocks
{
    internal class MatchBlock : Block
    {
        internal static readonly MatchBlock Include = new MatchBlock { GrammarSegment = GrammarSegment.IncludeMatch };
        internal static readonly MatchBlock Dash = new MatchBlock { GrammarSegment = GrammarSegment.DashMatch };
        internal static readonly Block Prefix = new MatchBlock { GrammarSegment = GrammarSegment.PrefixMatch };
        internal static readonly Block Substring = new MatchBlock { GrammarSegment = GrammarSegment.SubstringMatch };
        internal static readonly Block Suffix = new MatchBlock { GrammarSegment = GrammarSegment.SuffixMatch };
        internal static readonly Block Not = new MatchBlock { GrammarSegment = GrammarSegment.NegationMatch };

        public override string ToString()
        {
            switch (GrammarSegment)
            {
                case GrammarSegment.SubstringMatch:
                    return "*=";

                case GrammarSegment.SuffixMatch:
                    return "$=";

                case GrammarSegment.PrefixMatch:
                    return "^=";

                case GrammarSegment.IncludeMatch:
                    return "~=";

                case GrammarSegment.DashMatch:
                    return "|=";

                case GrammarSegment.NegationMatch:
                    return "!=";
            }

            return string.Empty;
        }
    }
}
