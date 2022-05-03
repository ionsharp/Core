using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Specifies a string of zeros and ones where zeros correspond to <see langword="false"/> and ones correspond to <see langword="true"/>;
    /// operators are specified by commas (<see langword="and"/>) and pipes (<see langword="or"/>). Unlimited depth is possible using parenthesis.
    /// Example: 1,(1|1,(0,1|(1,0))|(1|0,1)) = <see langword="if"/> (<see langword="true"/> and (<see langword="true"/> or <see langword="true"/>)).
    /// See <see cref="Evaluate(IList{bool})"/> to evaluate against a list of values. Note: All commas are replaced with ampersands on instance creation.
    /// </summary>
    public struct StringCondition
    {
        readonly string Data;

        public StringCondition(string input) => Data = input.Replace(",", "&");

        //...

        bool ConvertOperator(char input) => input == '|';

        int FirstIndex(string input, char find)
        {
            var result = 0;
            foreach (var i in input)
            {
                if (i == find)
                    return result;

                result++;
            }
            return -1;
        }

        Tuple<int, string> GetInnerMostExpression(string input)
        {
            var cFirst = FirstIndex(input, ')');
            var oFirst = -1;

            var length = 0;
            if (cFirst > -1)
            {
                for (var i = cFirst; i >= 0; i--)
                {
                    length++;
                    if (input[i] == '(')
                    {
                        oFirst = i;
                        break;
                    }
                }
            }

            if (cFirst == -1 || oFirst == -1)
                return null;

            return Tuple.Create(oFirst, input.Substring(oFirst, length));
        }

        bool Digit(char i) => i == '0' || i == '1';

        int Count(string input)
        {
            var result = 0;
            foreach (var i in input)
            {
                if (Digit(i))
                    result++;
            }
            return result;
        }

        int Past(string input, int index)
        {
            var result = 0;

            var j = 0;
            foreach (var i in input)
            {
                if (j == index)
                    break;

                if (Digit(i))
                    result++;

                j++;
            }
            return result;
        }

        string Solve(string input, IList<bool> values)
        {
            var result = false;

            bool? @operator = default;

            var index = 0;
            foreach (var i in input)
            {
                switch (i)
                {
                    case '0':
                        result =
                            @operator == default
                            ? values[index]
                            : (@operator.Value ? result || !values[index] : result && !values[index]);

                        index++;
                        break;

                    case '1':
                        result =
                            @operator == default
                            ? values[index]
                            : (@operator.Value ? result || values[index] : result && values[index]);

                        index++;
                        break;

                    case '&':
                    case '|':
                        @operator = ConvertOperator(i);
                        break;

                    default:
                        throw new InvalidOperationException($"'{i}' is invalid. '{typeof(StringCondition).FullName}.{nameof(Data)}' may only contain '0', '1', ',', '|', '(', ')'.");
                }
            }
            return result ? "1" : "0";
        }

        //...

        public bool Evaluate(IList<bool> values)
        {
            //1. Solve all inner expressions...
            var result = Data;
            while (GetInnerMostExpression(result) is Tuple<int, string> i)
            {
                var expression = i.Item2.Replace("(", "").Replace(")", "");

                var rIndex = Past(result, i.Item1);
                var rCount = Count(expression);

                var innerValues = values.As<List<bool>>().GetRange(rIndex, rCount);

                var nResult = Solve(expression, innerValues);
                var nIndex = i.Item1 + i.Item2.Length;

                if (nResult == "0")
                    return false;

                result = $"{result.Substring(0, i.Item1)}{nResult}{(nIndex < result.Length ? result.Substring(nIndex) : "")}";
                for (var j = rIndex + rCount - 1; j >= rIndex; j--)
                    values.RemoveAt(j);

                values.Insert(rIndex, true);
            }
            //2. Solve remaining expression...
            result = Solve(result, values);
            return result == "1";
        }
    }
}