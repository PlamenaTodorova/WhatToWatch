using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WhatToWatch.Utilities
{
    class StyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string dataValue = values[0] as string;
            Style red = values[1] as Style;
            Style yellow = values[2] as Style;
            Style green = values[3] as Style;
            Style none = values[4] as Style;

            if (dataValue == null)
                return none;

            if (dataValue.Equals("Red"))
                return red;

            if (dataValue.Equals("Yellow"))
                return yellow;

            if (dataValue.Equals("Green"))
                return green;

            return none;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
