using System.Collections.Generic;

namespace Sample.ViewModel
{
    public interface IQwickAdd
    {
        /// <summary>
        ///     ������ ����� ��� �������� ���������� �����
        /// </summary>
        List<QwickAdd> QwickAddTasksList { get; set; }

        void QwickAdd();
        void QwickAddElement(List<QwickAdd> qwickAddTasksList);
    }
}