﻿using MC.Toast.Configuration;
using MC.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace MC.Toast;

public partial class McToast : IDisposable
{
    [CascadingParameter] private McToasts ToastsContainer { get; set; } = default!;

    [Parameter, EditorRequired] public Guid ToastId { get; set; }
    [Parameter, EditorRequired] public ToastSettings Settings { get; set; } = default!;
    [Parameter] public ToastLevel? Level { get; set; }
    [Parameter] public RenderFragment? Message { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private RenderFragment? CloseButtonContent => ToastsContainer.CloseButtonContent;

    private CountdownTimer? _countdownTimer;
    private int _progress = 100;

    protected override async Task OnInitializedAsync()
    {
        if (Settings.DisableTimeout ?? false)
        {
            return;
        }
        
        if (Settings.ShowProgressBar!.Value)
        {
            _countdownTimer = new CountdownTimer(Settings.Timeout, Settings.ExtendedTimeout!.Value)
                .OnTick(CalculateProgressAsync)
                .OnElapsed(Close);
        }
        else
        {
            _countdownTimer = new CountdownTimer(Settings.Timeout, Settings.ExtendedTimeout!.Value)
                .OnElapsed(Close);
        }

        await _countdownTimer.StartAsync();
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastsContainer.RemoveToast(ToastId);

    private void TryPauseCountdown()
    {
        if (Settings.PauseProgressOnHover!.Value)
        {
            Settings.ShowProgressBar= false;
            _countdownTimer?.Pause();
        }
    }

    private void TryResumeCountdown()
    {        
        if (Settings.PauseProgressOnHover!.Value )
        {
            Settings.ShowProgressBar = true;
            _countdownTimer?.UnPause();
        }
    }

    private async Task CalculateProgressAsync(int percentComplete)
    {
        _progress = 100 - percentComplete;
        await InvokeAsync(StateHasChanged);
    }

    private void ToastClick()
        => Settings.OnClick?.Invoke();

    private bool ShowIconDiv() 
        => Settings.IconType switch
        {
            IconType.None => false,
            IconType.Mc => true,
            IconType.FontAwesome => !string.IsNullOrWhiteSpace(Settings.Icon),
            IconType.Material => !string.IsNullOrWhiteSpace(Settings.Icon),
            _ => false
        };

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;
    }
}