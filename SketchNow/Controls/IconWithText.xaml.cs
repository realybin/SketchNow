using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

namespace SketchNow.Controls;

public partial class IconWithText : UserControl
{
    public IconWithText()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty KindProperty =
        DependencyProperty.Register(nameof(Kind), typeof(PackIconKind), typeof(IconWithText), new PropertyMetadata(PackIconKind.None));

    public PackIconKind Kind
    {
        get => (PackIconKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconWithText), new PropertyMetadata(string.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}