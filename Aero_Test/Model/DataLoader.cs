using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Aero_Test.Model
{
    /// <summary>
    /// Загрузка данных из текстового файла
    /// </summary>
    static class DataLoader
    {
        public static List<FlightRecord> LoadFlightsList(ref Queue<FlightRecord> queue, string filePath)
        {
            if (File.Exists(filePath))
            {
                List<FlightRecord> temp = new List<FlightRecord>();
                using (StreamReader SR = new StreamReader(filePath, Encoding.GetEncoding(1251)))
                {                                     
                    string line = "";
                    FlightRecord tempRecord;
                    while ((line = SR.ReadLine()) != null)
                    {
                        tempRecord = ParseRecord(line);
                        temp.Add(tempRecord);                        
                    }                    
                }
                temp.Sort();
                foreach (FlightRecord FR in temp)
                {
                    queue.Enqueue(FR);
                }
                return temp;
            }
            else throw new FileNotFoundException("Файл с расписанием не найден");
        }

        private static FlightRecord ParseRecord(string record)
        {
            try
            {
                Random R = new Random();
                string[] temp = record.Split(';');
                string tmpPlane = temp[0];
                string tmpFlightType = temp[1];
                
                FlightRecord res = new FlightRecord(
                    temp[0],
                    new DateTime(1, 1, 1, int.Parse(temp[2].Split(':')[0]), int.Parse(temp[2].Split(':')[1]), 0),
                    temp[1],
                    temp[3],
                    R.Next(0, CommonData.PlanesCapacity[temp[0]]),
                    CommonData.PlanesCapacity[temp[0]]);
                return res;

            } catch (Exception E)
            {
                throw E;
            }
        }

        public static bool LoadPlanes(ref Dictionary<string, int> PlanesDictionary, string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader SR = new StreamReader(filePath, Encoding.GetEncoding(1251)))
                    {
                        string line = "";
                        while ((line = SR.ReadLine()) != null)
                        {
                            if (!PlanesDictionary.ContainsKey(line.Split(';')[0]))
                            {
                                PlanesDictionary.Add(line.Split(';')[0], int.Parse(line.Split(';')[1]));
                            }
                        }
                        return true;
                    }
                } catch (Exception E)
                {
                    throw new PlanesLoadException("Ошибка при загрузке типов самолетов", E);
                }

            }
            else return false;
        }
    }

    class PlanesLoadException : Exception
    {
        public PlanesLoadException()
        {

        }

        public PlanesLoadException(string message) : base(message)
        {

        }

        public PlanesLoadException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
