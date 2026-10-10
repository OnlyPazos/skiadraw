using Avalonia;
using Avalonia.Controls;

namespace skiadraw.Helpers;

public static class ResourceHelper
{
    public static T GetResource<T>(string key, T fallback)
    {
        var app = Application.Current;
        if (app is not null &&
            app.TryFindResource(key, app.ActualThemeVariant, out var value) &&
            value is T typed)
        {
            return typed;
        }

        return fallback;
    }
}