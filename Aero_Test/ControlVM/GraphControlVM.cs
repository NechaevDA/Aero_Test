using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Aero_Test.ControlVM
{
    class GraphControlVM : BindableBase
    {
        public GraphControlVM()
        {
            DepartureSummary = "0% (0)";
            ArrivalSummary = "0% (0)";
            DeparturePercentage = 0;
            ArrivalPercentage = 0;
            Animate = false;
        }

        private string _departureSummary;
        private string _arrivalSummary;
        private int _arrivalPercentage;
        private int _departurePercentage;        
        private bool _animate;

        /// <summary>
        /// Триггер для запуска анимации
        /// </summary>
        public bool Animate
        {
            get { return _animate; }
            set
            {
                SetProperty(ref _animate, value);
                SetProperty(ref _animate, false);
            }
        }                

        /// <summary>
        /// Сводка по вылетам (0% (0))
        /// </summary>
        public string DepartureSummary
        {
            get { return _departureSummary; }
            set { SetProperty(ref _departureSummary, value); }
        }

        /// <summary>
        /// Сводка по прилетам (0% (0))
        /// </summary>
        public string ArrivalSummary
        {
            get { return _arrivalSummary; }
            set { SetProperty(ref _arrivalSummary, value); }
        }

        /// <summary>
        /// Процент прилетов относительно общего числа полетов
        /// </summary>
        public int ArrivalPercentage
        {
            get { return _arrivalPercentage; }
            set
            {                
                SetProperty(ref _arrivalPercentage, value);
            }
        }

        /// <summary>
        /// Процент вылетов относительно общего числа полетов
        /// </summary>
        public int DeparturePercentage
        {
            get { return _departurePercentage; }
            set
            {
                if (value != _departurePercentage) Animate = true;
                SetProperty(ref _departurePercentage, value);
            }
        }

        /// <summary>
        /// Метод установки значений для контрола
        /// </summary>
        /// <param name="departureSum">Количество вылетов за последние 24ч</param>
        /// <param name="arrivalSum">Количество прилетов за послдение 24ч</param>
        public void SetValues(int departureSum, int arrivalSum)
        {
            int totalSum = departureSum + arrivalSum; //Считаем общую сумму полетов
            if (totalSum == 0) return; //Если полетов не было, нечего и считать (чтобы исключить деление на 0)
            int arrivalPercent = (int)Math.Ceiling(arrivalSum * 1.0 / totalSum * 100.0); //Устанавливаем процент прилетов
            int departurePercent = (int)Math.Floor(departureSum * 1.0 / totalSum * 100.0); //Устанавливаем процент вылетов. Ceiling и Floor используются, чтобы в сумме было 100%

            //Передаем значения в элементы контрола
            ArrivalPercentage = arrivalPercent;
            DeparturePercentage = departurePercent;            

            DepartureSummary = $"{DeparturePercentage}% ({departureSum})";
            ArrivalSummary = $"{ArrivalPercentage}% ({arrivalSum})";           
        }
        
    }    
}
