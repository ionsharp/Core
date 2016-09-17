namespace Imagin.Common
{
    public struct RowColumn
    {
        public double Row
        {
            get; set;
        }

        public double Column
        {
            get; set;
        }

        public RowColumn(double Row, double Column)
        {
            this.Row = Row;
            this.Column = Column;
        }
    }
}
