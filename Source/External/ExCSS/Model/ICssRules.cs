using System.Collections.Generic;
using Svg.ExCSS;

namespace Svg.External.ExCSS.Model
{
    interface ISupportsRuleSets
    {
        List<RuleSet> RuleSets { get; }
    }
}