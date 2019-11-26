using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Sample.Model;

namespace Sample.ViewModel
{
    public interface IHaveTaskPanel
    {

        /// <summary>
        /// ����������� ������ � ����� ������
        /// </summary>
        RelayCommand<Task> MoveTaskToEndOfListCommand { get; }

            /// <summary>
        /// ����������� ������ � ������ ������
        /// </summary>
        RelayCommand<Task> MoveTaskToBeginOfListCommand { get; }

        /// <summary>
        /// �������� ����� ������
        /// </summary>
        RelayCommand AddNewTask { get; }

            /// <summary>
        /// ��������� ������
        /// </summary>
        Task SellectedTask { get; set; }

        /// <summary>
        /// ������� �����
        /// </summary>
        List<StatusTask> Statuses { get; }

        /// <summary>
        ///     Gets the �������������� ������ �� ��������������� ������.
        /// </summary>
        RelayCommand<Task> AlterEditTaskCommand { get; }

        /// <summary>
        ///     Gets the ������� ����� �� ������ ������.
        /// </summary>
        RelayCommand<Task> TaskToQwestCommand { get; }

        /// <summary>
        ///     Gets the �������� �������������� ���������� ������.
        /// </summary>
        RelayCommand AlternateAddTaskCommand { get; }

        /// <summary>
        ///     Gets the �������������� "������ �� �������".
        /// </summary>
        RelayCommand<Task> AlternateMinusTaskCommand { get; }

        /// <summary>
        ///     Gets the �������������� �������� ������.
        /// </summary>
        RelayCommand<Task> AlternatePlusTaskCommand { get; }

        /// <summary>
        ///     Gets the �������� ������ �� ��������������� ������.
        /// </summary>
        RelayCommand<Task> AlternateRemoveTaskCommand { get; }

        /// <summary>
        ///     Gets the ��������� ������ �� ������.
        /// </summary>
        RelayCommand<Task> SendTaskToTomorowCommand { get; }
    }
}