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

        public Conversation Parent
        {
            get;
            protected set;
        }

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

        #region .ctor

        internal ConversationNode(Conversation parent, string id, string characterID, string textID)
        {
            if (parent == null)
                throw new ArgumentNullException("Parent cannot be null.");

            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Conversation node ID cannot be null");
            
            this.Parent = parent;
            this.ID = id;
            this.CharacterID = characterID;
            this.TextID = textID;
        }

        #endregion

        #region Methods

        public void UpdateTextID(string textID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
