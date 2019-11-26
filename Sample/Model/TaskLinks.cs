namespace Sample.Model
{
    using System;

    [Serializable]
    public class TaskLinks
    {
        /// <summary>
        /// �������� �������� �� ������� ������ ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��� �������� �� ������� ������ ������
        /// </summary>
        public RpgItemsTypes RPGItemType { get; set; }

        /// <summary>
        /// �� �������� �� ������� ������ ������
        /// </summary>
        public string IdElement { get; set; }
    }
}