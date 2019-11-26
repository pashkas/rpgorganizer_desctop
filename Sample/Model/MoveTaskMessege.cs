namespace Sample.Model
{
    /// <summary>
    /// The move task messege.
    /// </summary>
    public class MoveTaskMessege
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the task a.
        /// </summary>
        public Task taskA { get; set; }

        /// <summary>
        /// Gets or sets the task b.
        /// </summary>
        public Task taskB { get; set; }

        /// <summary>
        /// »гнорировать режим планировани€ при перемещении
        /// </summary>
        public bool IgnorePlaningMode { get; set; }

        #endregion
    }
}