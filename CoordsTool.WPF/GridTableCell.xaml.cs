using System;
using System.Windows;
using System.Windows.Controls;
using CoordsTool.Core.Coordinates;

namespace CoordsTool.WPF;

public partial class GridTableCell : UserControl
{
    public static readonly DependencyProperty IsHeaderProperty = DependencyProperty.Register(
        "IsHeader", typeof(bool), typeof(GridTableCell), new PropertyMetadata(default(bool)));

    public bool IsHeader
    {
        get => (bool)GetValue(IsHeaderProperty);
        set => SetValue(IsHeaderProperty, value);
    }

    public static readonly DependencyProperty IsEdgeProperty = DependencyProperty.Register("IsEdge",
        typeof(bool), typeof(GridTableCell), new PropertyMetadata(default(bool)));

    public bool IsEdge
    {
        get => (bool)GetValue(IsEdgeProperty);
        set => SetValue(IsEdgeProperty, value);
    }

    public static readonly DependencyProperty CellContentTypeProperty = DependencyProperty.Register("CellContentType",
        typeof(CellContentType), typeof(GridTableCell), new PropertyMetadata(default(CellContentType)));

    public CellContentType CellContentType
    {
        get => (CellContentType)GetValue(CellContentTypeProperty);
        set => SetValue(CellContentTypeProperty, value);
    }

    public static readonly DependencyProperty CellContentProperty = DependencyProperty.Register("CellContent",
        typeof(string), typeof(GridTableCell), new PropertyMetadata(default(string)));

    public string CellContent
    {
        get => (string)GetValue(CellContentProperty);
        set => SetValue(CellContentProperty, value);
    }

    public static readonly DependencyProperty CoordinatesProperty = DependencyProperty.Register("Coordinates",
        typeof(MinecraftCoordinates), typeof(GridTableCell), new PropertyMetadata(default(MinecraftCoordinates)));

    public MinecraftCoordinates Coordinates
    {
        get => (MinecraftCoordinates)GetValue(CoordinatesProperty);
        set => SetValue(CoordinatesProperty, value);
    }

    public static readonly RoutedEvent EditEvent = EventManager.RegisterRoutedEvent(
        "Edit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GridTableCell));

    public event RoutedEventHandler Edit
    {
        add => AddHandler(EditEvent, value);
        remove => RemoveHandler(EditEvent, value);
    }

    public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent(
        "Delete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GridTableCell));

    public event RoutedEventHandler Delete
    {
        add => AddHandler(DeleteEvent, value);
        remove => RemoveHandler(DeleteEvent, value);
    }

    public GridTableCell()
    {
        InitializeComponent();
    }

    private void OnEdit(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(EditEvent));
    }

    private void OnDelete(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(DeleteEvent));
    }
}