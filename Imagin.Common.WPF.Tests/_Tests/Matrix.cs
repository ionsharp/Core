using System;
using System.Collections.Generic;

namespace Imagin.Common.Tests
{
    public class MatrixTests : TestSeries
    {
        public MatrixTests(string name) : base(name) { }

        protected override IEnumerable<Test> GetTests()
        {
            var matrix_a = new Matrix(new[]
            {
                new[] { 8.0, 3, 2, 6, 5 },
                new[] { 1.0, 6, 3, 2, 9 },
                new[] { 7.0, 1, 5, 8, 6 },
            });

            var matrix_b = new Matrix(new[]
            {
                new[] { 2.0, 2, 2, 2, 2 },
                new[] { 2.0, 2, 2, 2, 2 },
                new[] { 2.0, 2, 2, 2, 2 },
            });

            var matrix_c = new Matrix(new[]
            {
                new[] { 8.0, 3, 5 },
                new[] { 1.0, 5, 6 },
                new[] { 7.0, 8, 4 },
                new[] { 3.0, 9, 7 },
                new[] { 6.0, 1, 8 },
            });

            var vector_a = new Vector(3, 2, 4, 2, 5);

            yield return new ArithmeticTest<Matrix, Matrix>("a + b", matrix_a, matrix_b, (a, b) => a + b);
            yield return new ArithmeticTest<Matrix, Matrix>("a - b", matrix_a, matrix_b, (a, b) => a - b);
            yield return new ArithmeticTest<Matrix, Matrix>("a * b", matrix_a, matrix_c, (a, b) => a * b);
            yield return new ArithmeticTest<Matrix, Double>("a * b", matrix_a, 4.0,      (a, b) => a * b);
            yield return new ArithmeticTest<Matrix, Vector>("a * b", matrix_a, vector_a, (a, b) => a * b);

            yield return new ArithmeticTest<Matrix>("a[-1]", matrix_a, a => a.Invert());
        }
    }
}