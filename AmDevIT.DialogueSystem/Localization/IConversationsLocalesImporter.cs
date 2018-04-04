using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Localization
{
    public interface IConversationsLocalesImporter
    {
        #region Methods

        IEnumerable<ConversationLocale> GetLocales();

        #endregion
    }
}
