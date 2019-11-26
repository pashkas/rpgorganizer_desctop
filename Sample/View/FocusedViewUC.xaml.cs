using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;

namespace Sample.View
{
    /// <summary>
    /// Interaction logic for FocusedViewUC.xaml
    /// </summary>
    public partial class FocusedViewUC : UserControl
    {
        public FocusedViewUC()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(
                  this,
                  item =>
                  {
                      if (item == "ФокусПанельЗадач!!!")
                      {
                      }
                  });
        }


        public static readonly DependencyProperty FocusedItemsVisibilityProperty = DependencyProperty.Register(
            "FocusedItemsVisibility", typeof (Visibility), typeof (FocusedViewUC), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// Видимость списка элементов фокусировки (когда все задачи группируются по элементам фокусировки)
        /// </summary>
        public Visibility FocusedItemsVisibility
        {
            get { return (Visibility) GetValue(FocusedItemsVisibilityProperty); }
            set { SetValue(FocusedItemsVisibilityProperty, value); }
        }


     



        public static readonly DependencyProperty SelectedFocusProperty = DependencyProperty.Register(
            "SelectedFocus", typeof (FocusModel), typeof (FocusedViewUC), new PropertyMetadata(default(FocusModel)));

        /// <summary>
        /// Выбранный элемент фокусировки на данный момент
        /// </summary>
        public FocusModel SelectedFocus
        {
            get
            {
                return (FocusModel) GetValue(SelectedFocusProperty);
            }
            set
            {
                SetValue(SelectedFocusProperty, value);
            }
        }
    
    }

    
}
