using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace VkParser
{
    class WorkWithJson
    {


        public WorkWithJson()
        {
        }

        public static void save<T>(List<T> input, int i,ref bool completed)
        {
            string nameFile = "f" + i.ToString() + ".json";

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<T>));

            FileStream setlock = new FileStream(i.ToString() + ".locker", FileMode.Create);
            using (FileStream f = new FileStream(nameFile, FileMode.Create))
            {
                jsonFormatter.WriteObject(f, input);
            }

            setlock.Close();
            completed = true;
            File.Delete(i.ToString() + ".locker");
        }

        public void deleteJsonFiles()
        {
            File.Delete(@"f1.json");
            File.Delete(@"f2.json");
            File.Delete(@"f3.json");
        }

        public T read<T>(int i, object locker)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(T));

            string file_name = "f" + i.ToString() + ".json";

            using (FileStream file = new FileStream(file_name, FileMode.OpenOrCreate))
            {
                T dict = (T)jsonFormatter.ReadObject(file);
                return dict;
            }
        }
    }
}
