namespace Sample.ViewModel
{
    using System.Windows.Media.Imaging;

    /// <summary>
    /// ��� ���������� ������� � ���� ������ � ���� �������� ����
    /// </summary>
    public class AllAimNeeds
    {
        public string NameOfNeed { get; set; }

        public double Progress { get; set; }

        public byte[] Image { get; set; }

        public string Type { get; set; }

        public string GUID { get; set; }
    }
}