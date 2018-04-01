using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem
{
    public class ConversationNode
    {
        #region Fields

        private readonly Dictionary<string, ConversationChoice> choices = new Dictionary<string, ConversationChoice>();

        #endregion

        #region Properties

        public string ID
        {
            get;
            protected set;
        }

        public string CharacterID
        {
            get;
            protected set;
        }

        public string TextID
        {
            get;
            protected set;
        }

        #endregion
    }
}
