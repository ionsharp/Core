namespace Imagin.Common
{
    public struct RowColumn
    {
        public int Row
        {
            get; set;
        }

        public int Column
        {
            get; set;
        }

        public RowColumn(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
        }
    }
}
