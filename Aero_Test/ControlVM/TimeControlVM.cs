using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Windows.Threading;
using Prism.Commands;

namespace Aero_Test.ControlVM
{
    class TimeControlVM : BindableBase
    {
        public TimeControlVM()
        {
            _increaseTimeSpan = new DelegateCommand(increaseTimeSpan);
            _decreaseTimeSpan = new DelegateCommand(decreaseTimeSpan);

            CurrentMultiplier = "1"; //Множитель времени (строковый вывод)
            CurrentTime = DateTime.Now;
            Time = CurrentTime.ToString("HH:mm");

            Timer = new DispatcherTimer();
            Timer.Interval = DefaultTimeSpan;
            Timer.Start();

            Timer.Tick += IncreaseTime;
        }

        public DateTime CurrentTime; //Текущее время
        TimeSpan DefaultTimeSpan = new TimeSpan(0, 0, 1, 0, 0); //Для времени 1х достаточно срабатывания таймера 1 раз в минуту.
        public DispatcherTimer Timer { get; set; }
        private int TimerMultipier = 1; //Множитель скорости времени

        private DelegateCommand _increaseTimeSpan;
        private DelegateCommand _decreaseTimeSpan;
        private string _currentMultiplier = "";
        private string _time = "";

        /// <summary>
        /// Команда - увеличить скорость
        /// </summary>
        public DelegateCommand IncreaseTimeSpan
        {
            get { return _increaseTimeSpan; }
        }

        /// <summary>
        /// Команда - уменьшить скорость
        /// </summary>
        public DelegateCommand DecreaseTimeSpan
        {
            get { return _decreaseTimeSpan; }
        }

        /// <summary>
        /// Вывод множителя
        /// </summary>
        public string CurrentMultiplier
        {
            get { return _currentMultiplier; }
            set
            {
                SetProperty(ref _currentMultiplier, value + "x");
            }
        }

        /// <summary>
        /// Вывод текущего времени
        /// </summary>
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        //Логика работы со временем:
        //Скорость 1х - интервал таймера 1 мин (60000 мс)
        //Скорость 10000х - интервал таймера 6 мс

        /// <summary>
        /// Увеличить скорость
        /// </summary>
        private void increaseTimeSpan()
        {
            if (TimerMultipier < 10000) //Максимальная скорость - 10000х
            {
                TimerMultipier *= 10;
                Timer.Interval = new TimeSpan(0, 0, 0, 0, (60000 / TimerMultipier));
                CurrentMultiplier = TimerMultipier.ToString();            
            }
        }

        /// <summary>
        /// Уменьшить скорость
        /// </summary>
        private void decreaseTimeSpan()
        {
            if (TimerMultipier > 1) //Минимальная скорость - 1х 
            {
                TimerMultipier /= 10;
                Timer.Interval = new TimeSpan(0, 0, 0, 0, (60000 / TimerMultipier));
                CurrentMultiplier = TimerMultipier.ToString();
            }
        }

        /// <summary>
        /// Счетчик текущего времени. Обработчик события Tick DispatcherTimer-а
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncreaseTime(object sender, EventArgs e)
        {
            CurrentTime = CurrentTime.Add(new TimeSpan(0, 1, 0));
            Time = CurrentTime.ToString("HH:mm");
        }
    }
}
