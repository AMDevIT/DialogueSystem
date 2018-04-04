using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Localization
{
    /// <summary>
    /// Contains the localized strings for the conversations defined in Conversations Manager.
    /// </summary>
    public class ConversationsLocalizationManager
    {
        #region Fields

        private readonly Dictionary<string, Dictionary<string, string>> installedLocalesDictionary = new Dictionary<string, Dictionary<string, string>>();

        #endregion

        #region .ctor

        #endregion

        #region Methods

        public void AddLocale(string locale, IEnumerable<KeyValuePair<string , string>> localizedStringsWithTags)
        {
            Dictionary<string, string> currentLocale = null;

            if (string.IsNullOrEmpty(locale))
                throw new ArgumentNullException(nameof(locale));

            if (localizedStringsWithTags == null)
                throw new ArgumentNullException(nameof(localizedStringsWithTags));

            if (this.installedLocalesDictionary.ContainsKey(locale))
            {
                currentLocale = this.installedLocalesDictionary[locale];
                if (currentLocale != null)
                    currentLocale.Clear();
                else
                    this.installedLocalesDictionary.Remove(locale);
            }

            if (currentLocale == null)            
                currentLocale = new Dictionary<string, string>();                
            
            foreach (KeyValuePair<string, string> currentLocalizedString in localizedStringsWithTags)
                currentLocale.Add(currentLocalizedString.Key, currentLocalizedString.Value);

            this.installedLocalesDictionary.Add(locale, currentLocale);
        }

        public void RemoveLocale(string locale)
        {
            if (this.installedLocalesDictionary.ContainsKey(locale))
                this.installedLocalesDictionary.Remove(locale);
        }

        public void ImportLocales(IConversationsLocalesImporter importer)
        {
            IEnumerable<ConversationLocale> importedLocales = null;

            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            importedLocales = importer.GetLocales();
            if (importedLocales != null)
            {
                foreach(ConversationLocale currentLocale in importedLocales)                
                    this.AddLocale(currentLocale.Locale, currentLocale.LocalizedStrings);                
            }
        }

        public string GetString(string locale, string id)
        {
            string result = null;

            if (this.installedLocalesDictionary.ContainsKey(locale) && this.installedLocalesDictionary[locale].ContainsKey(id))
                result = this.installedLocalesDictionary[locale][id];
            return result;
        }

        #endregion
    }
}
