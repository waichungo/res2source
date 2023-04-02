using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using res2source.viewmodels;

namespace res2source
{
    public class BooleanToEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object negated, CultureInfo culture)
        {
            var truthy = (bool)value;
            if (negated != null)
            {
                truthy = !truthy;
            }
            return truthy;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class FloatToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double no = 0;
            if (value != null)
            {
                if (value is float)
                {
                    no = (float)value;
                }
                else if (value is double)
                {
                    no = (double)value;
                }
            }


            return $"{(int)(no * 100)}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ListCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object negated, CultureInfo culture)
        {
            var count =(int) value;
            var visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
            if (negated != null&&negated is bool&& ((bool)negated ))
            {
                visibility = visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class EmptyListToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object negated, CultureInfo culture)
        {
            var count =(int) value;
            var ok = count> 0;
           
            if (negated != null)
            {
                ok = ((bool)negated) ? !ok : ok;
            }

            return ok;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class ListToCountTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as IEnumerable<object>;

            var total = list == null ? 0 : list.Count();
            var text = total > 0 ? $"Showing {total} entries" : (total == 1 ? "1 entry listed" : "Showing 0 entries");
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EmptyListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object negated, CultureInfo culture)
        {
            var list = value as IEnumerable<object>;
            var ok = list.Count() > 0;
            if (negated != null)
            {
                ok = ((bool)negated) ? !ok : ok;
            }
            return ok ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class NullOrEmptytToViibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object negatedParam, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            var negated = negatedParam != null ? ((bool)negatedParam) : false;
            if (value == null)
            {
                if (negated)
                {
                    visibility = Visibility.Visible;
                }
                else
                {
                    visibility = Visibility.Collapsed;
                }

            }
            else if (value is string)
            {
                var str = (string)value;
                if (str.Length == 0)
                {
                    if (negated)
                    {
                        visibility = Visibility.Visible;
                    }
                    else
                    {
                        visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (negated)
                    {
                        visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        visibility = Visibility.Visible;
                    }
                }


            }
            else if (value is IEnumerable<object>)
            {
                var list = (IEnumerable<object>)value;
                if (list.Count() == 0)
                {
                    if (negated)
                    {
                        visibility = Visibility.Visible;
                    }
                    else
                    {
                        visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (negated)
                    {
                        visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                if (negated)
                {
                    visibility = Visibility.Collapsed;
                }
                else
                {
                    visibility = Visibility.Visible;
                }
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object negated, CultureInfo culture)
        {
            var truthy = (bool)value;
            if (negated != null)
            {
                var negatedVal = (bool)negated;
                if (negatedVal)
                {
                    truthy = !truthy;
                }
            }
            return truthy ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
   

}
