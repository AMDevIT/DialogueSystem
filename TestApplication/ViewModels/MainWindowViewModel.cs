using AmDevIT.Games.DialogueSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using TestApplication.Common;
using TestApplication.Dialogues;
using TestApplication.Dialogues.Localization;
using TestApplication.Dialogues.Serialization;
using TestApplication.Models;

namespace TestApplication.ViewModels
{
    public sealed class MainWindowViewModel
        : BindableBase
    {

        #region Consts

        private const string DefaultTestConversationDataPath = "Assets/forestConversation.json";

        #endregion

        #region Fields

        private readonly Dictionary<string, Character> charactersDictionary = new Dictionary<string, Character>();
        private MainConversationHandler mainConversationHandler = null;
        private ConversationsManager conversationsManager = null;
        private DialogueSystemJsonConverter dialogueJsonConverter = null;
        private ConversationsLocalesImporter conversationsLocalesImporter = null;
        private bool isConversationInitializable = true;
        private bool isConversationStarted = false;
        private bool showDialogueUI = false;

        private string characterName = null;
        private string currentDialogueText = null;
        private ImageSource currentCharacterPortrait = null;
        private DialogueChoice[] dialogueChoices = null;

        #endregion

        #region Properties

        public bool IsConversationStarted
        {
            get
            {
                return this.isConversationStarted;
            }
            private set
            {
                if (this.SetProperty(ref this.isConversationStarted, value))                
                    this.StartConversationCommand.RaiseCanExecuteChanged();                
            }
        }

        public bool ShowDialogueUI
        {
            get
            {
                return this.showDialogueUI;
            }
            private set
            {
                this.SetProperty(ref this.showDialogueUI, value);
            }
        }

        public string CharacterName
        {
            get
            {
                return this.characterName;
            }
            private set
            {
                this.SetProperty(ref this.characterName, value);
            }
        }

        public string CurrentDialogueText
        {
            get
            {
                return this.currentDialogueText;
            }
            private set
            {
                this.SetProperty(ref this.currentDialogueText, value);
            }
        }

        public ImageSource CurrentCharacterPortrait
        {
            get
            {
                return this.currentCharacterPortrait;
            }
            private set
            {
                this.SetProperty(ref this.currentCharacterPortrait, value);
            }
        }

        public DialogueChoice[] DialogueChoices
        {
            get
            {
                return this.dialogueChoices;
            }
            private set
            {
                this.SetProperty(ref this.dialogueChoices, value);
            }
        }

        #endregion

        #region Commands

        private DelegateCommand initializeConversationCommand = null;
        private DelegateCommand testSerializationCommand = null;
        private DelegateCommand startConversationCommand = null;
        private DelegateCommand exitApplicationCommand = null;
        private DelegateCommand choiceListViewItemSelectedCommand = null;

        public DelegateCommand ExitApplicationCommand
        {
            get
            {
                if (this.exitApplicationCommand == null)
                    this.exitApplicationCommand = new DelegateCommand(this.ExitApplication);
                return this.exitApplicationCommand;
            }
        }

        public DelegateCommand InitializeConversationCommand
        {
            get
            {
                if (this.initializeConversationCommand == null)
                    this.initializeConversationCommand = new DelegateCommand(this.InitializeConversation, this.CanInitializeConversation);
                return this.initializeConversationCommand;
            }
        }

        public DelegateCommand TestSerializationCommand
        {
            get
            {
                if (this.testSerializationCommand == null)
                    this.testSerializationCommand = new DelegateCommand(this.TestSerialization);
                return this.testSerializationCommand;
            }
        }

        public DelegateCommand StartConversationCommand
        {
            get
            {
                if (this.startConversationCommand == null)
                    this.startConversationCommand = new DelegateCommand(this.StartConversation, this.CanStartTestConversation);
                return this.startConversationCommand;
            }            
        }

        public DelegateCommand ChoiceListViewItemSelectedCommand
        {
            get
            {
                if (this.choiceListViewItemSelectedCommand == null)
                    this.choiceListViewItemSelectedCommand = new DelegateCommand(this.ChoiceListViewItemSelected);
                return this.choiceListViewItemSelectedCommand;
            }
        }

        #endregion

        #region Methods

        private void ExitApplication(object parameter)
        {
            App.Current.Shutdown(0);
        }

        private bool CanInitializeConversation(object parameter)
        {
            return this.isConversationInitializable;
        }

        private void InitializeConversation(object parameter)
        {
            Character currentCharacter = null;
            Uri conversationJsonUri = new Uri("pack://application:,,,/" + DefaultTestConversationDataPath);
            StreamResourceInfo streamResourceInfo = null;
            StreamReader streamReader = null;
            KeyValuePair<string, Delegate>[] delegates = null;            
            string conversationJSon = null;                       

            if (this.isConversationInitializable)
            {
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

                this.mainConversationHandler = new MainConversationHandler();
                this.mainConversationHandler.StartConversationEnded += MainConversationHandler_StartConversationEnded;
                this.mainConversationHandler.DidEnterNode += MainConversationHandler_DidEnterNode;
                this.mainConversationHandler.DidExitNode += MainConversationHandler_DidExitNode;
                delegates = this.mainConversationHandler.GetMethodsDelegates();

                this.conversationsLocalesImporter = new ConversationsLocalesImporter();
                this.conversationsLocalesImporter.RefreshStrings();
                this.dialogueJsonConverter = new DialogueSystemJsonConverter();

                this.conversationsManager = new ConversationsManager(this.dialogueJsonConverter);
                this.conversationsManager.ConversationStarted += ConversationsManager_ConversationStarted;
                this.conversationsManager.ConversationEnded += ConversationsManager_ConversationEnded;
                this.conversationsManager.LocalizationManager.ImportLocales(this.conversationsLocalesImporter);
                this.conversationsManager.RegisterDelegates(delegates);

                streamResourceInfo = App.GetResourceStream(conversationJsonUri);
                if (streamResourceInfo != null)
                {
                    using (streamReader = new StreamReader(streamResourceInfo.Stream))
                    {
                        conversationJSon = streamReader.ReadToEnd();
                    }
                }
                this.conversationsManager.ParseConversation(conversationJSon);
                this.isConversationInitializable = false;
                this.InitializeConversationCommand.RaiseCanExecuteChanged();

                MessageBox.Show("Conversation data initialized. Ready to start.");
            }
        }

        private void TestSerialization(object parameter)
        {

        }

        private bool CanStartTestConversation(object parameter)
        {
            return !this.isConversationStarted;
        }

        private void StartConversation(object parameter)
        {
            string conversationName = parameter as string;

            if (String.IsNullOrEmpty(conversationName))
            {
                // Show error.
            }

            if (this.conversationsManager.ContainsConversation(conversationName))
                this.conversationsManager.StartConversation(conversationName);
            else
            {
                // Show error.
            }
        }

        private void UpdateCharacterPortrait(string characterID)
        {
            string uriBase = "pack://application:,,,/";
            string imageUriString = null;
            Uri imageUri = null;
            BitmapImage bitmapImage = null;

            switch (characterID)
            {
                case "main_character":
                    imageUriString = uriBase + "Assets/Images/FlareMaleHero3.png";
                    break;

                case "companion1":
                    imageUriString = uriBase + "Assets/Images/FlareFemaleHero1.png";
                    break;

                case "companion2":
                    imageUriString = uriBase + "Assets/Images/FlareMaleHero2.png";
                    break;

                default:
                    break;
            }

            if (!String.IsNullOrEmpty(imageUriString))
            {
                imageUri = new Uri(imageUriString);
                bitmapImage = new BitmapImage(imageUri);
                this.CurrentCharacterPortrait = bitmapImage;
            }
        }

        private void ChoiceListViewItemSelected(object parameter)
        {
            DialogueChoice dialogueChoice = parameter as DialogueChoice;

            if (parameter != null && this.conversationsManager.RunningConversation != null)
                this.conversationsManager.RunningConversation.CurrentNode.ChoiceSelected(dialogueChoice.ID);
        }

        #endregion

        #region Event Handlers

        private void ConversationsManager_ConversationStarted(object sender, EventArgs e)
        {
            this.IsConversationStarted = true;
        }

        private void ConversationsManager_ConversationEnded(object sender, EventArgs e)
        {
            this.IsConversationStarted = false;
        }

        private void MainConversationHandler_StartConversationEnded(object sender, EventArgs e)
        {
            this.IsConversationStarted = true;
        }

        private void MainConversationHandler_DidEnterNode(object sender, EventArgs e)
        {
            List<DialogueChoice> dialogueChoicesList = null;
            Character currentCharacter = null;
            string text = null;
            string characterID = null;
            string characterName = null;

            if (this.conversationsManager.RunningConversation.CurrentNode != null)
            {
                text = this.conversationsManager.RunningConversation.CurrentNode.Text;
                characterID = this.conversationsManager.RunningConversation.CurrentNode.CharacterID;
                if (this.charactersDictionary.ContainsKey(characterID))
                    currentCharacter = this.charactersDictionary[characterID];
                if (currentCharacter != null)
                    characterName = currentCharacter.FullName;

                this.CharacterName = characterName;
                this.CurrentDialogueText = text;
                this.UpdateCharacterPortrait(currentCharacter.ID);

                dialogueChoicesList = new List<DialogueChoice>();
                
                foreach(ConversationChoice conversationChoice in this.conversationsManager.RunningConversation.CurrentNode.Choices)
                {
                    DialogueChoice currentChoice = new DialogueChoice();

                    currentChoice.ID = conversationChoice.ID;
                    currentChoice.Text = conversationChoice.Text;

                    dialogueChoicesList.Add(currentChoice);
                }

                this.DialogueChoices = dialogueChoicesList.ToArray();
            }
        }


        private void MainConversationHandler_DidExitNode(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
