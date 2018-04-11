using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem
{
    public class ConversationChoice
    {
        #region Properties

        public ConversationNode ParentNode
        {
            get;
            protected set;
        }

        #region IDs

        public string ID
        {
            get;
            protected set;
        }

        public string TextID
        {
            get;
            protected set;
        }        

        public string CanShowID
        {
            get;
            protected set;
        }

        public string OnSelectedID
        {
            get;
            protected set;
        }

        public string NavigateTo
        {
            get;
            protected set;
        }

        #endregion

        public bool CanShow
        {
            get
            {
                bool result = true;

                DialogueSystemCanExecuteDelegate canShowDelegate = null;

                if (!String.IsNullOrEmpty(this.CanShowID))
                    canShowDelegate = this.ParentNode.ParentConversation.ParentConversationManager.GetMethodDelegate(this.CanShowID) as DialogueSystemCanExecuteDelegate;
                else
                {
                    if (!String.IsNullOrEmpty(this.ParentNode.ParentConversation.DefaultCanShowID))
                        canShowDelegate = this.ParentNode.ParentConversation.ParentConversationManager.GetMethodDelegate(this.ParentNode.ParentConversation.DefaultCanShowID) as DialogueSystemCanExecuteDelegate;
                }

                if (canShowDelegate != null)
                    result = canShowDelegate.Invoke(this.ParentNode.ParentConversation.ParentConversationManager, this.ID);
                return result;
            }
        }

        public string Text
        {
            get;
            protected set;
        }

        #endregion

        #region .ctor

        internal ConversationChoice(ConversationNode parent, 
                                    string id,
                                    string textID,
                                    string canShowID,
                                    string onSelectedID,
                                    string navigateTo)
        {
            this.ParentNode = parent;
            this.ID = id;
            this.TextID = textID;
            this.CanShowID = canShowID;
            this.OnSelectedID = onSelectedID;
            this.NavigateTo = navigateTo;

            if (this.ParentNode.ParentConversation != null && this.ParentNode.ParentConversation.ParentConversationManager != null)
                this.Text = this.ParentNode.ParentConversation.ParentConversationManager.GetLocalizedString(textID);
        }

        #endregion

        #region Methods

        public void SetText(string text)
        {
            this.Text = text;
        }

        #endregion
    }
}
