using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TestApplication.UI
{
    public class BooleanToVisibilityConverter
        : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            bool negate = false;
            bool useHidden = false;
            string currentParameters = parameter as String;
            Visibility visibility = Visibility.Collapsed;

            if (!String.IsNullOrEmpty(currentParameters))
            {
                currentParameters = currentParameters.ToLower();
                if (currentParameters.Contains("h"))
                    useHidden = true;
                if (currentParameters.Contains("!"))
                    negate = true;
            }

            if (isVisible)
            {
                if (negate)
                    visibility = Visibility.Collapsed;
                else
                    visibility = Visibility.Visible;
            }
            else
            {
                if (negate)
                    visibility = Visibility.Visible;
                else
                    visibility = Visibility.Collapsed;
            }

            if (useHidden && visibility == Visibility.Collapsed)
                visibility = Visibility.Hidden;

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
