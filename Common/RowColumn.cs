using System;

namespace Imagin.Common
{
    /// <summary>
    /// A data type to represent a row and column pair.
    /// </summary>
    public struct RowColumn : IEquatable<RowColumn>
    {
        /// <summary>
        /// 
        /// </summary>
        public double Row
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Column
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public RowColumn(double row, double column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RowColumn left, RowColumn right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                    return true;

                return false;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RowColumn left, RowColumn right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Equals(RowColumn p)
        {
            if (ReferenceEquals(p, null))
                return false;

            if (ReferenceEquals(this, p))
                return true;

            if (GetType() != p.GetType())
                return false;

            return (Column == p.Column) && (Row == p.Row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            return Equals((RowColumn)o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return new { Column, Row }.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColumnMinimum"></param>
        /// <param name="ColumnMaximum"></param>
        /// <param name="RowMinimum"></param>
        /// <param name="RowMaximum"></param>
        /// <returns></returns>
        public bool InRange(double ColumnMinimum, double ColumnMaximum, double RowMinimum, double RowMaximum)
        {
            return Column >= ColumnMinimum && Column < ColumnMaximum && Row >= RowMinimum && Row < RowMaximum;
        }
    }
}
