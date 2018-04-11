using AmDevIT.Games.DialogueSystem;
using AmDevIT.Games.DialogueSystem.Reflection;
using AmDevIT.Games.DialogueSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApplication.Dialogues
{
    public class ForestConversationHandler
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

            manager.ExecuteNode(manager.RunningConversation.DefaultRootNodeID);
        }

        public override void DefaultDidEnterNode(ConversationsManager manager, object state)
        {
            this.OnDidEnterNode();
        }

        public override void DefaultDidExitNode(ConversationsManager manager, object state)
        {
            this.OnDidExitNode();
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

                case "sn_c1":
                    // Selected sn_c1
                    break;

                case ReservedIdentifiers.DefaultContinueConversationChoiceID:
                default:
                    manager.EndCurrentConversation();
                    break;
            }
        }
        
        public override bool DefaultCanShow(ConversationsManager manager, object state)
        {
            return true;
        }

        [DialogDelegate(ID = "sn_c1_CanShow", DelegateType = DialogDelegatesTypes.CanExecute)]
        public bool SnC1CanShow(ConversationsManager manager, object state)
        {
            return true;
        }

        [DialogDelegate(ID = "sn_c1_OnSelected")]
        public void OnChoiceCN1Selected(ConversationsManager manager, object state)
        {
            string id = state as String;

            if (id == "sn_c1")
            {
                // We want to rest so?
                manager.ExecuteNode("node_002");
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
