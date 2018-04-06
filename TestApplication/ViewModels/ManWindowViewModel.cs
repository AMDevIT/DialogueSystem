using AmDevIT.Games.DialogueSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using TestApplication.Common;
using TestApplication.Dialogues;
using TestApplication.Dialogues.Localization;
using TestApplication.Dialogues.Serialization;

namespace TestApplication.ViewModels
{
    public sealed class ManWindowViewModel
        : BindableBase
    {

        #region Consts

        private const string DefaultTestConversationDataPath = "Assets/testConversation.json";

        #endregion

        #region Fields

        private MainConversationHandler mainConversationHandler = null;
        private ConversationsManager conversationsManager = null;
        private DialogueSystemJsonConverter dialogueJsonConverter = null;
        private ConversationsLocalesImporter conversationsLocalesImporter = null;
        private bool isConversationInitializable = true;
        private bool isConversationStarted = false;
        private bool showDialogueUI = false;

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
                    this.StartTestConversationCommand.RaiseCanExecuteChanged();                
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

        #endregion

        #region Commands

        private DelegateCommand initializeConversationCommand = null;
        private DelegateCommand testSerializationCommand = null;
        private DelegateCommand startTestConversationCommand = null;
        private DelegateCommand exitApplicationCommand = null;

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

        public DelegateCommand StartTestConversationCommand
        {
            get
            {
                if (this.startTestConversationCommand == null)
                    this.startTestConversationCommand = new DelegateCommand(this.StartTestConversation, this.CanStartTestConversation);
                return this.startTestConversationCommand;
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
            Uri conversationJsonUri = new Uri("pack://application:,,,/" + DefaultTestConversationDataPath);
            StreamResourceInfo streamResourceInfo = null;
            StreamReader streamReader = null;
            KeyValuePair<string, Delegate>[] delegates = null;            
            string testJson = null;           

            

            if (this.isConversationInitializable)
            {
                this.mainConversationHandler = new MainConversationHandler();
                delegates = this.mainConversationHandler.GetMethodsDelegates();

                this.conversationsLocalesImporter = new ConversationsLocalesImporter();
                this.conversationsLocalesImporter.RefreshStrings();
                this.dialogueJsonConverter = new DialogueSystemJsonConverter();

                this.conversationsManager = new ConversationsManager(this.dialogueJsonConverter);
                this.conversationsManager.LocalizationManager.ImportLocales(this.conversationsLocalesImporter);
                this.conversationsManager.RegisterDelegates(delegates);

                streamResourceInfo = App.GetResourceStream(conversationJsonUri);
                if (streamResourceInfo != null)
                {
                    using (streamReader = new StreamReader(streamResourceInfo.Stream))
                    {
                        testJson = streamReader.ReadToEnd();
                    }
                }
                this.conversationsManager.TestParseConversation(testJson);                
                this.isConversationInitializable = false;
                this.InitializeConversationCommand.RaiseCanExecuteChanged();
            }
        }

        private void TestSerialization(object parameter)
        {

        }

        private bool CanStartTestConversation(object parameter)
        {
            return !this.isConversationStarted;
        }

        private void StartTestConversation(object parameter)
        {

        }

        #endregion
    }
}
