using AmDevIT.Games.DialogueSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Dialogues;
using TestApplication.Dialogues.Localization;
using TestApplication.Dialogues.Serialization;

namespace TestApplication.Models
{
    public sealed class GameManager
    {
        #region Events

        public event EventHandler RefreshUI;
        public event EventHandler ConversationStarted;
        public event EventHandler ConversationEnded;

        #endregion 

        #region Singleton

        private readonly static GameManager currentGameManagerInstance = new GameManager();
        public static GameManager Current => currentGameManagerInstance;

        #endregion

        #region Fields

        private readonly Dictionary<string, Character> charactersDictionary = new Dictionary<string, Character>();
        private readonly List<ConversationHandlerBase> conversationHandlersList = new List<ConversationHandlerBase>();
        private ConversationsManager conversationsManager = null;
        private DialogueSystemJsonConverter dialogueJsonConverter = null;
        private ConversationsLocalesImporter conversationsLocalesImporter = null;

        #endregion

        #region Properties

        public bool Initialized
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<string, Character> Characters
        {
            get
            {
                return this.charactersDictionary;
            }
        }
        
        public ConversationsManager ConversationsManager
        {
            get
            {
                return this.conversationsManager;
            }
        }

        #endregion

        #region .ctor

        public GameManager()
        {            
        }
        
        #endregion

        #region Methods

        public void Initialize()
        {
            string[] dialoguesJson = null;

            if (this.Initialized == false)
            {
                this.conversationsLocalesImporter = new ConversationsLocalesImporter();
                this.dialogueJsonConverter = new DialogueSystemJsonConverter();

                this.conversationsManager = new ConversationsManager(this.dialogueJsonConverter);
                this.conversationsManager.ConversationStarted += ConversationsManager_ConversationStarted;
                this.conversationsManager.ConversationEnded += ConversationsManager_ConversationEnded;

                // Import localizations

                this.conversationsLocalesImporter.RefreshStrings();
                this.conversationsManager.LocalizationManager.ImportLocales(this.conversationsLocalesImporter);

                // Import convesations

                dialoguesJson = this.GetDialoguesJsonFromAssets();

                foreach(string currentDialogueJson in dialoguesJson)
                    this.conversationsManager.ParseConversation(currentDialogueJson);

                // Init characters

                this.InitializeAdventureCharacters();

                // Init delegates

                this.InitializeConversationHandlers();
                
                this.Initialized = true;
            }
        }

        public void StartConversation(string id)
        {
            if (this.conversationsManager.ContainsConversation(id))
                this.conversationsManager.StartConversation(id);
            else
                throw new ArgumentException("Conversation ID is not valid");
        }

        public void ChoiceSelected(DialogueChoice dialogueChoice)
        {
            if (this.ConversationsManager.RunningConversation != null &&
                this.ConversationsManager.RunningConversation.CurrentNode != null)
                this.ConversationsManager.RunningConversation.CurrentNode.ChoiceSelected(dialogueChoice.ID);
        }

        private string[] GetDialoguesJsonFromAssets()
        {
            String conversationsPath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Conversations";
            string[] filesPaths = null;
            List<string> jsons = new List<string>();

            filesPaths = Directory.GetFiles(conversationsPath, "*.json");
            if (filesPaths != null && filesPaths.Length > 0)
            {
                foreach (string currentFilePath in filesPaths)
                {
                    string currentJson = null;
                    try
                    {
                         currentJson = File.ReadAllText(currentFilePath);
                    }
                    finally
                    {

                    }

                    if (!String.IsNullOrEmpty(currentJson))
                        jsons.Add(currentJson);
                }
            }

            return jsons.ToArray();
        }

        private void InitializeAdventureCharacters()
        {
            Character currentCharacter = null;

            if (this.charactersDictionary.Count > 0)
                this.charactersDictionary.Clear();

            currentCharacter = new Character();
            currentCharacter.ID = "main_character";
            currentCharacter.Name = "Marcus";
            currentCharacter.Surname = "Lionhearth";
            currentCharacter.Gender = Genders.Male;
            this.charactersDictionary.Add(currentCharacter.ID, currentCharacter);

            currentCharacter = new Character();
            currentCharacter.ID = "companion1";
            currentCharacter.Name = "Lorraine";
            currentCharacter.Surname = "Rose";
            currentCharacter.Gender = Genders.Female;
            this.charactersDictionary.Add(currentCharacter.ID, currentCharacter);

            currentCharacter = new Character();
            currentCharacter.ID = "companion2";
            currentCharacter.Name = "Slash";
            currentCharacter.Gender = Genders.Male;
            this.charactersDictionary.Add(currentCharacter.ID, currentCharacter);
        }

        private void InitializeConversationHandlers()
        {
            ForestConversationHandler mainConversationHandler = new ForestConversationHandler();
            KeyValuePair<string, Delegate>[] delegatesArray = null;

            mainConversationHandler.DidEnterNode += MainConversationHandler_DidEnterNode;
            mainConversationHandler.DidExitNode += MainConversationHandler_DidExitNode;
            mainConversationHandler.StartConversationEnded += MainConversationHandler_StartConversationEnded;
            delegatesArray = mainConversationHandler.GetMethodsDelegates();

            if (this.ConversationsManager != null)
                this.ConversationsManager.RegisterDelegates(delegatesArray);

            this.conversationHandlersList.Add(mainConversationHandler);

        }   
        
        private void OnRefreshUI()
        {
            this.RefreshUI?.Invoke(this, EventArgs.Empty);
        }

        private void OnConversationStarted()
        {
            this.ConversationStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnConversationEnded()
        {
            this.ConversationEnded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Event Handlers

        private void ConversationsManager_ConversationStarted(object sender, EventArgs e)
        {
            this.OnConversationStarted();
        }

        private void ConversationsManager_ConversationEnded(object sender, EventArgs e)
        {
            this.OnConversationEnded();
        }

        private void MainConversationHandler_StartConversationEnded(object sender, EventArgs e)
        {
            this.OnRefreshUI();
        }

        private void MainConversationHandler_DidEnterNode(object sender, EventArgs e)
        {
            this.OnRefreshUI();
        }

        private void MainConversationHandler_DidExitNode(object sender, EventArgs e)
        {
            this.OnRefreshUI();
        }

        #endregion
    }
}
