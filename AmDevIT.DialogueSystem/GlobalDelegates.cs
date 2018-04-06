using System;

namespace AmDevIT.Games.DialogueSystem
{
    public delegate void DialogueSystemCallback(ConversationsManager manager, object localState);
    public delegate bool DialogueSystemCanExecuteDelegate(ConversationsManager manager, object localState);
}