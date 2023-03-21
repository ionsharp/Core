using System;
using System.Linq.Expressions;

namespace Imagin.Core.Linq;

public static class XExpression
{
    public static string GetMemberName<T>(this Expression<Func<T>> input) => input.Body.As<MemberExpression>().Member.Name;
}