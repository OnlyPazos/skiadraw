using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace skiadraw.Models;

public abstract class DebugItem
{
}

public class DebugInfo : DebugItem
{
    public required string Name { get; init; }
    public required string Type { get; init; }

    public required Func<object?> GetValue { get; init; }

    public object? Value => GetValue();
}

public class DebugSeparator : DebugItem
{
    public string? Title { get; set; }
}