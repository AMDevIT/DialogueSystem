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

        private ConversationNode previousNode = null;
        private ConversationNode currentNode = null;

        protected string defaultRootNodeID = null;
        protected string textID = null;
        protected string text = null;
                
        #endregion

        #region Properties

        public ConversationsManager ParentConversationManager
        {
            get;
            protected set;
        }

        #region Objects IDs

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

        public string OnStartConversationID
        {
            get;
            set;
        }

        public string DefaultDidEnterNodeID
        {
            get;
            set;
        }

        public string DefaultDidExitNodeID
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

        public ConversationNode PreviousNode
        {
            get
            {
                return this.previousNode;
            }
            set
            {
                this.previousNode = value;
            }
        }

        public ConversationNode CurrentNode
        {
            get
            {
                return this.currentNode;
            }
            protected set
            {
                this.currentNode = value;
            }
        }
        
        #endregion

        #region .ctor

        internal Conversation(ConversationsManager manager,
                          string id,
                          string defaultRootNodeID,
                          string onStartConversationID,
                          string didEnterNodeID,
                          string didExitNodeID,
                          string defaultOnSelectedID,
                          string defaultCanShowID)
            : this(manager, 
                   id, 
                   defaultRootNodeID, 
                   onStartConversationID, 
                   didEnterNodeID,
                   didExitNodeID,
                   defaultOnSelectedID, 
                   defaultCanShowID, null)
        {

        }

        internal Conversation(ConversationsManager manager, 
                              string id, 
                              string defaultRootNodeID,
                              string onStartConversationID,
                              string defaultDidEnterNodeID,
                              string defaultDidExitNodeID,
                              string defaultOnSelectedID,
                              string defaultCanShowID, 
                              ConversationNode[] conversationNodes)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            if (String.IsNullOrEmpty(defaultRootNodeID))
                throw new ArgumentNullException(nameof(defaultRootNodeID));            
            
            this.ParentConversationManager = manager;
            this.ID = id;
            this.defaultRootNodeID = defaultRootNodeID;
            this.OnStartConversationID = onStartConversationID;
            this.DefaultDidEnterNodeID = defaultDidEnterNodeID;
            this.DefaultDidExitNodeID = defaultDidExitNodeID;
            this.DefaultOnSelectedID = defaultOnSelectedID;
            this.DefaultCanShowID = defaultCanShowID;

            this.AddConversationNodes(conversationNodes);
        }

        #endregion

        #region Methods      

        internal void ExecuteNode(string id)
        {
            ConversationNode node = null;
            bool execute = false;

            if (String.IsNullOrEmpty(id))
                return;

            if (this.CurrentNode == null)
                execute = true;
            else
            { 
                if (this.CurrentNode.ID != id)
                    execute = true;
            }            

            if (execute == true && this.conversationNodesDictionary.ContainsKey(id))
            {
                node = this.conversationNodesDictionary[id];
                if (node != null)
                    this.ExecuteNode(node);
            }
        }

        internal void ExecuteNode(ConversationNode node)
        {
            DialogueSystemCallbackDelegate currentDelegate = null;

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (this.CurrentNode != null)
            {
                if (!String.IsNullOrEmpty(this.CurrentNode.DidExitNodeID))
                {
                    currentDelegate = this.ParentConversationManager.GetMethodDelegate(this.CurrentNode.DidExitNodeID) as DialogueSystemCallbackDelegate;
                    currentDelegate?.Invoke(this.ParentConversationManager, this.CurrentNode.ID);
                    currentDelegate = null;
                }

                this.PreviousNode = this.CurrentNode;
            }

            this.CurrentNode = node;

            if (!String.IsNullOrEmpty(this.CurrentNode.DidEnterNodeID))
            {
                currentDelegate = this.ParentConversationManager.GetMethodDelegate(this.CurrentNode.DidEnterNodeID) as DialogueSystemCallbackDelegate;                
            }
            else
            {
                if (!String.IsNullOrEmpty(this.DefaultDidEnterNodeID))
                {
                    currentDelegate = this.ParentConversationManager.GetMethodDelegate(this.DefaultDidEnterNodeID) as DialogueSystemCallbackDelegate;
                }
            }

            if (currentDelegate != null)
                currentDelegate.Invoke(this.ParentConversationManager, node.ID);
        }

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
