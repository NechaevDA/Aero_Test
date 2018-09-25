using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Aero_Test.ControlVM;
using Aero_Test.Model;
using System.Windows;
using System.IO;
using Prism.Commands;
using Microsoft.Win32;

namespace Aero_Test.ViewModel
{
    class MainWindowVM : BindableBase
    {
        public MainWindowVM()
        {
            _loadFiles = new DelegateCommand(loadFiles);
        }

        /// <summary>
        /// Вывод информации о полетах
        /// </summary>
        public FlightRecordControlVM _flightInfo;
        public FlightRecordControlVM FlightInfo
        {
            get { return _flightInfo; }
            private set { SetProperty(ref _flightInfo, value); }
        }

        /// <summary>
        /// Контроль времени
        /// </summary>
        private TimeControlVM _timeControl;
        public TimeControlVM TimeControl
        {
            get { return _timeControl; }
            private set { SetProperty(ref _timeControl, value); }
        }

        /// <summary>
        /// Статистика вылетов
        /// </summary>
        private SideInfoVM _departureInfo;
        public SideInfoVM DepartureInfo
        {
            get { return _departureInfo; }
            set { SetProperty(ref _departureInfo, value); }
        }

        /// <summary>
        /// Статистика прилетов
        /// </summary>
        private SideInfoVM _arrivalInfo;
        public SideInfoVM ArrivalInfo
        {
            get { return _arrivalInfo; }
            set { SetProperty(ref _arrivalInfo, value); }
        }

        /// <summary>
        /// Диаграмма внизу
        /// </summary>
        private GraphControlVM _graphControl;
        public GraphControlVM GraphControl
        {
            get { return _graphControl; }
            set { SetProperty(ref _graphControl, value); }
        }

        Queue<FlightRecord> FlightQueue = new Queue<FlightRecord>(); //Очередь на вылет/прилет. Из нее берется следующий рейс
        List<FlightRecord> AllFlights = new List<FlightRecord>(); //Список всех полетов
        List<FlightRecord> FlightsWithin24h = new List<FlightRecord>(); //Список полетов за последние 24 часа
        Random R = new Random();        

        /// <summary>
        /// Основной цикл программы, обрабатывает событие Tick DispatcherTimer-а из контроля времени.
        /// В зависимости от скорости времени срабатывает минимум за 6мс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            try
            {
                if (FlightQueue.Peek().InteractTime.Hour == TimeControl.CurrentTime.Hour &&
                    FlightQueue.Peek().InteractTime.Minute == TimeControl.CurrentTime.Minute) //Если текущее время равно времени следующего полета
                {
                    FlightRecord flight = FlightQueue.Dequeue(); //Забираем полет из очереди

                    //Считаем в зависимости от типа полета соответствующую статистику
                    if (flight.FlightType.ToLower() == "вылет")
                    {
                        DepartureInfo.CountLastFlight = flight.CurrentPassengers;
                        DepartureInfo.CountTotal += flight.CurrentPassengers;
                    }
                    else
                    {
                        ArrivalInfo.CountLastFlight = flight.CurrentPassengers;
                        ArrivalInfo.CountTotal += flight.CurrentPassengers;
                    }

                    //Записываем в последние полеты с текущим временем вылета (уичтвает день)
                    flight.InteractTime = TimeControl.CurrentTime;
                    FlightsWithin24h.Add(flight);

                    //Генерируем следующий полет
                    FlightRecord nextFlight = new FlightRecord(flight);
                    nextFlight.CurrentPassengers = R.Next(0, CommonData.PlanesCapacity[nextFlight.PlaneType]);
                    FlightQueue.Enqueue(nextFlight);
                    FlightInfo.ChangeInfo(FlightQueue.Peek());
                }

                //Проверяем список самолетов за последние 24 часа, удаляем которые не вписываются в заданные рамки
                FlightsWithin24h.RemoveAll(x => x.InteractTime < TimeControl.CurrentTime.AddHours(-24));
                //Считаем сумму за последние 24 часа
                DepartureInfo.Count24h = (FlightsWithin24h.Where(x => x.FlightType.ToLower() == "вылет")).Sum(x => x.CurrentPassengers);
                ArrivalInfo.Count24h = (FlightsWithin24h.Where(x => x.FlightType.ToLower() == "прилет")).Sum(x => x.CurrentPassengers);
                if (TimeControl.CurrentTime.Minute == 0) //Каждый час
                {
                    //Сюда можно было бы поместить подсчет статистики
                    GraphControl.SetValues(DepartureInfo.Count24h, ArrivalInfo.Count24h); //устанавливаем значения графиков
                }            
            } catch (Exception E)
            {
                MessageBox.Show(E.Message + "\n" + E.StackTrace);
                TimeControl.Timer.Stop();
            }
        }

        private DelegateCommand _loadFiles;
        public DelegateCommand LoadFiles
        {
            get { return _loadFiles; }
        }

        /// <summary>
        /// Загрузка файлов через диалоговые окна
        /// </summary>
        private void loadFiles()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            string planesPath, flightsPath;
            MessageBox.Show("Выберите файл с типами самолетов");
            if (OFD.ShowDialog() == true)
            {
                planesPath = OFD.FileName;
                MessageBox.Show("Выберите файл с расписанием");
                if (OFD.ShowDialog() == true)
                {
                    flightsPath = OFD.FileName;
                    LoadInfo(planesPath, flightsPath);
                }
                else return;
            }
            else return;
        }
        /// <summary>
        /// Загрузка информации при старте
        /// </summary>
        public void LoadInfo(string planesPath, string flightsPath)
        {
            try
            {                
                TimeControl = new TimeControlVM();
                TimeControl.Timer.Tick += Update;

                if (!DataLoader.LoadPlanes(ref CommonData.PlanesCapacity, planesPath)) //Загружаем список типов самолетов
                {
                    MessageBox.Show("Файл с типами самолетов не найден. Работа программы остановлена");
                    TimeControl.Timer.Stop();
                }

                if (File.Exists(flightsPath)) //Загружаем список полетов
                {
                    AllFlights = DataLoader.LoadFlightsList(ref FlightQueue, flightsPath);
                    //Прокручиваем очередь для того, чтобы получить полет, следующий за временем запуска программы.
                    //А не первый после полуночи
                    FlightRecord tmp;
                    if (AllFlights.Where(x => x.InteractTime.TimeOfDay <= TimeControl.CurrentTime.TimeOfDay).Count() != AllFlights.Count())
                        while (FlightQueue.Peek().InteractTime.TimeOfDay < TimeControl.CurrentTime.TimeOfDay)
                        {
                            tmp = FlightQueue.Dequeue();
                            FlightQueue.Enqueue(tmp);
                        }
                    FlightInfo = new FlightRecordControlVM(FlightQueue.Peek());

                }
                else
                {
                    MessageBox.Show("Файл с записями полетов не найден. Работа программы остановлена");
                    TimeControl.Timer.Stop();
                }

                

                ArrivalInfo = new SideInfoVM("Статистика: прилеты");
                DepartureInfo = new SideInfoVM("Статистика: вылеты");
                GraphControl = new GraphControlVM();
            }
            catch (KeyNotFoundException E)
            {
                MessageBox.Show("Нет связи по типу самолета. (Отсутствует в словаре) Работа прекращена");
                TimeControl.Timer.Stop();
            }
            catch (FileNotFoundException E)
            {
                MessageBox.Show(E.Message + "\nРабота программы остановлена");
            }
            catch (Exception E)
            {
                MessageBox.Show("Возникла ошибка: " + E.Message + "\n" + (E.InnerException?.Message ?? "" + "\nРабота программы остановлена"));
            }
        }        
    }
}
