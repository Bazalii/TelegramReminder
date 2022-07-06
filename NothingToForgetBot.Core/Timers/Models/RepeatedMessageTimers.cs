namespace NothingToForgetBot.Core.Timers.Models;
using Timer = System.Timers.Timer;


public class RepeatedMessageTimers
{
    public Timer RepeatedTimer { get; set; }

    public Timer EndTimer { get; set; }
}