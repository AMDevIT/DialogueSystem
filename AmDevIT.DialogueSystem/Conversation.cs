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

        private readonly Dictionary<string, ConversationNode> conversationNodesDictionary = new Dictionary<string, ConversationNode>();

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

        public string DefaultRootNodeID
        {
            get
            {
                return this.defaultRootNodeID;
            }
        }

        public string CurrentCharacterID
        {
            get;
            protected set;
        }

        public string CurrentSpeechText
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

        public string InitConversationScriptID
        {
            get;
            set;
        }

        public string DefaultOnSelectedID
        {
            get;
            set;
        }

        public string DefaultCanShowID
        {
            get;
            set;
        }

        #endregion

        #region .ctor

        internal Conversation(ConversationsManager manager, 
                              string id, 
                              string defaultRootNodeID,
                              string initConversationScriptID,
                              string defaultOnSelectedID,
                              string defaultCanShowID, 
                              KeyValuePair<string, ConversationNode>[] conversationNodes)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            if (String.IsNullOrEmpty(defaultRootNodeID))
                throw new ArgumentNullException(nameof(defaultRootNodeID));

            if (conversationNodes == null)
                throw new ArgumentNullException(nameof(conversationNodes));

            if (conversationNodes.Length == 0)
                throw new ArgumentException(nameof(conversationNodes)); 

            this.Manager = manager;
            this.ID = id;
            this.defaultRootNodeID = defaultRootNodeID;
            this.InitConversationScriptID = initConversationScriptID;
            this.DefaultOnSelectedID = defaultOnSelectedID;
            this.DefaultCanShowID = defaultCanShowID;

            foreach (KeyValuePair<string, ConversationNode> currentNode in conversationNodes)
                this.conversationNodesDictionary.Add(currentNode.Key, currentNode.Value);
        }

        #endregion

        #region Methods      

        #endregion
    }
}
