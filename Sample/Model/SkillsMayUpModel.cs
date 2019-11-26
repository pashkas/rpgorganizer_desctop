using Sample.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Sample.Model
{
    /// <summary>
    /// Отображение какие навыки доступны стали
    /// </summary>
    public static class SkillsMayUpModel
    {
        #region Private Properties

        private static List<AbilitiModel> valBefore { get; set; } = new List<AbilitiModel>();
        private static List<AbilitiModel> valAfter { get; set; } = new List<AbilitiModel>();

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Это надо вызвать до изменений
        /// </summary>
        public static void GetBefore()
        {
            valBefore.Clear();
            // Навыки, которые пока не доступны для прокачки
            var collection = getChangesAbs();
            valBefore.AddRange(collection);
        }

        /// <summary>
        /// Считываем что стало после изменений и отображаем какие навыки можно прокачать
        /// </summary>
        public static void ShowAbUps()
        {
            getAfter();
            var abs = valAfter.Except(valBefore).ToList();
            foreach (var abilitiModel in abs)
            {
                string headerText = $"Навык \"{abilitiModel.NameOfProperty}\" доступен для прокачки!!!";
                AddOrEditAbilityViewModel.showAbLevelChange(headerText, abilitiModel, Brushes.Green);
            }
            valBefore.Clear();
            valAfter.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private static void getAfter()
        {
            valAfter.Clear();
            // Навыки, которые пока не доступны для прокачки
            var collection = getChangesAbs();
            valAfter.AddRange(collection);
        }

        private static IEnumerable<AbilitiModel> getChangesAbs()
        {
            var collection = StaticMetods.PersProperty.Abilitis.Where(n => n.ClearedNotAllowReqwirements.Any());
            return collection;
        }

        #endregion Private Methods
    }
}