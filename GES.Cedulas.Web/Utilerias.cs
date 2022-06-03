using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario
{
    public class Utilerias
    {
        public static T Deserialize<T>(string input) where T : class
        {
            //input = input.Replace("G136070", "136070");
            //input = input.Replace("A13607I", "13607");
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}
