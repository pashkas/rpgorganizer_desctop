namespace Sample.Model
{
    public class TaskRelayAbil
    {
        /// <summary>
        /// ���������� ������
        /// </summary>
        public NeedTasks TaskNeed { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public AbilitiModel Ability { get; set; }
    }

    public class QwestRelayAb
    {
        /// <summary>
        /// ���������� ������
        /// </summary>
        public CompositeAims AimNeed { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public AbilitiModel Ability { get; set; }
    }
}