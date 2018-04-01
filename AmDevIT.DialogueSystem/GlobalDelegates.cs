using System;

namespace AmDevIT.Games.DialogueSystem
{
    public delegate void DialogueSystemCallback(object state);
    public delegate bool DialogueSystemCanExecuteDelegate(object state);
}