namespace Sample.Model
{
    public class TaskRelayAbil
    {
        /// <summary>
        /// Требование задачи
        /// </summary>
        public NeedTasks TaskNeed { get; set; }

        /// <summary>
        /// Скилл
        /// </summary>
        public AbilitiModel Ability { get; set; }
    }

    public class QwestRelayAb
    {
        /// <summary>
        /// Требование задачи
        /// </summary>
        public CompositeAims AimNeed { get; set; }

        /// <summary>
        /// Скилл
        /// </summary>
        public AbilitiModel Ability { get; set; }
    }
}