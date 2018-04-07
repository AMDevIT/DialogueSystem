using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CultureInfo currentLocale = null;

        #endregion

        #region Properties

        public CultureInfo CurrentLocale
        {
            get
            {
                return this.currentLocale;
            }
            set
            {
                this.currentLocale = value;         // Maybe it's worth a check?
            }
        }

        public int Count
        {
            get
            {
                return this.installedLocalesDictionary.Count;
            }
        }

        #endregion

        #region .ctor

        public ConversationsLocalizationManager()
        {
            this.CurrentLocale = CultureInfo.CurrentCulture;
        }

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

        public string GetString(string id)
        {
            string locale = null;

            if (this.installedLocalesDictionary.Count > 0)
            {
                if (this.installedLocalesDictionary.ContainsKey(this.CurrentLocale.Name))
                    locale = this.CurrentLocale.Name;

                if (String.IsNullOrEmpty(locale))
                {
                    if (this.CurrentLocale.Parent != null && 
                        this.installedLocalesDictionary.ContainsKey(this.CurrentLocale.Parent.Name))
                        locale = this.CurrentLocale.Parent.Name;
                }

                if (String.IsNullOrEmpty(locale))
                {
                    try
                    {
                        locale = this.installedLocalesDictionary.Keys.First();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (!String.IsNullOrEmpty(locale))
                return this.GetString(locale, id);
            else
                return null;
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
