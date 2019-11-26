namespace Sample.Model
{
    public class TaskRelayQwest
    {
        /// <summary>
        /// Требование задачи
        /// </summary>
        public NeedTasks TaskNeed { get; set; }

        /// <summary>
        /// Квест
        /// </summary>
        public Aim Qwest { get; set; }
    }

    public class QwestRelayQwest
    {
        /// <summary>
        /// Требование задачи
        /// </summary>
        public CompositeAims QwestNeed { get; set; }

        /// <summary>
        /// Квест
        /// </summary>
        public Aim Qwest { get; set; }
    }
}