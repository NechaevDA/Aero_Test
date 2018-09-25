using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero_Test.Model
{
    struct FlightRecord : IComparable
    {
        public string PlaneType { get; set; }
        public DateTime InteractTime { get; set; }
        public string FlightType { get; set; }
        public string CityName { get; set; }
        public int CurrentPassengers { get; set; }
        public int TotalPassengers { get; set; }

        /// <summary>
        /// Запись о вылете/прилете
        /// </summary>
        /// <param name="planeType">Тип самолета</param>
        /// <param name="interactTime">Время вылета/прилета</param>
        /// <param name="flightType">Вылет/прилет</param>
        /// <param name="cityName">Город вылета/прилета</param>
        /// <param name="currentPassengers">Количество пассажиров</param>
        /// <param name="totalPassengers">Вместительность самолета</param>
        public FlightRecord(string planeType, DateTime interactTime, string flightType, string cityName, int currentPassengers, int totalPassengers)
        {
            PlaneType = planeType;
            InteractTime = interactTime;
            FlightType = flightType;
            CityName = cityName;
            CurrentPassengers = currentPassengers;
            TotalPassengers = totalPassengers;
        }

        //Для создания копии записи
        public FlightRecord(FlightRecord FR)
        {
            this.CityName = FR.CityName;
            this.CurrentPassengers = FR.CurrentPassengers;
            this.FlightType = FR.FlightType;
            this.InteractTime = FR.InteractTime;
            this.PlaneType = FR.PlaneType;
            this.TotalPassengers = FR.TotalPassengers;
        }

        //public FlightRecord() { }

        /// <summary>
        /// Компаратор, для изначальной сортировки, при загрузке. Выстраивает записи по времени
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (((FlightRecord)obj).InteractTime == this.InteractTime)
            {
                return 0;
            } else if (((FlightRecord)obj).InteractTime >= this.InteractTime)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
