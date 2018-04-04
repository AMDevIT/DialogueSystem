using AmDevIT.Games.DialogueSystem.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Localization;

namespace TestApplication.Dialogues.Localization
{
    public class ConversationsLocalesImporter
        : IConversationsLocalesImporter
    {
        #region Fields

        private readonly List<ConversationLocale> conversationLocales = new List<ConversationLocale>();

        #endregion

        #region Properties

        public bool IsStringArraySet
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        public IEnumerable<ConversationLocale> GetLocales()
        {
            if (this.conversationLocales != null)
                return this.conversationLocales.ToArray();
            else
                return new ConversationLocale[] { };
        }

        /// <summary>
        /// This method will scan all resources in the executable and loads the strings in the array, generating key/value pairs from the resource class via reflection.
        /// </summary>
        /// <remarks>
        /// This way of auto scan for implemented resx files is very expensive in terms of RAM and CPU. The best way to implement localization is to already know at runtime wich 
        /// strings set are available.
        /// </remarks>
        public void RefreshStrings()
        {
            CultureInfo currentDefaultCulture = TestConversationLocalizedStrings.Culture;
            CultureInfo[] availableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            ResourceManager resourceManager = TestConversationLocalizedStrings.ResourceManager;

            foreach(CultureInfo selectedCulture in availableCultures)
            {
                ResourceSet resourceSet = resourceManager.GetResourceSet(selectedCulture, true, false);
                ConversationLocale currentConversationLocale = null;
                KeyValuePair<string, string>[] stringsArray = null;
                PropertyInfo[] properties = null;
                string cultureName = selectedCulture.Name;

                if (String.IsNullOrEmpty(selectedCulture.Name))
                    cultureName = selectedCulture.ThreeLetterISOLanguageName;
                else
                    cultureName = selectedCulture.Name;

                if (resourceSet != null)
                {
                    // This culture info is available.
                    TestConversationLocalizedStrings.Culture = selectedCulture;

                    properties = typeof(TestConversationLocalizedStrings).GetProperties(BindingFlags.Static | 
                                                                                        BindingFlags.NonPublic);
                    if (properties != null)
                    {
                        stringsArray = properties
                                       .Where(el => el.PropertyType == typeof(string))
                                       .Select(el =>
                                       {
                                           KeyValuePair<string, string> element;
                                           string key = null;
                                           string value = null;

                                           key = el.Name;
                                           value = el.GetValue(null, null) as string;
                                           element = new KeyValuePair<string, string>(key, value);
                                           return element;
                                       })
                                       .ToArray();
                    }                    
                    currentConversationLocale = new ConversationLocale(cultureName, stringsArray);
                    this.conversationLocales.Add(currentConversationLocale);
                }
            }

            TestConversationLocalizedStrings.Culture = currentDefaultCulture;
            this.IsStringArraySet = true;
        }

        #endregion        
    }
}
