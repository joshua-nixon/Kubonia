using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Kubonia.Client.Controls;

public partial class SortableDataGrid : UserControl
{
    public SortableDataGrid()
    {
        InitializeComponent();
        ColumnVisibilityPopup.PlacementTarget = ColumnsButton;
    }

    public IList<DataGridColumn> Columns => InnerGrid.Columns;

    public IEnumerable? ItemsSource
    {
        get => InnerGrid.ItemsSource;
        set => InnerGrid.ItemsSource = value;
    }

    public double ColumnHeaderHeight
    {
        get => InnerGrid.ColumnHeaderHeight;
        set => InnerGrid.ColumnHeaderHeight = value;
    }

    public double RowHeight
    {
        get => InnerGrid.RowHeight;
        set => InnerGrid.RowHeight = value;
    }

    private void ColumnsButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ColumnVisibilityPopup.IsOpen)
        {
            ColumnVisibilityPopup.IsOpen = false;
            return;
        }

        ColumnOptionsPanel.Children.Clear();
        for (var index = 0; index < InnerGrid.Columns.Count; index++)
        {
            var column = InnerGrid.Columns[index];
            var header = column.Header?.ToString();
            var option = new CheckBox
            {
                Classes = { "SortableDataGridColumnOption" },
                Content = string.IsNullOrWhiteSpace(header) ? $"Column {index + 1}" : header,
                IsChecked = column.IsVisible
            };
            option.IsCheckedChanged += (_, _) => column.IsVisible = option.IsChecked == true;
            ColumnOptionsPanel.Children.Add(option);
        }

        ColumnVisibilityPopup.IsOpen = true;
    }
}
