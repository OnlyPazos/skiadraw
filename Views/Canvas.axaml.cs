using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using skiadraw.Models;
using skiadraw.ViewModels;

namespace skiadraw.Views;

public partial class Canvas : UserControl
{
    private CanvasViewModel? Vm => DataContext as CanvasViewModel;

    private TranslateTransform PanTransform =>
        (TranslateTransform)((TransformGroup)CanvasRoot.RenderTransform!).Children[1];

    public Canvas()
    {
        InitializeComponent();
        Viewport.AddHandler(PointerPressedEvent, Viewport_OnPointerPressed,  RoutingStrategies.Tunnel);
    }

    private void Shape_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is not Control { DataContext: ShapeModel shape } control) return;
        
        var point = e.GetPosition(DrawCanvas);
        Vm?.HandleShapePressed(shape, point);
    }

    private void Shape_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetPosition(DrawCanvas);
        Vm?.HandleShapeMoved(point);
    }

    private void Shape_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Vm?.HandleShapeReleased();
    }

    private void Canvas_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(DrawCanvas);
        Vm?.HandlePointerPressed(point);
    }

    private void Canvas_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetPosition(DrawCanvas);
        Vm?.HandlePointerMoved(point);
    }

    private void Canvas_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var point = e.GetPosition(DrawCanvas);
        Vm?.HandlePointerReleased(point);
    }

    private void Canvas_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        Vm?.HandleShapePointerEntered(sender, e);
    }

    private void Canvas_OnPointerExited(object? sender, PointerEventArgs e)
    {
        Vm?.HandleShapePointerExited(sender, e);
    }
    
    private void Viewport_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Vm?.HandleViewportPointerPressed(Viewport, PanTransform, e);
    }

    private void Viewport_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        Vm?.HandleViewportPointerMoved(Viewport, PanTransform, e);
    }

    private void Viewport_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Vm?.HandleViewportPointerReleased(sender, e);
    }

    private void Viewport_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        Vm?.HandleViewportPointerWheelChanged(Viewport, PanTransform, e);
    }
}