using AmDevIT.Games.DialogueSystem.Serialization;
using Newtonsoft.Json;
using System;

namespace TestApplication.Dialogues.Serialization
{
    public class DialogueSystemJsonConverter
        : IJsonConverter
    {
        #region Methods

        public string Serialize(Object value)
        {
            String result = null;

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            result = JsonConvert.SerializeObject(value);
            return result;
        }

        public T Deserialize<T>(string json)
        {
            T result = default(T);

            if (String.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json));            

            result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        #endregion
    }
}
