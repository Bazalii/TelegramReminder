﻿using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.Messages.Repositories;

namespace NothingToForgetBot.Core.Messages.Selectors.Implementations;

public class UserMessageSelector : IUserMessageSelector
{
    private readonly IScheduledMessageRepository _scheduledMessageRepository;
    private readonly IRepeatedViaMinutesMessageRepository _repeatedViaMinutesMessageRepository;
    private readonly IRepeatedViaSecondsMessageRepository _repeatedViaSecondsMessageRepository;

    public UserMessageSelector(IScheduledMessageRepository scheduledMessageRepository,
        IRepeatedViaMinutesMessageRepository repeatedViaMinutesMessageRepository,
        IRepeatedViaSecondsMessageRepository repeatedViaSecondsMessageRepository)
    {
        _scheduledMessageRepository = scheduledMessageRepository;
        _repeatedViaMinutesMessageRepository = repeatedViaMinutesMessageRepository;
        _repeatedViaSecondsMessageRepository = repeatedViaSecondsMessageRepository;
    }

    public async Task<UserMessages> Select(long chatId, CancellationToken cancellationToken)
    {
        var scheduledMessages = await _scheduledMessageRepository.GetAllByChatId(chatId, cancellationToken);

        var repeatedViaMinutesMessages =
            await _repeatedViaMinutesMessageRepository.GetAllByChatId(chatId, cancellationToken);

        var repeatedViaSecondsMessages =
            await _repeatedViaSecondsMessageRepository.GetAllByChatId(chatId, cancellationToken);

        scheduledMessages.Sort((x, y) => x.PublishingDate.CompareTo(y.PublishingDate));

        repeatedViaMinutesMessages.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));

        repeatedViaSecondsMessages.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));

        return new UserMessages
        {
            ScheduledMessages = scheduledMessages,
            RepeatedViaMinutesMessages = repeatedViaMinutesMessages,
            RepeatedViaSecondsMessages = repeatedViaSecondsMessages
        };
    }
}