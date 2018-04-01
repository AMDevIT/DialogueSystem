using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmDevIT.Games.DialogueSystem.Serialization
{
    public interface IJsonConverter
    {
        #region Methods

        string Serialize(Object value);
        T Deserialize<T>(string json);

        #endregion
    }
}
