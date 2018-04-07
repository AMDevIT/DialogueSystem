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
        
        #region IDs

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

        public string DidEnterNodeID
        {
            get;
            protected set;
        }

        public string DidExitNodeID
        {
            get;
            protected set;
        }

        #endregion

        public Conversation ParentConversation
        {
            get;
            protected set;
        }

        public ConversationChoice[] Choices
        {
            get
            {
                if (this.choices.Count > 0)
                    return this.choices.Values.ToArray();
                else
                    return new ConversationChoice[] { };
            }
        }

        public string Text
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
            
            this.ParentConversation = parent;
            this.ID = id;
            this.CharacterID = characterID;
            this.TextID = textID;

            if (this.ParentConversation.ParentConversationManager != null)
                this.Text = this.ParentConversation.ParentConversationManager.GetLocalizedString(this.TextID);
        }

        #endregion

        #region Methods

        public void UpdateText(string text)
        {
            this.Text = text;
        }

        public void ChoiceSelected(string id)
        {
            DialogueSystemCallbackDelegate callbackDelegate = null;
            ConversationChoice conversationChoice = null;

            if (this.choices.ContainsKey(id))
            {
                conversationChoice = this.choices[id];

                if (!String.IsNullOrEmpty(conversationChoice.NavigateTo))
                {
                    // All selected is overriden by NavigateTo
                    this.ParentConversation.ExecuteNode(conversationChoice.NavigateTo);
                }
                else
                {
                    if (!String.IsNullOrEmpty(conversationChoice.OnSelectedID))
                        callbackDelegate = this.ParentConversation.ParentConversationManager.GetMethodDelegate(conversationChoice.OnSelectedID) as DialogueSystemCallbackDelegate;
                    else
                    {
                        if (!String.IsNullOrEmpty(this.ParentConversation.DefaultOnSelectedID))
                            callbackDelegate = this.ParentConversation.ParentConversationManager.GetMethodDelegate(this.ParentConversation.DefaultOnSelectedID) as DialogueSystemCallbackDelegate;
                    }
                }
            }

            callbackDelegate?.Invoke(this.ParentConversation.ParentConversationManager, id);
        }

        internal void AddChoice(ConversationChoice conversationChoice)
        {            
            this.choices.Add(conversationChoice.ID, conversationChoice);
        }

        #endregion
    }
}
