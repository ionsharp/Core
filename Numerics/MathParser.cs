using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace Imagin.Core.Numerics;

/// <summary>A parser for math equations.</summary>
/// <remarks><see cref="MathParser">Legacy</see>: Source unknown!</remarks>
public class MathParser
{
    #region Legacy

    #region Example usage

    // public static void Main()
    // {     
    //     MathParser parser = new MathParser();
    //     string s1 = "pi+5*5+5*3-5*5-5*3+1E1";
    //     string s2 = "sin(cos(tg(sh(ch(th(100))))))";
    //     bool isRadians = false;
    //     double d1 = parser.Parse(s1, isRadians);
    //     double d2 = parser.Parse(s2, isRadians);
    //
    //     Console.WriteLine(d1); // 13.141592...
    //     Console.WriteLine(d2); // 0.0174524023974442
    //     Console.ReadKey(true); 
    // }   

    #endregion

    #region Common tips to extend this math parser

    // If you want to add new operator, then add new item to supportedOperators
    // dictionary and check next methods:
    // (1) IsRightAssociated, (2) GetPriority, (3) SyntaxAnalysisRPN, (4) NumberOfArguments
    //
    // If you want to add new function, then add new item to supportedFunctions
    // dictionary and check next above methods: (2), (3), (4).
    // 
    // if you want to add new constant, then add new item to supportedConstants

    #endregion

    #region Fields

    #region Markers (each marker should have length equals to 1)

    const string NumberMaker = "#";

    const string OperatorMarker = "$";

    const string FunctionMarker = "@";

    #endregion

    #region Internal tokens

    const string Plus = OperatorMarker + "+";
    const string UnPlus = OperatorMarker + "un+";
    const string Minus = OperatorMarker + "-";
    const string UnMinus = OperatorMarker + "un-";
    const string Multiply = OperatorMarker + "*";
    const string Divide = OperatorMarker + "/";
    const string Degree = OperatorMarker + "^";
    const string LeftParent = OperatorMarker + "(";
    const string RightParent = OperatorMarker + ")";
    const string Sqrt = FunctionMarker + "sqrt";
    const string Sin = FunctionMarker + "sin";
    const string Cos = FunctionMarker + "cos";
    const string Tg = FunctionMarker + "tg";
    const string Ctg = FunctionMarker + "ctg";
    const string Sh = FunctionMarker + "sh";
    const string Ch = FunctionMarker + "ch";
    const string Th = FunctionMarker + "th";
    const string Log = FunctionMarker + "log";
    const string Ln = FunctionMarker + "ln";
    const string Exp = FunctionMarker + "exp";
    const string Abs = FunctionMarker + "abs";

    #endregion

    #region Dictionaries (containts supported input tokens, exclude number)

    // Key -> supported input token, Value -> internal token or number

    /// <summary>
    /// Contains supported operators
    /// </summary>
    readonly Dictionary<string, string> supportedOperators = new Dictionary<string, string>
    {
        { "+", Plus },
        { "-", Minus },
        { "*", Multiply },
        { "/", Divide },
        { "^", Degree },
        { "(", LeftParent },
        { ")", RightParent }
    };

    /// <summary>
    /// Contains supported functions
    /// </summary>
    readonly Dictionary<string, string> supportedFunctions = new Dictionary<string, string>
    {
        { "sqrt", Sqrt },
        { "√", Sqrt },
        { "sin", Sin },
        { "cos", Cos },
        { "tg", Tg },
        { "ctg", Ctg },
        { "sh", Sh },
        { "ch", Ch },
        { "th", Th },
        { "log", Log },
        { "exp", Exp },
        { "abs", Abs }
    };

    readonly Dictionary<string, string> supportedConstants = new Dictionary<string, string>
    {
        {"pi", NumberMaker + System.Math.PI.ToString() },
        {"e", NumberMaker + System.Math.E.ToString() }
    };

    #endregion

    readonly char decimalSeparator;

    bool isRadians;

    #endregion

    #region MathParser

    /// <summary>
    /// Initialize new instance of MathParser
    /// (symbol of decimal separator is read
    /// from regional settings in system)
    /// </summary>
    public MathParser()
    {
        try
        {
            decimalSeparator = char.Parse(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Error: can't read char decimal separator from system, check your regional settings.", ex);
        }
    }

    /// <summary>
    /// Initialize new instance of MathParser
    /// </summary>
    /// <param name="decimalSeparator">Set decimal separator</param>
    public MathParser(char decimalSeparator)
    {
        this.decimalSeparator = decimalSeparator;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Produce result of the given math expression
    /// </summary>
    /// <param name="expression">Math expression (infix/standard notation)</param>
    /// <returns>Result</returns>
    public double Parse(string expression, bool isRadians = true)
    {
        this.isRadians = isRadians;

        try
        {
            return Calculate(ConvertToRPN(FormatString(expression)));
        }
        catch (DivideByZeroException e)
        {
            throw e;
        }
        catch (FormatException e)
        {
            throw e;
        }
        catch (InvalidOperationException e)
        {
            throw e;
        }
        catch (ArgumentOutOfRangeException e)
        {
            throw e;
        }
        catch (ArgumentException e)
        {
            throw e;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    /// <summary>
    /// Produce formatted string by the given string
    /// </summary>
    /// <param name="expression">Unformatted math expression</param>
    /// <returns>Formatted math expression</returns>
    private string FormatString(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            throw new ArgumentNullException("Expression is null or empty");
        }

        StringBuilder formattedString = new StringBuilder();
        int balanceOfParenth = 0; // Check number of parenthesis

        // Format string in one iteration and check number of parenthesis
        // (this function do 2 tasks because performance priority)
        for (int i = 0; i < expression.Length; i++)
        {
            char ch = expression[i];

            if (ch == '(')
            {
                balanceOfParenth++;
            }
            else if (ch == ')')
            {
                balanceOfParenth--;
            }

            if (char.IsWhiteSpace(ch))
            {
                continue;
            }
            else if (char.IsUpper(ch))
            {
                formattedString.Append(char.ToLower(ch));
            }
            else
            {
                formattedString.Append(ch);
            }
        }

        if (balanceOfParenth != 0)
        {
            throw new FormatException("Number of left and right parenthesis is not equal");
        }

        return formattedString.ToString();
    }

    #region Convert to Reverse-Polish Notation

    /// <summary>
    /// Produce math expression in reverse polish notation
    /// by the given string
    /// </summary>
    /// <param name="expression">Math expression in infix notation</param>
    /// <returns>Math expression in postfix notation (RPN)</returns>
    private string ConvertToRPN(string expression)
    {
        int pos = 0; // Current position of lexical analysis
        StringBuilder outputString = new StringBuilder();
        Stack<string> stack = new Stack<string>();

        // While there is unhandled char in expression
        while (pos < expression.Length)
        {
            string token = LexicalAnalysisInfixNotation(expression, ref pos);

            outputString = SyntaxAnalysisInfixNotation(token, outputString, stack);
        }

        // Pop all elements from stack to output string            
        while (stack.Count > 0)
        {
            // There should be only operators
            if (stack.Peek()[0] == OperatorMarker[0])
            {
                outputString.Append(stack.Pop());
            }
            else
            {
                throw new FormatException("Format exception,"
                + " there is function without parenthesis");
            }
        }

        return outputString.ToString();
    }

    /// <summary>
    /// Produce token by the given math expression
    /// </summary>
    /// <param name="expression">Math expression in infix notation</param>
    /// <param name="pos">Current position in string for lexical analysis</param>
    /// <returns>Token</returns>
    private string LexicalAnalysisInfixNotation(string expression, ref int pos)
    {
        // Receive first char
        StringBuilder token = new StringBuilder();
        token.Append(expression[pos]);

        // If it is a operator
        if (supportedOperators.ContainsKey(token.ToString()))
        {
            // Determine it is unary or binary operator
            bool isUnary = pos == 0 || expression[pos - 1] == '(';
            pos++;

            switch (token.ToString())
            {
                case "+":
                    return isUnary ? UnPlus : Plus;
                case "-":
                    return isUnary ? UnMinus : Minus;
                default:
                    return supportedOperators[token.ToString()];
            }
        }
        else if (char.IsLetter(token[0])
            || supportedFunctions.ContainsKey(token.ToString())
            || supportedConstants.ContainsKey(token.ToString()))
        {
            // Read function or constant name

            while (++pos < expression.Length
                && char.IsLetter(expression[pos]))
            {
                token.Append(expression[pos]);
            }

            if (supportedFunctions.ContainsKey(token.ToString()))
            {
                return supportedFunctions[token.ToString()];
            }
            else if (supportedConstants.ContainsKey(token.ToString()))
            {
                return supportedConstants[token.ToString()];
            }
            else
            {
                throw new ArgumentException("Unknown token");
            }

        }
        else if (char.IsDigit(token[0]) || token[0] == decimalSeparator)
        {
            // Read number

            // Read the whole part of number
            if (char.IsDigit(token[0]))
            {
                while (++pos < expression.Length
                && char.IsDigit(expression[pos]))
                {
                    token.Append(expression[pos]);
                }
            }
            else
            {
                // Because system decimal separator
                // will be added below
                token.Clear();
            }

            // Read the fractional part of number
            if (pos < expression.Length
                && expression[pos] == decimalSeparator)
            {
                // Add current system specific decimal separator
                token.Append(CultureInfo.CurrentCulture
                    .NumberFormat.NumberDecimalSeparator);

                while (++pos < expression.Length
                && char.IsDigit(expression[pos]))
                {
                    token.Append(expression[pos]);
                }
            }

            // Read scientific notation (suffix)
            if (pos + 1 < expression.Length && expression[pos] == 'e'
                && (char.IsDigit(expression[pos + 1])
                    || (pos + 2 < expression.Length
                        && (expression[pos + 1] == '+'
                            || expression[pos + 1] == '-')
                        && char.IsDigit(expression[pos + 2]))))
            {
                token.Append(expression[pos++]); // e

                if (expression[pos] == '+' || expression[pos] == '-')
                    token.Append(expression[pos++]); // sign

                while (pos < expression.Length
                    && char.IsDigit(expression[pos]))
                {
                    token.Append(expression[pos++]); // power
                }

                // Convert number from scientific notation to decimal notation
                return NumberMaker + Convert.ToDouble(token.ToString());
            }

            return NumberMaker + token.ToString();
        }
        else
        {
            throw new ArgumentException("Unknown token in expression");
        }
    }

    /// <summary>
    /// Syntax analysis of infix notation
    /// </summary>
    /// <param name="token">Token</param>
    /// <param name="outputString">Output string (math expression in RPN)</param>
    /// <param name="stack">Stack which contains operators (or functions)</param>
    /// <returns>Output string (math expression in RPN)</returns>
    private StringBuilder SyntaxAnalysisInfixNotation(string token, StringBuilder outputString, Stack<string> stack)
    {
        // If it's a number just put to string            
        if (token[0] == NumberMaker[0])
        {
            outputString.Append(token);
        }
        else if (token[0] == FunctionMarker[0])
        {
            // if it's a function push to stack
            stack.Push(token);
        }
        else if (token == LeftParent)
        {
            // If its '(' push to stack
            stack.Push(token);
        }
        else if (token == RightParent)
        {
            // If its ')' pop elements from stack to output string
            // until find the ')'

            string elem;
            while ((elem = stack.Pop()) != LeftParent)
            {
                outputString.Append(elem);
            }

            // if after this a function is in the peek of stack then put it to string
            if (stack.Count > 0 &&
                stack.Peek()[0] == FunctionMarker[0])
            {
                outputString.Append(stack.Pop());
            }
        }
        else
        {
            // While priority of elements at peek of stack >= (>) token's priority
            // put these elements to output string
            while (stack.Count > 0 &&
                Priority(token, stack.Peek()))
            {
                outputString.Append(stack.Pop());
            }

            stack.Push(token);
        }

        return outputString;
    }

    /// <summary>
    /// Is priority of token less (or equal) to priority of p
    /// </summary>
    private bool Priority(string token, string p)
    {
        return IsRightAssociated(token) ?
            GetPriority(token) < GetPriority(p) :
            GetPriority(token) <= GetPriority(p);
    }

    /// <summary>
    /// Is right associated operator
    /// </summary>
    private bool IsRightAssociated(string token)
    {
        return token == Degree;
    }

    /// <summary>
    /// Get priority of operator
    /// </summary>
    private int GetPriority(string token)
    {
        switch (token)
        {
            case LeftParent:
                return 0;
            case Plus:
            case Minus:
                return 2;
            case UnPlus:
            case UnMinus:
                return 6;
            case Multiply:
            case Divide:
                return 4;
            case Degree:
            case Sqrt:
                return 8;
            case Sin:
            case Cos:
            case Tg:
            case Ctg:
            case Sh:
            case Ch:
            case Th:
            case Log:
            case Ln:
            case Exp:
            case Abs:
                return 10;
            default:
                throw new ArgumentException("Unknown operator");
        }
    }

    #endregion

    #region Calculate expression in RPN

    /// <summary>
    /// Calculate expression in reverse-polish notation
    /// </summary>
    /// <param name="expression">Math expression in reverse-polish notation</param>
    /// <returns>Result</returns>
    private double Calculate(string expression)
    {
        int pos = 0; // Current position of lexical analysis
        var stack = new Stack<double>(); // Contains operands

        // Analyse entire expression
        while (pos < expression.Length)
        {
            string token = LexicalAnalysisRPN(expression, ref pos);

            stack = SyntaxAnalysisRPN(stack, token);
        }

        // At end of analysis in stack should be only one operand (result)
        if (stack.Count > 1)
        {
            throw new ArgumentException("Excess operand");
        }

        return stack.Pop();
    }

    /// <summary>
    /// Produce token by the given math expression
    /// </summary>
    /// <param name="expression">Math expression in reverse-polish notation</param>
    /// <param name="pos">Current position of lexical analysis</param>
    /// <returns>Token</returns>
    private string LexicalAnalysisRPN(string expression, ref int pos)
    {
        StringBuilder token = new StringBuilder();

        // Read token from marker to next marker

        token.Append(expression[pos++]);

        while (pos < expression.Length && expression[pos] != NumberMaker[0]
            && expression[pos] != OperatorMarker[0]
            && expression[pos] != FunctionMarker[0])
        {
            token.Append(expression[pos++]);
        }

        return token.ToString();
    }

    /// <summary>
    /// Syntax analysis of reverse-polish notation
    /// </summary>
    /// <param name="stack">Stack which contains operands</param>
    /// <param name="token">Token</param>
    /// <returns>Stack which contains operands</returns>
    private Stack<double> SyntaxAnalysisRPN(Stack<double> stack, string token)
    {
        // if it's operand then just push it to stack
        if (token[0] == NumberMaker[0])
        {
            stack.Push(double.Parse(token.Remove(0, 1)));
        }
        // Otherwise apply operator or function to elements in stack
        else if (NumberOfArguments(token) == 1)
        {
            double arg = stack.Pop();
            double rst;

            switch (token)
            {
                case UnPlus:
                    rst = arg;
                    break;
                case UnMinus:
                    rst = -arg;
                    break;
                case Sqrt:
                    rst = System.Math.Sqrt(arg);
                    break;
                case Sin:
                    rst = ApplyTrigFunction(System.Math.Sin, arg);
                    break;
                case Cos:
                    rst = ApplyTrigFunction(System.Math.Cos, arg);
                    break;
                case Tg:
                    rst = ApplyTrigFunction(System.Math.Tan, arg);
                    break;
                case Ctg:
                    rst = 1 / ApplyTrigFunction(System.Math.Tan, arg);
                    break;
                case Sh:
                    rst = System.Math.Sinh(arg);
                    break;
                case Ch:
                    rst =
                rst = System.Math.Cosh(arg);
                    break;
                case Th:
                    rst = System.Math.Tanh(arg);
                    break;
                case Ln:
                    rst = System.Math.Log(arg);
                    break;
                case Exp:
                    rst = System.Math.Exp(arg);
                    break;
                case Abs:
                    rst = System.Math.Abs(arg);
                    break;
                default:
                    throw new ArgumentException("Unknown operator");
            }

            stack.Push(rst);
        }
        else
        {
            // otherwise operator's number of arguments equals to 2

            double arg2 = stack.Pop();
            double arg1 = stack.Pop();

            double rst;

            switch (token)
            {
                case Plus:
                    rst = arg1 + arg2;
                    break;
                case Minus:
                    rst = arg1 - arg2;
                    break;
                case Multiply:
                    rst = arg1 * arg2;
                    break;
                case Divide:
                    if (arg2 == 0)
                    {
                        throw new DivideByZeroException("Second argument is zero");
                    }
                    rst = arg1 / arg2;
                    break;
                case Degree:
                    rst = System.Math.Pow(arg1, arg2);
                    break;
                case Log:
                    rst = System.Math.Log(arg2, arg1);
                    break;
                default:
                    throw new ArgumentException("Unknown operator");
            }

            stack.Push(rst);
        }

        return stack;
    }

    /// <summary>
    /// Apply trigonometric function
    /// </summary>
    /// <param name="func">Trigonometric function</param>
    /// <param name="arg">Argument</param>
    /// <returns>Result of function</returns>
    private double ApplyTrigFunction(Func<double, double> func, double arg)
    {
        if (!isRadians)
        {
            arg = arg * System.Math.PI / 180; // Convert value to degree
        }

        return func(arg);
    }

    /// <summary>
    /// Produce number of arguments for the given operator
    /// </summary>
    private int NumberOfArguments(string token)
    {
        switch (token)
        {
            case UnPlus:
            case UnMinus:
            case Sqrt:
            case Tg:
            case Sh:
            case Ch:
            case Th:
            case Ln:
            case Ctg:
            case Sin:
            case Cos:
            case Exp:
            case Abs:
                return 1;
            case Plus:
            case Minus:
            case Multiply:
            case Divide:
            case Degree:
            case Log:
                return 2;
            default:
                throw new ArgumentException("Unknown operator");
        }
    }

    #endregion

    #endregion

    #endregion

    /// <summary>Specifies the level of difficulty.</summary>
    [Serializable]
    public enum Difficulty : int
    {
        None,
        Easy,
        Normal,
        Hard
    }

    static char Operator(Difficulty difficulty)
    {
        var n = 4;
        switch (difficulty)
        {
            //+, -
            case Difficulty.Easy:
                n = 2;
                break;
            //+, -, *
            case Difficulty.Normal:
                n = 3;
                break;
            //+, -, *, /
            case Difficulty.Hard:
                n = 4;
                break;
        }

        switch (Random.Current.Next(n))
        {
            case 0:
                return '+';
            case 1:
                return '-';
            case 2:
                return '*';
            case 3:
                return '/';
        }
        return default;
    }

    public static string Clean(string input)
    {
        var result = new StringBuilder();
        foreach (var i in input)
        {
            if (i == ' ')
                continue;

            switch (i)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case ')':
                case '(':
                case ',':
                case '.':
                    result.Append(i);
                    break;

                case '+':
                case '-':
                case '*':
                case '/':
                case '\\':
                    result.Append(' ');
                    result.Append(i);
                    result.Append(' ');
                    break;
            }
        }
        return result.ToString();
    }
        
    public static string Create(Difficulty difficulty)
    {
        var result = string.Empty;

        int parenthesis = 0;
        int lastParenthesis = 0;

        int m = 0, n = 0;
        switch (difficulty)
        {
            case Difficulty.Easy:
                n = Random.Current.Next(2, 3);
                m = 21;
                break;
            case Difficulty.Normal:
                n = Random.Current.Next(2, 4);
                m = 51;
                break;
            case Difficulty.Hard:
                n = Random.Current.Next(3, 5);
                m = 101;
                break;
        }

        for (var i = 0; i < n; i++)
        {
            result += $"{Random.Current.Next(m)}";

            if (parenthesis > 0)
            {
                lastParenthesis++;
                if (lastParenthesis > 1)
                {
                    if (Random.Current.Next(2) == 1)
                    {
                        result += ")";
                        parenthesis--;
                        lastParenthesis = 0;
                    }
                }
            }

            if (i + 1 == n)
            {
                if (parenthesis > 0)
                {
                    for (var j = 0; j < parenthesis; j++)
                    {
                        result += ")";
                    }
                }
            }
            else if (i + 1 < n)
            {
                result += $" {Operator(difficulty)} ";
                if (i + 2 < n)
                {
                    if (Random.Current.Next(2) == 1)
                    {
                        result += "(";
                        parenthesis++;
                        lastParenthesis = 0;
                    }
                }
            }
        }

        return result;
    }

    public static double Solve(string equation)
    {
        var result = new DataTable();
        return Convert.ToDouble(result.Compute(equation, string.Empty));
    }
}