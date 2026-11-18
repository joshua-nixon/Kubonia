using Avalonia;
using Avalonia.Controls;

namespace Kubonia.Client.Controls;

public partial class PageLayout : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<PageLayout, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<PageLayout, string>(nameof(Description), string.Empty);

    public static readonly StyledProperty<Control?> HeaderActionsProperty =
        AvaloniaProperty.Register<PageLayout, Control?>(nameof(HeaderActions));

    public static readonly StyledProperty<Control?> BodyProperty =
        AvaloniaProperty.Register<PageLayout, Control?>(nameof(Body));

    public PageLayout()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public Control? HeaderActions
    {
        get => GetValue(HeaderActionsProperty);
        set => SetValue(HeaderActionsProperty, value);
    }

    public Control? Body
    {
        get => GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }
}
