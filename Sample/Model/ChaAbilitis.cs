namespace Sample.Model
{
    public class ChaAbilitis
    {
        /// <summary>
        /// ��������������
        /// </summary>
        public string CharacteristicName { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public AbilitiModel Ability { get; set; }

        /// <summary>
        /// ���������� ������
        /// </summary>
        public int kNeedAbility { get; set; }
    }
}