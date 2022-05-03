using System.Text;

namespace Imagin.Common.Numbers
{
    public static class Equation
    {
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
    }
}