using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    public class PlaningViewModel:INotifyPropertyChanged
    {
        public Pers PersProperty
        {
            get { return StaticMetods.PersProperty; }
            set { StaticMetods.PersProperty = value; }
        }

        public PlaningViewModel()
        {
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
