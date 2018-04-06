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

        public event EventHandler DialogChoiceSelected;

        #endregion

        #region Fields

        #endregion

        #region Methods

        [DialogDelegate(ID = "testConversationInitScript")]
        public void InitConversation(ConversationsManager manager, object state)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));            
        }

        [DialogDelegate(ID = "testConversationOnSelected")]
        public void OnChoiceSelected(ConversationsManager manager, object state)
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
                this.OnChoiceSelected(manager, state);
            }
        }


        [DialogDelegate(ID = "testConversationCanShow", DelegateType = DialogDelegatesTypes.CanExecute)]
        public bool DefaultCanShow(ConversationsManager manager, object state)
        {
            return true;
        }

        protected void OnDialogChoceSelected()
        {
            this.DialogChoiceSelected?.Invoke(this, EventArgs.Empty);
        }        

        #endregion
    }
}
