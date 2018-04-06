using System;

namespace AmDevIT.Games.DialogueSystem.Reflection
{
    public class DialogDelegateAttribute
        : Attribute
    {
        #region Properties

        public string ID
        {
            get;
            set;
        }

        public DialogDelegatesTypes DelegateType
        {
            get;
            set;
        }

        #endregion        
    }
}
