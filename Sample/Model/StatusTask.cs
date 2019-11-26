namespace Sample.Model
{
    using System;

    /// <summary>
    /// ������ ������, ���� ������ �����, ��������, �����-������ �.�.�.
    /// </summary>
    [Serializable]
    public class StatusTask
    {

        #region Public Properties
        
        /// <summary>
        /// �������� �������
        /// </summary>
        public string NameOfStatus { get; set; }

        /// <summary>
        /// ���������� ����� �������
        /// </summary>
        public string Uid { get; set; }

        #endregion
    }
}