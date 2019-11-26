using System.Collections.Generic;
using Sample.Model;

namespace Sample.ViewModel
{
    public class FocusModelComparer : IEqualityComparer<FocusModel>
    {
        #region Methods

        public bool Equals(FocusModel x, FocusModel y)
        {
            return x.IdProperty.Equals(y.IdProperty);
        }

        public int GetHashCode(FocusModel obj)
        {
            return obj.IdProperty.GetHashCode();
        }

        #endregion Methods
    }
}