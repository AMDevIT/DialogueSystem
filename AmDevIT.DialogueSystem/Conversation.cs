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
                          string defaultCanShowID)
            : this(manager, id, defaultRootNodeID, 
                   initConversationScriptID, defaultOnSelectedID, 
                   defaultCanShowID, null)
        {

        }

        internal Conversation(ConversationsManager manager, 
                              string id, 
                              string defaultRootNodeID,
                              string initConversationScriptID,
                              string defaultOnSelectedID,
                              string defaultCanShowID, 
                              ConversationNode[] conversationNodes)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            if (String.IsNullOrEmpty(defaultRootNodeID))
                throw new ArgumentNullException(nameof(defaultRootNodeID));            
            
            this.Manager = manager;
            this.ID = id;
            this.defaultRootNodeID = defaultRootNodeID;
            this.InitConversationScriptID = initConversationScriptID;
            this.DefaultOnSelectedID = defaultOnSelectedID;
            this.DefaultCanShowID = defaultCanShowID;

            this.AddConversationNodes(conversationNodes);
        }

        #endregion

        #region Methods      

        internal void AddConversationNode(ConversationNode node)
        {
            if (node != null)            
                this.conversationNodesDictionary.Add(node.ID, node);            
        }

        internal void AddConversationNodes(ConversationNode[] nodes)
        {
            if (nodes != null && nodes.Length > 0)
            {
                foreach (ConversationNode currentNode in nodes)
                    this.conversationNodesDictionary.Add(currentNode.ID, currentNode);
            }
        }

        #endregion
    }
}
