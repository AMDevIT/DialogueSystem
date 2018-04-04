using AmDevIT.Games.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Common;
using TestApplication.Dialogues.Localization;
using TestApplication.Dialogues.Serialization;

namespace TestApplication.ViewModels
{
    public sealed class ManWindowViewModel
        : BindableBase
    {
        #region Fields

        private ConversationsManager conversationsManager = null;
        private DialogueSystemJsonConverter dialogueJsonConverter = null;
        private ConversationsLocalesImporter conversationsLocalesImporter = null;
        private bool isConversationInitializable = true;

        #endregion

        #region Properties

        #endregion

        #region Commands

        private DelegateCommand initializeConversationCommand = null;
        private DelegateCommand testSerializationCommand = null;
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
                if (testSerializationCommand == null)
                    this.testSerializationCommand = new DelegateCommand(this.TestSerialization);
                return this.testSerializationCommand;
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
            

            if (this.isConversationInitializable)
            {
                this.conversationsLocalesImporter = new ConversationsLocalesImporter();
                this.conversationsLocalesImporter.RefreshStrings();
                this.dialogueJsonConverter = new DialogueSystemJsonConverter();

                this.conversationsManager = new ConversationsManager(this.dialogueJsonConverter);
                this.conversationsManager.LocalizationManager.ImportLocales(this.conversationsLocalesImporter);
                
                this.isConversationInitializable = false;
                this.InitializeConversationCommand.RaiseCanExecuteChanged();
            }
        }

        private void TestSerialization(object parameter)
        {

        }

        #endregion
    }
}
