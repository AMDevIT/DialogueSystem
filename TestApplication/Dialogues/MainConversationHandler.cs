using AmDevIT.Games.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Dialogues
{
    public class MainConversationHandler
    {
        #region Events

        public event EventHandler DialogChoiceSelected;

        #endregion

        #region Fields

        #endregion

        #region Methods

        public void InitConversation(ConversationsManager manager, object state)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));            
        }

        public void OnChoiceSelected(ConversationsManager manager, object state)
        {

        }

        protected void OnDialogChoceSelected()
        {
            this.DialogChoiceSelected?.Invoke(this, EventArgs.Empty);
        }        

        #endregion
    }
}
