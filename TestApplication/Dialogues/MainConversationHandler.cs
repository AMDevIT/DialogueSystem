using AmDevIT.Games.DialogueSystem;
using AmDevIT.Games.DialogueSystem.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Dialogues
{
    public class MainConversationHandler
        : ConversationHandlerBase
    {
        #region Events

        public event EventHandler StartConversationEnded;
        public event EventHandler DidEnterNode;
        public event EventHandler DidExitNode;
        public event EventHandler DialogChoiceSelected;

        #endregion

        #region Fields

        #endregion

        #region Methods

        public override void OnStartConversation(ConversationsManager manager, object state)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));            
        }

        public override void DefaultDidEnterNode(ConversationsManager manager, object state)
        {            
        }

        public override void DefaultDidExitNode(ConversationsManager manager, object state)
        {
        }

        public override void DefaultOnChoiceSelected(ConversationsManager manager, object state)
        {
            string id = state as String;

            switch(id)
            {
                case "text_sn_c1":
                    // Wrong call.
                    this.OnChoiceCN1Selected(manager, state);
                    break;
            }
        }
        
        public override bool DefaultCanShow(ConversationsManager manager, object state)
        {
            return true;
        }

        [DialogDelegate(ID = "testConversationOnSelectedC1")]
        public void OnChoiceCN1Selected(ConversationsManager manager, object state)
        {
            string id = state as String;

            if (id == "text_sn_c1")
            {
                // Ok
            }
            else
            {
                // Wrong call. We can fallback on the default handler.
                this.DefaultOnChoiceSelected(manager, state);
            }
        }

        #region Events 

        protected void OnStartConversationEnded()
        {
            this.StartConversationEnded?.Invoke(this, EventArgs.Empty);
        }
        protected void OnDidEnterNode()
        {
            this.DidEnterNode?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDidExitNode()
        {
            this.DidExitNode?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDialogChoceSelected()
        {
            this.DialogChoiceSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #endregion
    }
}
