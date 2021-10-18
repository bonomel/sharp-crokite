using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public static class ViewModelLocator
    {
        public static DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            string viewTypeName = d.GetType().Name;
            string viewModelAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string viewModelTypeName = viewModelAssemblyName + ".ViewModels." + viewTypeName + "Model";

            Type viewModelType = Type.GetType(viewModelTypeName);
            object viewModel = Activator.CreateInstance(viewModelType);

            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
