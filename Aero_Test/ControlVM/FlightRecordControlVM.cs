using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Aero_Test.Model;

namespace Aero_Test.ControlVM
{
    class FlightRecordControlVM : BindableBase
    {
        public FlightRecordControlVM() { }
        public FlightRecordControlVM(FlightRecord record)
        {
            FlightType = record.FlightType;
            CityName = record.CityName;
            InteractTime = record.InteractTime.ToString("HH:mm");
            PlaneType = record.PlaneType;
            Fullness = $"{record.CurrentPassengers}/{record.TotalPassengers}";
            StartAnimation = true;
        }

        private string _planeType;
        private string _flightType;
        private string _interactTime;
        private string _cityName;
        private string _fullness;

        /// <summary>
        /// Тип самолета
        /// </summary>
        public string PlaneType
        {
            get { return _planeType; }
            set { SetProperty(ref _planeType, value); }
        }

        /// <summary>
        /// Тип полета (вылет/прилет)
        /// </summary>
        public string FlightType
        {
            get { return _flightType; }
            set { SetProperty(ref _flightType, value); }
        }

        /// <summary>
        /// Время полета
        /// </summary>
        public string InteractTime
        {
            get { return _interactTime; }
            set { SetProperty(ref _interactTime, value); }
        }

        /// <summary>
        /// Город
        /// </summary>
        public string CityName
        {
            get { return _cityName; }
            set { SetProperty(ref _cityName, value); }
        }

        /// <summary>
        /// Заполненность
        /// </summary>
        public string Fullness
        {
            get { return _fullness; }
            set { SetProperty(ref _fullness, value); }
        }

        /// <summary>
        /// Триггер для анимации
        /// </summary>
        private bool _startAnimation;
        public bool StartAnimation
        {
            get { return _startAnimation; }
            set
            {
                SetProperty(ref _startAnimation, value);
                SetProperty(ref _startAnimation, false);
            }
        }

        /// <summary>
        /// Метод для передачи информации в контрол
        /// </summary>
        /// <param name="record"></param>
        public void ChangeInfo(FlightRecord record)
        {
            //Заполняем поля полученными данными и запускаем анимацию
            FlightType = record.FlightType;
            CityName = record.CityName;
            InteractTime = record.InteractTime.ToString("HH:mm");
            PlaneType = record.PlaneType;
            Fullness = $"{record.CurrentPassengers}/{record.TotalPassengers}";
            StartAnimation = true;
        }
    }
}
