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
        #region Fields
        
        private bool isViewModelInitialized = false;
        private bool isConversationStarted = false;
        private bool showDialogueUI = false;

        private string characterName = null;
        private string currentDialogueText = null;
        private ImageSource currentCharacterPortrait = null;
        private DialogueChoice[] dialogueChoices = null;

        #endregion

        #region Properties

        public bool Initialized
        {
            get
            {
                return this.isViewModelInitialized;
            }
            private set
            {
                if (this.SetProperty(ref this.isViewModelInitialized, value))
                    this.InitializeViewModelCommand.RaiseCanExecuteChanged();
            }
        }

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

        private DelegateCommand initializeViewModelCommand = null;
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

        public DelegateCommand InitializeViewModelCommand
        {
            get
            {
                if (this.initializeViewModelCommand == null)
                    this.initializeViewModelCommand = new DelegateCommand(this.InitializeViewModel, this.CanInitializeViewModel);
                return this.initializeViewModelCommand;
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

        #region .ctor

        public MainWindowViewModel()
        {
            GameManager.Current.RefreshUI += Current_RefreshUI;

        }        

        #endregion

        #region Methods

        private void ExitApplication(object parameter)
        {
            App.Current.Shutdown(0);
        }

        private bool CanInitializeViewModel(object parameter)
        {
            return !this.isViewModelInitialized;
        }

        private void InitializeViewModel(object parameter)
        { 
            if (this.isViewModelInitialized == false)
            {
                if (GameManager.Current.Initialized == false)
                    GameManager.Current.Initialize();

                GameManager.Current.ConversationStarted += GameManager_ConversationStarted;
                GameManager.Current.ConversationEnded += GameManager_ConversationEnded;
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
            string conversationID = parameter as string;

            if (String.IsNullOrEmpty(conversationID))
                throw new ArgumentNullException(nameof(conversationID));
            
            GameManager.Current.StartConversation(conversationID);
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

            if (dialogueChoice != null)
                GameManager.Current.ChoiceSelected(dialogueChoice);            
        }

        #endregion

        #region Event Handlers

        private void GameManager_ConversationStarted(object sender, EventArgs e)
        {
            this.IsConversationStarted = true;
        }

        private void GameManager_ConversationEnded(object sender, EventArgs e)
        {
            this.IsConversationStarted = false;
        }

        private void Current_RefreshUI(object sender, EventArgs e)
        {
            List<DialogueChoice> dialogueChoicesList = null;
            Conversation runninConversation = GameManager.Current.ConversationsManager.RunningConversation;
            ConversationNode currentNode = null;
            Character currentCharacter = null;
            string text = null;
            string characterID = null;
            string characterName = null;

            if (runninConversation != null && runninConversation.CurrentNode != null)
            {
                currentNode = GameManager.Current.ConversationsManager.RunningConversation.CurrentNode;
                text = currentNode.Text;
                characterID = currentNode.CharacterID;
                if (GameManager.Current.Characters.ContainsKey(characterID))
                    currentCharacter = GameManager.Current.Characters[characterID];
                if (currentCharacter != null)
                    characterName = currentCharacter.FullName;

                this.CharacterName = characterName;
                this.CurrentDialogueText = text;
                this.UpdateCharacterPortrait(currentCharacter.ID);

                dialogueChoicesList = new List<DialogueChoice>();

                foreach (ConversationChoice conversationChoice in currentNode.Choices)
                {
                    DialogueChoice currentChoice = new DialogueChoice();

                    currentChoice.ID = conversationChoice.ID;
                    currentChoice.Text = conversationChoice.Text;

                    dialogueChoicesList.Add(currentChoice);
                }

                this.DialogueChoices = dialogueChoicesList.ToArray();
            }
        }

        #endregion
    }
}
