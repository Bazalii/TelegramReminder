using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using NothingToForgetBot.Core.Exceptions;
using NothingToForgetBot.Core.Messages.Models;
using NothingToForgetBot.Core.TimeZones.Repositories;

namespace NothingToForgetBot.Core.Messages.Parsers.Implementations;

public class MessageParser : IMessageParser
{
    private readonly ResXResourceReader _resourceReader;

    private readonly ITimeZoneRepository _timeZoneRepository;

    public MessageParser(ResXResourceReader resourceReader, ITimeZoneRepository timeZoneRepository)
    {
        _resourceReader = resourceReader;
        _timeZoneRepository = timeZoneRepository;
    }

    public async Task<Message> Parse(long chatId, string message, string localisation,
        CancellationToken cancellationToken)
    {
        var timeSeparatorResourceValue = _resourceReader.GetString("TimeSeparator");
        var dateSeparatorResourceValue = _resourceReader.GetString("DateSeparator");

        var inResourceValue = _resourceReader.GetString(localisation + "In");
        var atResourceValue = _resourceReader.GetString(localisation + "At");
        var everyResourceValue = _resourceReader.GetString(localisation + "Every");
        var untilResourceValue = _resourceReader.GetString(localisation + "Until");
        var minutesResourceValue = _resourceReader.GetString(localisation + "Minutes");
        var secondsResourceValue = _resourceReader.GetString(localisation + "Seconds");

        var publishingDateIsEarlierThanNowExceptionMessage =
            _resourceReader.GetString(localisation + "PublishingDateIsEarlierThanNowExceptionMessage");
        var endDateIsEarlierThanNowExceptionMessage =
            _resourceReader.GetString(localisation + "EndDateIsEarlierThanNowExceptionMessage");

        var messageScheduledInMinutesRegex = new Regex($".+\\s+{inResourceValue}\\s\\d+\\s+{minutesResourceValue}.+");
        var messageScheduledInSecondsRegex = new Regex($".+\\s+{inResourceValue}\\s\\d+\\s+{secondsResourceValue}.+");
        var messageWithPublishingTimeRegex = new Regex($".+\\s+\\d+\\.\\d+\\s{atResourceValue}\\s\\d+:\\d+\\s?.+");
        var repeatedViaMinutesMessageRegex = new Regex(
            $".+\\s+{everyResourceValue}\\s\\d+\\s{minutesResourceValue}.?\\s{untilResourceValue}\\s\\d+:\\d+\\s?.+");
        var repeatedViaSecondsMessageRegex = new Regex(
            $".+\\s+{everyResourceValue}\\s\\d+\\s{secondsResourceValue}.?\\s{untilResourceValue}\\s\\d+:\\d+\\s?.+");

        var chatTimeZone = await _timeZoneRepository.GetByChatId(chatId, cancellationToken);
        var timeZone = chatTimeZone.TimeZone;

        if (messageScheduledInMinutesRegex.IsMatch(message))
        {
            var indexOfMinutesWord = message.LastIndexOf(minutesResourceValue, StringComparison.Ordinal);
            var indexOfIn = message[..indexOfMinutesWord].LastIndexOf(inResourceValue, StringComparison.Ordinal);

            var content = message[..(indexOfIn - 1)];
            var publishingDate = DateTime.UtcNow +
                                 TimeSpan.FromMinutes(Convert.ToInt32(
                                     message[(indexOfIn + inResourceValue.Length + 1)..(indexOfMinutesWord - 1)]));

            return new ScheduledMessage
            {
                Content = content,
                PublishingDate = publishingDate
            };
        }

        if (messageScheduledInSecondsRegex.IsMatch(message))
        {
            var indexOfIn = message.LastIndexOf(inResourceValue, StringComparison.Ordinal);
            var indexOfSecondsWord = message.LastIndexOf(secondsResourceValue, StringComparison.Ordinal);

            var content = message[..(indexOfIn - 1)];
            var publishingDate = DateTime.UtcNow +
                                 TimeSpan.FromSeconds(Convert.ToInt32(
                                     message[(indexOfIn + inResourceValue.Length + 1)..(indexOfSecondsWord - 1)]));

            return new ScheduledMessage
            {
                Content = content,
                PublishingDate = publishingDate
            };
        }

        if (messageWithPublishingTimeRegex.IsMatch(message))
        {
            var indexOfAt = message.LastIndexOf(atResourceValue, StringComparison.Ordinal);
            var indexOfDateSeparator = message.LastIndexOf(dateSeparatorResourceValue, StringComparison.Ordinal);
            var indexOfTimeSeparator = message.LastIndexOf(timeSeparatorResourceValue, StringComparison.Ordinal);
            var indexOfDay = indexOfDateSeparator - 2;
            var indexOfMonth = indexOfDateSeparator + 1;

            var content = message[..(indexOfDay - 1)];

            var second = 0;
            var hour = Convert.ToInt32(
                message[(indexOfAt + atResourceValue.Length + 1)..indexOfTimeSeparator]);
            var minute =
                Convert.ToInt32(
                    message[(indexOfTimeSeparator + 1)..(indexOfTimeSeparator + 3)]);
            var day = Convert.ToInt32(message[indexOfDay..indexOfDateSeparator]);
            var month = Convert.ToInt32(message[indexOfMonth..(indexOfDateSeparator + 2)]);
            var year = DateTime.UtcNow.Year;

            var publishingDate = new DateTime(year, month, day, hour, minute, second) - TimeSpan.FromHours(timeZone);

            if (publishingDate < DateTime.UtcNow)
            {
                throw new DateIsEarlierThanNowException(publishingDateIsEarlierThanNowExceptionMessage);
            }

            return new ScheduledMessage
            {
                Content = content,
                PublishingDate = publishingDate
            };
        }

        if (repeatedViaMinutesMessageRegex.IsMatch(message))
        {
            var indexOfEvery = message.LastIndexOf(everyResourceValue, StringComparison.Ordinal);
            var indexOfUntil = message.LastIndexOf(untilResourceValue, StringComparison.Ordinal);
            var indexOfMinutes = message.LastIndexOf(minutesResourceValue, StringComparison.Ordinal);
            var indexOfTimeSeparator = message.LastIndexOf(timeSeparatorResourceValue, StringComparison.Ordinal);

            var content = message[..(indexOfEvery - 1)];

            var interval =
                Convert.ToInt32(message[(indexOfEvery + everyResourceValue.Length + 1)..(indexOfMinutes - 1)]);

            var second = 0;
            var hour = Convert.ToInt32(message[(indexOfUntil + untilResourceValue.Length + 1)..indexOfTimeSeparator]);
            var minute = Convert.ToInt32(message[(indexOfTimeSeparator + 1)..(indexOfTimeSeparator + 3)]);

            var endDate =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second) -
                TimeSpan.FromHours(timeZone);

            endDate = endDate.ToUniversalTime();

            if (endDate < DateTime.UtcNow)
            {
                throw new DateIsEarlierThanNowException(endDateIsEarlierThanNowExceptionMessage);
            }

            return new RepeatedViaMinutesMessage
            {
                Content = content,
                Interval = interval,
                EndDate = endDate
            };
        }

        if (repeatedViaSecondsMessageRegex.IsMatch(message))
        {
            var indexOfEvery = message.LastIndexOf(everyResourceValue, StringComparison.Ordinal);
            var indexOfUntil = message.LastIndexOf(untilResourceValue, StringComparison.Ordinal);
            var indexOfSeconds = message.LastIndexOf(secondsResourceValue, StringComparison.Ordinal);
            var indexOfTimeSeparator = message.LastIndexOf(timeSeparatorResourceValue, StringComparison.Ordinal);

            var content = message[..(indexOfEvery - 1)];

            var interval =
                Convert.ToInt32(message[(indexOfEvery + everyResourceValue.Length + 1)..(indexOfSeconds - 1)]);

            var second = 0;
            var hour = Convert.ToInt32(message[(indexOfUntil + untilResourceValue.Length + 1)..indexOfTimeSeparator]);
            var minute = Convert.ToInt32(message[(indexOfTimeSeparator + 1)..(indexOfTimeSeparator + 3)]);

            var endDate =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second) +
                TimeSpan.FromHours(timeZone);

            endDate = endDate.ToUniversalTime();

            if (endDate < DateTime.UtcNow)
            {
                throw new DateIsEarlierThanNowException(endDateIsEarlierThanNowExceptionMessage);
            }

            return new RepeatedViaSecondsMessage
            {
                Content = content,
                Interval = interval,
                EndDate = endDate
            };
        }

        throw new NotScheduledMessageException("Message is not scheduled!");
    }
}