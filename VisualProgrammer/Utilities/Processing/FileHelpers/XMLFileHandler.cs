using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualProgrammer.Utilities.Processing.FileHelpers
{
    public class XMLFileHandler : IFileHandler
    {
        public void Save<T>(string filepath, T data)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (Stream s = File.Create(filepath))
                xs.Serialize(s, data);
        }

        public T Load<T>(string filepath)
        {
            T data;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using(Stream s = File.OpenRead(filepath))
            {
                data = (T)xs.Deserialize(s);
            }

            return data;
        }
    }
}
