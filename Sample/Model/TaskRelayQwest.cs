namespace Sample.Model
{
    public class TaskRelayQwest
    {
        /// <summary>
        /// ���������� ������
        /// </summary>
        public NeedTasks TaskNeed { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public Aim Qwest { get; set; }
    }

    public class QwestRelayQwest
    {
        /// <summary>
        /// ���������� ������
        /// </summary>
        public CompositeAims QwestNeed { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public Aim Qwest { get; set; }
    }
}