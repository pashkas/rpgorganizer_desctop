using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sample.Model
{
    public class AbUslStrikeOutConverter:IMultiValueConverter
    {
        /// <summary>
        /// Зачеркивать название условия в скилле если оно выполнено
        /// </summary>
        /// <param name="values">1-требование квеста или задачи, 2-скилл</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Толщина
            if (parameter != null && parameter.ToString() == "Thikness")
            {
                var ab = values[1] as AbilitiModel;
                if(ab == null)
                    return new Thickness(2);
                var ind = System.Convert.ToInt32(values[0]);
                if (ab.CellValue == ind)
                {
                    return new Thickness(2.5);
                }
                else
                {
                    var th = 1;
                    if (ind == 1 && ab.CellValue == 0)
                    {
                        return new Thickness(th);
                    }
                    if (ind<= ab.CellValue)
                    {
                        return new Thickness(th,0,th,th);
                    }
                    else
                    {
                        return new Thickness(th, th, th, 0);
                    }
                }
            }
            // Если надо расчитать прозрачность
            if (parameter != null && parameter.ToString() == "Opacity2")
            {
                var ab = values[1] as AbilitiModel;
                if (ab == null) return 1;
                var ind = System.Convert.ToInt32(values[0]);
                if (ab.CellValue != ind)
                {
                    return 0.5;
                }
                else
                {
                    return 1;
                }
            }
                if (parameter!=null&&parameter.ToString()== "Opacity")
            {
                var ab = values[1] as AbilitiModel;
                var tsks = values[0] as NeedTasks;
                // Если задача
                if (tsks != null && ab != null)
                {
                    var item = tsks;
                    if (ab.CellValue != item.LevelProperty)
                    {
                        return 0.5;
                    }
                    else
                    {
                        return 1;
                    }
                }
                // Если квест
                var aims = values[0] as CompositeAims;
                if (aims != null && ab != null)
                {
                    var item = aims;
                    if (!item.AimProperty.IsActiveProperty)
                    {
                        return 0.6;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else if (parameter != null && parameter.ToString() == "index")
            {
                var ab = values[0] as AbilitiModel;
                // Если задача
                if (ab != null && values[1] is int)
                {
                    int ind = (int)values[1];
                    if ((ab.CellValue > ind))
                    {
                        return TextDecorations.Strikethrough;
                    }
                    return null;
                }
            }
            else if (parameter != null && parameter.ToString() == "needTask")
            {
                bool b = (bool)values[0];
                if (b)
                {
                    return TextDecorations.Strikethrough;
                }
            }
            else if (parameter != null && parameter.ToString() == "Qwest")
            {
                var qw = values[0] as Aim;
                if (qw!=null && qw.IsDoneProperty)
                {
                    return TextDecorations.Strikethrough;
                }
            }
            else
            {
                // Если квест
                var ab = values[1] as AbilitiModel;
                var aims = values[0] as CompositeAims;
                if (aims != null && ab != null)
                {
                    var item = aims;
                    if (item.AimProperty.IsDoneProperty)
                    {
                        return TextDecorations.Strikethrough;
                    }
                }

                var tsks = values[0] as NeedTasks;
                // Если задача
                if (tsks != null && ab != null && values[2] is int)
                {
                    var item = tsks;
                    int ind = (int)values[2];
                    if ((ab.CellValue > ind) || item.TaskProperty.IsDelProperty)
                    {
                        return TextDecorations.Strikethrough;
                    }
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
