﻿using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using NothingToForgetBot.Core.Exceptions;

namespace NothingToForgetBot.Core.Messages.Parsers.Implementations;

public class MessageParser : IMessageParser
{
    private readonly string _resourcesFile;

    public MessageParser(string resourcesFile)
    {
        _resourcesFile = resourcesFile;
    }

    public Message Parse(string message, string localisation)
    {
        var correctTypedLocalisation = localisation[0].ToString().ToUpper() + localisation[1..localisation.Length];

        var resourceReader = new ResXResourceReader(_resourcesFile);

        var timeSeparatorResourceValue = resourceReader.GetString("TimeSeparator");
        var dateSeparatorResourceValue = resourceReader.GetString("DateSeparator");

        var inResourceValue = resourceReader.GetString(correctTypedLocalisation + "In");
        var atResourceValue = resourceReader.GetString(correctTypedLocalisation + "At");
        var everyResourceValue = resourceReader.GetString(correctTypedLocalisation + "Every");
        var untilResourceValue = resourceReader.GetString(correctTypedLocalisation + "Until");
        var minutesResourceValue = resourceReader.GetString(correctTypedLocalisation + "Minutes");
        var secondsResourceValue = resourceReader.GetString(correctTypedLocalisation + "Seconds");

        var publishingDateIsEarlierThanNowExceptionMessage =
            resourceReader.GetString(correctTypedLocalisation + "PublishingDateIsEarlierThanNowExceptionMessage");
        var endDateIsEarlierThanNowExceptionMessage =
            resourceReader.GetString(correctTypedLocalisation + "EndDateIsEarlierThanNowExceptionMessage");
        var messageNotSupportedExceptionMessage =
            resourceReader.GetString(correctTypedLocalisation + "MessageNotSupportedExceptionMessage");

        var messageScheduledInMinutesRegex = new Regex($".+\\s+{inResourceValue}\\s\\d+\\s+{minutesResourceValue}.+");
        var messageScheduledInSecondsRegex = new Regex($".+\\s+{inResourceValue}\\s\\d+\\s+{secondsResourceValue}.+");
        var messageWithPublishingTimeRegex = new Regex($".+\\s+\\d+\\.\\d+\\s{atResourceValue}\\s\\d+:\\d+\\s?.+");
        var repeatedViaMinutesScheduledMessageRegex = new Regex(
            $".+\\s+{everyResourceValue}\\s\\d+\\s{minutesResourceValue}.?\\s{untilResourceValue}\\s\\d+:\\d+\\s?.+");
        var repeatedViaSecondsScheduledMessageRegex = new Regex(
            $".+\\s+{everyResourceValue}\\s\\d+\\s{secondsResourceValue}.?\\s{untilResourceValue}\\s\\d+:\\d+\\s?.+");

        if (messageScheduledInMinutesRegex.IsMatch(message))
        {
            var indexOfMinutesWord = message.LastIndexOf(minutesResourceValue, StringComparison.Ordinal);
            var indexOfIn = message[..indexOfMinutesWord].LastIndexOf(inResourceValue, StringComparison.Ordinal);


            var content = message[..(indexOfIn - 1)];
            var publishingDate = DateTime.Now +
                                 TimeSpan.FromMinutes(
                                     Convert.ToInt32(
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
            var publishingDate = DateTime.Now +
                                 TimeSpan.FromSeconds(
                                     Convert.ToInt32(
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
            var year = DateTime.Now.Year;

            var publishingDate = new DateTime(year, month, day, hour, minute, second);

            if (publishingDate < DateTime.Now)
            {
                throw new DateIsEarlierThanNowException(publishingDateIsEarlierThanNowExceptionMessage);
            }

            return new ScheduledMessage
            {
                Content = content,
                PublishingDate = publishingDate
            };
        }

        if (repeatedViaMinutesScheduledMessageRegex.IsMatch(message))
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
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);

            if (endDate < DateTime.Now)
            {
                throw new DateIsEarlierThanNowException(endDateIsEarlierThanNowExceptionMessage);
            }

            return new RepeatedViaMinutesScheduledMessage
            {
                Content = content,
                Interval = interval,
                EndDate = endDate
            };
        }

        if (repeatedViaSecondsScheduledMessageRegex.IsMatch(message))
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
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);

            if (endDate < DateTime.Now)
            {
                throw new DateIsEarlierThanNowException(endDateIsEarlierThanNowExceptionMessage);
            }

            return new RepeatedViaSecondsScheduledMessage
            {
                Content = content,
                Interval = interval,
                EndDate = endDate
            };
        }

        throw new MessageNotSupportedException(messageNotSupportedExceptionMessage);
    }
}