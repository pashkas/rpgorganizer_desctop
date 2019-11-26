namespace Sample.ViewModel
{
    /// <summary>
    ///     ��������� ������
    /// </summary>
    public class QwestHardness
    {
        #region Constructors

        public QwestHardness(string _name, int _val, string _toolTip)
        {
            HardnessName = _name;
            HardnessValue = _val;
            ToolTip = _toolTip;
        }

        #endregion Constructors

        #region Properties

        public string HardnessName { get; set; }
        public int HardnessValue { get; set; }
        
        /// <summary>
        /// ���������
        /// </summary>
        public string ToolTip { get; set; }

        #endregion Properties
    }
}