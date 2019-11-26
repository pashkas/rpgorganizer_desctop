namespace Sample.Model
{
    public class ChaAbilitis
    {
        /// <summary>
        /// Характеристика
        /// </summary>
        public string CharacteristicName { get; set; }

        /// <summary>
        /// Скилл
        /// </summary>
        public AbilitiModel Ability { get; set; }

        /// <summary>
        /// Требование скилла
        /// </summary>
        public int kNeedAbility { get; set; }
    }
}