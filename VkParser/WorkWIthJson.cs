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

        private static List<T> addExistingData<T>(string nameFile, int i) {
            if (System.IO.File.Exists(nameFile)) {
                return read<T>(i);
            }
            return new List<T>();
        }

        public static void save<T>(List<T> input, int i,ref bool completed, ref Locker locker, ref bool processTurn)
        {
            string nameFile = "f" + i.ToString() + ".json";
            string lockString = processTurn ? i.ToString() + ".locker" : i.ToString() + ".WPlocker";
            FileStream setlock = new FileStream(lockString, FileMode.Create);

            if (!processTurn)
            {
                locker.Lock();
                input.AddRange(addExistingData<T>(nameFile, i)); // add old data if win serv didnt read them

                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<T>));

                using (FileStream f = new FileStream(nameFile, FileMode.Create))
                {
                    jsonFormatter.WriteObject(f, input);
                }

                completed = true;
                locker.Unlock();
            }

            setlock.Close();
            File.Delete(lockString);
        }

        public static void interferingFunc(int i, ref Locker locker, ref bool processTurn)
        {
            string nameFile = "f" + i.ToString() + ".json";
            string lockString = processTurn ? i.ToString() + ".locker" : i.ToString() + ".WPlocker";
            FileStream setlock = new FileStream(lockString, FileMode.Create);

            if (!processTurn)
            {
                locker.Lock();

                using (FileStream f = new FileStream(nameFile, FileMode.OpenOrCreate))
                {
                }

                locker.Unlock();
            }

            setlock.Close();
            File.Delete(lockString);
        }

        public void deleteJsonFiles()
        {
            File.Delete(@"f1.json");
            File.Delete(@"f2.json");
            File.Delete(@"f3.json");
        }

        public static List<T> read<T>(int i)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<T>));
            
            string file_path = @"C:\Users\badyi\source\repos\OS_Homework5\VkParser\VkParser\bin\Debug\" + "f" + i.ToString() + ".json";

            using (FileStream file = new FileStream(file_path, FileMode.Open))
            {
                try
                {
                    List<T> data = (List<T>)jsonFormatter.ReadObject(file);

                    return data;
                }
                catch
                {
                    // if json file is empty
                    return new List<T>();
                }
            }
        }
    }
}
