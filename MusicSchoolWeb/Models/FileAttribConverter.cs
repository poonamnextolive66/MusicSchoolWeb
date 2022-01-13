// -----------------------------------------------------------------------
// <copyright file="FileAttribConverter.cs" company="Hewlett-Packard Company">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MusicSchoolWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileAttribConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = (bool)value;
            if (res)
            {
                return "FileImpression";
            }
            else
            {
                return "FileLength";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
