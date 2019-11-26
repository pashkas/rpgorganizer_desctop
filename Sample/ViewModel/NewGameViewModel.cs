using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;
    using System.IO;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// Выбор персонажей - начать новую игру
    /// </summary>
    public class NewGameViewModel : INotifyPropertyChanged
    {
        private string pathToPersTemplates = Path.Combine(Directory.GetCurrentDirectory(), "Templates");

        /// <summary>
        /// Выбранный персонаж.
        /// </summary>
        private Pers sellectedPers;

        public NewGameViewModel()
        {
            Perses = new List<Pers>();

            foreach (string file in Directory.GetFiles(pathToPersTemplates))
            {
                try
                {
                    Perses.Add(Pers.LoadPers(file));
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Sets and gets Выбранный персонаж.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers SellectedPersProperty
        {
            get
            {
                return sellectedPers;
            }

            set
            {
                if (sellectedPers == value)
                {
                    return;
                }

                sellectedPers = value;
                OnPropertyChanged(nameof(SellectedPersProperty));
            }
        }

        /// <summary>
        /// Персонажи из папки
        /// </summary>
        public List<Pers> Perses { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}