using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace XamlDS.Itk.Helpers;

public static class DataTemplateHelper
{
    public static void AddDataTemplate(FrameworkElement element, Type viewModelType, Type viewType)
    {
        DataTemplate template = new DataTemplate(viewModelType)
        {
            VisualTree = new FrameworkElementFactory(viewType)
        };
        element.Resources.Add(new DataTemplateKey(viewModelType), template);
    }
}
