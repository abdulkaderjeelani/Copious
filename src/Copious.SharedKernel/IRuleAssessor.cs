using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Copious.SharedKernel
{
    public interface IRuleAssessor
    {
        object Assess(string rule, Func<string, object> valueProvider);

        object Assess(string rule, IDictionary<string, object> values);

        object Assess(string rule);

        Expression AsExpression(string rule);
    }
}