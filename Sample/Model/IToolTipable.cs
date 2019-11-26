namespace Sample.Model
{
    /// <summary>
    /// Интерфейс для подсказок
    /// </summary>
    public interface IToolTipable
    {
        string NameOfProperty { get; set; }
        byte[] ImageProperty { get; set; }
        string DescriptionProperty { get; set; }
    }
}