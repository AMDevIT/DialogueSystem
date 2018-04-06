using System;

namespace AmDevIT.Games.DialogueSystem
{
    public delegate void DialogueSystemCallbackDelegate(ConversationsManager manager, object localState);
    public delegate bool DialogueSystemCanExecuteDelegate(ConversationsManager manager, object localState);
}