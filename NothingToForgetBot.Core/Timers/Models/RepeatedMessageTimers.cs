using Timer = System.Timers.Timer;

namespace NothingToForgetBot.Core.Timers.Models;

public class RepeatedMessageTimers
{
    public Timer RepeatedTimer { get; set; }

    public Timer EndTimer { get; set; }
}