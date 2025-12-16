namespace PlaySudoku.Models
{
    public class SudokuCell
    {
        public int Value { get; set; }
        public bool IsFixed { get; set; }

        public SudokuCell(int value, bool isFixed)
        {
            Value = value;
            IsFixed = isFixed;
        }
    }
}