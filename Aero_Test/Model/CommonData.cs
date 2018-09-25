using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero_Test.Model
{
    static class CommonData
    {
        /// <summary>
        /// Словарь, в котором каждой модели самолета соответствует количество мест
        /// </summary>
        public static Dictionary<string, int> PlanesCapacity = new Dictionary<string, int>();
        //{
        //    {"Cessna-431", 20 },
        //    {"Boeing-737", 438 },
        //    {"Airbus-A320", 320 }
        //};
    }
}
