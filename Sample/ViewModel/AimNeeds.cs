namespace Sample.ViewModel
{
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Для требований квестов в одну строку в виде прогресс бара
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