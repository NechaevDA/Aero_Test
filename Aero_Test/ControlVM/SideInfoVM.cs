using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Aero_Test.ControlVM
{
    class SideInfoVM : BindableBase
    {
        public SideInfoVM()
        {
            Header = "---//---";
            CountLastFlight = 0;
            Count24h = 0;
            CountTotal = 0;
        }

        public SideInfoVM(string header)
        {
            Header = header;
            CountLastFlight = 0;
            Count24h = 0;
            CountTotal = 0;
        }

        private int _countLastFlight;
        private int _count24h;
        private int _countTotal;

        private string _displayLastFlight;
        private string _display24h;
        private string _displayTotal;

        private string _header;

        /// <summary>
        /// За последний рейс (число)
        /// </summary>
        public int CountLastFlight
        {            
            get { return _countLastFlight; }
            set
            {
                DisplayLastFlight = $"За последний рейс: {value}";
                SetProperty(ref _countLastFlight, value);
            }
        }

        /// <summary>
        /// Строка для вывода в поле
        /// </summary>
        public string DisplayLastFlight
        {
            get { return _displayLastFlight; }
            set { SetProperty(ref _displayLastFlight, value); }
        }

        /// <summary>
        /// За 24 часа (число)
        /// </summary>
        public int Count24h
        {
            get { return _count24h; }
            set
            {
                Display24h = $"За последние 24 часа: {value}";
                SetProperty(ref _count24h, value);
            }
        }

        /// <summary>
        /// За 24 часа строка для вывода
        /// </summary>
        public string Display24h
        {
            get { return _display24h; }
            set { SetProperty(ref _display24h, value); }
        }

        /// <summary>
        /// Всего (число)
        /// </summary>
        public int CountTotal
        {
            get { return _countTotal; }
            set
            {
                DisplayTotal = $"За все время: {value}";
                SetProperty(ref _countTotal, value);
            }
        }

        /// <summary>
        /// Всего (строка для вывода)
        /// </summary>
        public string DisplayTotal
        {
            get { return _displayTotal; }
            set { SetProperty(ref _displayTotal, value); }
        }

        /// <summary>
        /// Заголовок формы (чтобы не делать отдельно для правой и левой части)
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }
    }
}
