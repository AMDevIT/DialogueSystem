using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem
{
    public class Conversation
    {
        #region Fields      

        protected string defaultRootNodeID = null;
        protected string textID = null;
        protected string text = null;
        
        #endregion

        #region Properties

        protected ConversationsManager Manager
        {
            get;
            set;
        }

        public string ID
        {
            get;
            protected set;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            protected set
            {
                this.text = value;
            }
        }

        #endregion

        #region .ctor

        internal Conversation(ConversationsManager manager, 
                              string id, 
                              string textID,
                              string defaultRootNodeID)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            this.Manager = manager;
            this.ID = id;
            this.defaultRootNodeID = defaultRootNodeID;
        }

        #endregion

        #region Methods      

        #endregion
    }
}
