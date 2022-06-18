using NothingToForgetBot.Core.Notes.Models;

namespace NothingToForgetBot.Core.Notes.Handlers;

public interface INoteHandler
{
    Task Handle(Note note);
}