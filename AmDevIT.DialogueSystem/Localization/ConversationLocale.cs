using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Localization
{
    public sealed class ConversationLocale
    {
        #region Fields

        private string locale = null;
        private KeyValuePair<string, string>[] localizedStrings = null;

        #endregion

        #region Properties

        public string Locale
        {
            get
            {
                return this.locale;
            }
            private set
            {
                this.locale = value;
            }
        }

        public KeyValuePair<string, string>[] LocalizedStrings
        {
            get
            {
                return this.localizedStrings;
            }
            private set
            {
                this.localizedStrings = value;
            }
        }

        #endregion

        #region .ctor

        public ConversationLocale(string locale, KeyValuePair<string, string>[] localizedStrings)
        {
            if (String.IsNullOrEmpty(locale))
                throw new ArgumentNullException(nameof(locale));

            if (localizedStrings == null)
                throw new ArgumentNullException(nameof(localizedStrings));

            this.Locale = locale;
            this.LocalizedStrings = localizedStrings;
        }

        #endregion
    }
}
