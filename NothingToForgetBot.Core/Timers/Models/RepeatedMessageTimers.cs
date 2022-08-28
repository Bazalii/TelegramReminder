using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Timers.Models;

public class RepeatedMessageTimers
{
    public Timer RepeatedTimer { get; init; } = new();

    public Timer EndTimer { get; init; } = new();
}