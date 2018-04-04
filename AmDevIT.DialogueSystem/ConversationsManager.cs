using AmDevIT.Games.DialogueSystem.Localization;
using AmDevIT.Games.DialogueSystem.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem
{
    public class ConversationsManager
    {
        #region Consts

        protected const string DefaultCanShowDelegateID = "conversation_default_canShow";
        protected const string DefaultOnChoiceSelectedID = "conversation_default_onChoiceSelected";

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
            this.RegisterDelegate(DefaultOnChoiceSelectedID, new DialogueSystemCallback(this.DefaultOnChoiceSelected));
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
                conversation = conversationJsonData.AsConversation();

            if (conversation != null)
            {
                if (this.conversationsDictionary.ContainsKey(conversation.ID))
                    this.conversationsDictionary[conversation.ID] = conversation;
                else
                    this.conversationsDictionary.Add(conversation.ID, conversation);
            }
        }

        public void RegisterDelegate(string id, DialogueSystemCallback callback)
        {
            this.RegisterDelegateInternal(id, callback);
        }

        public void RegisterDelegate(string id, DialogueSystemCanExecuteDelegate canExecuteCallback)
        {
            this.RegisterDelegateInternal(id, canExecuteCallback);
        }

        public string GetString(string key)
        {
            string result = this.GetValue(key) as String;
            return result;
        }

        public long GetNumber(string key)
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

        protected virtual bool DefaultCanShow(object status)
        {
            return true;
        }

        protected virtual void DefaultOnChoiceSelected(object status)
        {

        }

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
                conversation = conversationJsonData.AsConversation();

            Debugger.Break();
        }

#endif 

        #endregion

        #endregion
    }
}
