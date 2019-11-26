using DotNetLead.DragDrop.UI.Behavior;

namespace Sample.Model
{
    using System;

    /// <summary>
    /// Требования для характеристик
    /// </summary>
    [Serializable]
    public class NeedCharact : AimNeeds, IDropable, IDragable
    {
        #region Fields

        /// <summary>
        /// Характеристика
        /// </summary>
        private Characteristic characteristic;

        #endregion


        public double Progress
        {
            get
            {
                return (CharactProperty.ValueProperty - CharactProperty.FirstVal)/
                       (ValueProperty - CharactProperty.FirstVal);
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets Характеристика
        /// </summary>
        public Characteristic CharactProperty
        {
            get
            {
                return this.characteristic;
            }

            set
            {
                this.characteristic = value;
                this.OnPropertyChanged(nameof(CharactProperty));
            }
        }

        #endregion

        Type IDropable.DataType
        {
            get { return typeof (NeedCharact); }
        }

        public void Remove(object i)
        {
            
        }

        public void Drop(object data, int index = -1)
        {
            var allAims = StaticMetods.Locator.QwestsVM.SelectedAimProperty.NeedCharacts;
            int indB = allAims.IndexOf(this);
            var aimA = data as NeedCharact;
            int indA = allAims.IndexOf(aimA);
            allAims.Move(indA, indB);
        }

        Type IDragable.DataType
        {
            get { return typeof (NeedCharact); }
        }
    }
}