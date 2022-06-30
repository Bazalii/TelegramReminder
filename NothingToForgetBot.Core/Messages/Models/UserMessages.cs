﻿namespace NothingToForgetBot.Core.Messages.Models;

public class UserMessages
{
    public List<ScheduledMessage> ScheduledMessages { get; set; }
    
    public List<RepeatedViaMinutesScheduledMessage> RepeatedViaMinutesScheduledMessages { get; set; }
    
    public List<RepeatedViaSecondsScheduledMessage> RepeatedViaSecondsScheduledMessages { get; set; }
}