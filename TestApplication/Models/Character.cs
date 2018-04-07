using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Models
{
    public class Character
    {
        #region Properties

        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Surname
        {
            get;
            set;
        }

        public string FullName
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(this.Name);
                if (String.IsNullOrEmpty(this.Surname))
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(this.Surname);
                }

                return stringBuilder.ToString();
            }
        }

        public Genders Gender
        {
            get;
            set;
        }

        #endregion
    }
}
