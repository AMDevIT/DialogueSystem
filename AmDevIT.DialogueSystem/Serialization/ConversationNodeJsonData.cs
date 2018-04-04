using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Serialization
{
    [DataContract]
    [KnownType(typeof(ConversationChoiceJsonData))]
    internal  class ConversationNodeJsonData
    {
        #region Properties

        [DataMember(Name = "id")]
        public string ID
        {
            get;
            set;
        }

        [DataMember(Name = "characterId")]
        public string CharacterID
        {
            get;
            set;
        }

        [DataMember(Name = "textId")]
        public string TextID
        {
            get;
            set;
        }

        [DataMember(Name = "choices")]
        public ConversationChoiceJsonData[] Choices
        {
            get;
            set;
        }

        #endregion
    }
}
