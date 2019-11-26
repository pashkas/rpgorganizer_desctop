namespace Sample.Model
{
    using System;

    /// <summary>
    /// �������� ��� �����, ����� ��� �� ����� ���������
    /// </summary>
    [Serializable]
    public class Context
    {
        #region Public Properties

        /// <summary>
        /// �������� ���������
        /// </summary>
        public string NameOfContext { get; set; }

        /// <summary>
        /// ���������� ����� ���������
        /// </summary>
        public string Uid { get; set; }

        #endregion
    }
}