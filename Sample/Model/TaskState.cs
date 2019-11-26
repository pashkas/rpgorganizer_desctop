using System;

namespace Sample.Model
{
    [Serializable]
    public class TaskState
    {
        /// <summary>
        /// Название состояния.
        /// </summary>
        public string Name { get; set; }
    }
}