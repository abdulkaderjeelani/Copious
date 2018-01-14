using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NReco.Linq;

namespace Copious.SharedKernel
{
    public class RuleAssessor : IRuleAssessor
    {
        readonly LambdaParser _parser;

        public RuleAssessor()
        {
            _parser = new LambdaParser();
        }

        public Expression AsExpression(string rule)
        => _parser.Parse(rule);

        public object Assess(string rule, Func<string, object> valueProvider)
        => _parser.Eval(rule, valueProvider);

        public object Assess(string rule, IDictionary<string, object> values)
        => _parser.Eval(rule, values);

        public object Assess(string rule)
            => _parser.Eval(rule, default(Dictionary<string, object>));
    }
}