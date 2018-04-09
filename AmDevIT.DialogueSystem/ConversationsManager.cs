using AmDevIT.Games.DialogueSystem.Localization;
using AmDevIT.Games.DialogueSystem.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AmDevIT.Games.DialogueSystem
{
    public class ConversationsManager
    {        
        #region Consts

        protected const string DefaultCanShowDelegateID = "conversation_default_canShow";
        protected const string DefaultOnChoiceSelectedID = "conversation_default_onChoiceSelected";

        #endregion

        #region Events

        public event EventHandler ConversationStarted;
        public event EventHandler ConversationEnded;      

        #endregion

        #region Fields

        private readonly Dictionary<string, object> variablesDictionary = new Dictionary<string, object>();
        private readonly Dictionary<string, Delegate> delegatesDictionary = new Dictionary<string, Delegate>();
        private readonly Dictionary<string, Conversation> conversationsDictionary = new Dictionary<string, Conversation>();

        private Conversation runningConversation = null;
        private readonly ConversationsLocalizationManager conversationsLocalizationManager = new ConversationsLocalizationManager();

        #endregion

        #region Properties

        protected IJsonConverter JsonConverter
        {
            get;
            set;
        }

        public ConversationsLocalizationManager LocalizationManager
        {
            get
            {
                return this.conversationsLocalizationManager;
            }
        }

        public Conversation RunningConversation
        {
            get
            {
                return this.runningConversation;
            }
            protected set
            {
                this.runningConversation = value;
            }
        }

        #endregion

        #region .ctor

        public ConversationsManager(IJsonConverter converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            this.JsonConverter = converter;

            // Register default callback delegates.

            this.RegisterDelegate(DefaultCanShowDelegateID, new DialogueSystemCanExecuteDelegate(this.DefaultCanShow));
            this.RegisterDelegate(DefaultOnChoiceSelectedID, new DialogueSystemCallbackDelegate(this.DefaultOnChoiceSelected));
        }

        #endregion

        #region Methods

        public void ParseConversation(string json)
        {
            ConversationJsonData conversationJsonData = null;
            Conversation conversation = null;

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("Conversation json");

            conversationJsonData = this.JsonConverter.Deserialize<ConversationJsonData>(json);
            if (conversationJsonData != null)            
                conversation = conversationJsonData.AsConversation(this);

            if (conversation != null)
            {
                if (this.conversationsDictionary.ContainsKey(conversation.ID))
                    this.conversationsDictionary[conversation.ID] = conversation;
                else
                    this.conversationsDictionary.Add(conversation.ID, conversation);
            }
        }

        public bool ContainsConversation(string id)
        {            
            return this.conversationsDictionary.ContainsKey(id);
        }

        public void StartConversation(string id)
        {
            DialogueSystemCallbackDelegate callbackDelegate = null;
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Conversation ID cannot be null");

            // Here we start.
            if (this.conversationsDictionary.ContainsKey(id) == false)
                throw new ArgumentException("No conversation with ID " + id + " found in the manager.");

            this.RunningConversation = this.conversationsDictionary[id];

            this.OnConversationStarted();
                        
            if (!String.IsNullOrEmpty(this.RunningConversation.OnStartConversationID))
            {
                callbackDelegate = this.GetMethodDelegate(this.RunningConversation.OnStartConversationID) as DialogueSystemCallbackDelegate;
                callbackDelegate?.Invoke(this, null);
            }
            else
            {
                if (!String.IsNullOrEmpty(this.RunningConversation.DefaultRootNodeID))
                    this.ExecuteNode(this.RunningConversation.DefaultRootNodeID);
            }
        }

        public void EndCurrentConversation()
        {
            if (this.RunningConversation != null)
            {
                this.RunningConversation = null;
                this.OnConversationEnded();
            }
        }

        public void ExecuteNode(string id)
        {
            if (this.RunningConversation != null)
                this.RunningConversation.ExecuteNode(id);            
        }

        #region Delegates management

        public void RegisterDelegate(string id, DialogueSystemCallbackDelegate callback)
        {
            this.RegisterDelegateInternal(id, callback);
        }

        public void RegisterDelegate(string id, DialogueSystemCanExecuteDelegate canExecuteCallback)
        {
            this.RegisterDelegateInternal(id, canExecuteCallback);
        }

        public void RegisterDelegates(KeyValuePair<string, Delegate>[] delegates)
        {
            if (delegates == null)
                throw new ArgumentNullException(nameof(delegates));

            foreach(KeyValuePair<string, Delegate> currentDelegate in delegates)            
                this.RegisterDelegateInternal(currentDelegate.Key, currentDelegate.Value);            
        }

        #endregion

        #region Variables management

        public string GetStringValue(string key)
        {
            string result = this.GetValue(key) as String;
            return result;
        }

        public long GetNumberValue(string key)
        {
            long result = 0;

            try
            {
                result = (long)this.GetValue(key);
            }
            catch(InvalidCastException)
            {
                result = 0;
            }

            return result;
        }

        public object GetValue(string key)
        {
            object value = null;

            if (String.IsNullOrEmpty(key))
                return null;

            if (this.variablesDictionary.ContainsKey(key))
                value = this.variablesDictionary[key];

            return value;
        }

        public void SetValue(string key, object value)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(value));

            if (this.variablesDictionary.ContainsKey(key))
                this.variablesDictionary[key] = value;
            else
                this.variablesDictionary.Add(key, value);
        }

        #endregion

        #region LocalizationManagement

        public string GetLocalizedString(string key)
        {
            string value = key;

            if (this.conversationsLocalizationManager.Count > 0)
                value = this.conversationsLocalizationManager.GetString(key);
            return value;
        }

        #endregion

        #region Conversation

        #endregion

        #region Internal methods

        internal Delegate GetMethodDelegate(string id)
        {
            Delegate method = null;
            if (this.delegatesDictionary.Count > 0 && this.delegatesDictionary.ContainsKey(id))
                method = this.delegatesDictionary[id];
            return method;
        }
         
        protected void ExecuteConversationNode(string nodeID)
        {

        }
        
        protected void RegisterDelegateInternal(string id, Delegate value)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (value == null)
                throw new ArgumentNullException("delegate method");

            if ((value is Delegate) == false)
                throw new ArgumentException("value is not a delegate");

            if (this.delegatesDictionary.ContainsKey(id))
                this.delegatesDictionary[id] = value;
            else
                this.delegatesDictionary.Add(id, value);
        }

        protected void UnregisterDelegate(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
        }     

        protected virtual bool DefaultCanShow(ConversationsManager manager, object state)
        {
            DialogueSystemCanExecuteDelegate canExecuteDelegate = null;
            bool result = true;

            if (manager.RunningConversation != null && !String.IsNullOrEmpty(manager.RunningConversation.DefaultCanShowID))
            {
                canExecuteDelegate = this.GetMethodDelegate(manager.RunningConversation.DefaultCanShowID) as DialogueSystemCanExecuteDelegate;
                if (canExecuteDelegate != null)
                    result = canExecuteDelegate.Invoke(manager, state);
            }
            return result;
        }

        protected virtual void DefaultOnChoiceSelected(ConversationsManager manager, object state)
        {
            DialogueSystemCallbackDelegate callbackDelegate = null;
            bool handled = false;
            string choiceID = state as String;

            if (manager.RunningConversation != null && !String.IsNullOrEmpty(manager.runningConversation.DefaultOnSelectedID))
            {
                callbackDelegate = this.GetMethodDelegate(manager.RunningConversation.DefaultOnSelectedID) as DialogueSystemCallbackDelegate;
                if (callbackDelegate != null)
                {
                    handled = true;
                    callbackDelegate.Invoke(manager, state);
                }
            }

            if (!handled && manager.RunningConversation != null && manager.RunningConversation.CurrentNode != null)
            {

#warning Add the choice code.

            }
        }

        protected void OnConversationStarted()
        {
            // Event args must be added.
            this.ConversationStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void OnConversationEnded()
        {
            // Event args must be added.
            this.ConversationEnded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Test Methods

#if DEBUG

        // This section will be removed later in favor of unit tests.
        // Now the code simple doesn't have enought stability to create a suite of tests.

        public void TestParseConversation(string json)
        {
            ConversationJsonData conversationJsonData = null;
            Conversation conversation = null;

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("Conversation json");

            conversationJsonData = this.JsonConverter.Deserialize<ConversationJsonData>(json);
            if (conversationJsonData != null)
                conversation = conversationJsonData.AsConversation(this);

            Debugger.Break();
        }

#endif 

        #endregion

        #endregion
    }
}
