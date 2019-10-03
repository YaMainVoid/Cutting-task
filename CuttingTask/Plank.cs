using System;
using System.Collections.Generic;

namespace CuttingTask
{
    public class Plank
    {
        private int _сountDesiredInPlank, _possibleLength, _diameret;

        public int CountDesiredInPlank
        {
            get
            {
                return _сountDesiredInPlank;
            }
            set
            {
                if (value < 2 || value > 100) throw new Exception("Ограничение на количество разрезов должно находиться в передлах от 2 до 100");
                else _сountDesiredInPlank = value;
            }
        }

        public int PossibleLength
        {
            get
            {
                return _possibleLength;
            }
            set
            {
                if (value <= 0) throw new Exception("Максимальная длина не может быть отрицательна");
                else _possibleLength = value;
            }
        }

        public int Diameter
        {
            get
            {
                return _diameret;
            }
            private set
            {
                if (value < 0) throw new Exception("Диаметр не может быть отрицательным");
                else _diameret = value;
            }
        }

        public int CurrentCountDesiredInPlank { get; private set; }

        public int FreeLength { get; private set; }

        public int HowMany { get; internal set; }

        public List<int> Cuts { get; private set; }

        public List<string> Sleeves { get; private set; }

        public List<string> Names { get; private set; }

        // public Weight { get; private set; }

        // сколько было 
        public List<decimal> InitialCount { get; private set; }

        // сколько осталось
        public List<decimal> FinalCount { get; set; }

        /// <summary>
        /// Создает представление планки
        /// </summary>
        /// <param name="possibleLength">Длина большого рулона</param>
        /// <param name="countDesiredInPlank">Ограничение на количество разрезов</param>
        public Plank(int possibleLength, int countDesiredInPlank, int diameter)
        {
            Diameter = diameter;
            FreeLength = possibleLength;
            PossibleLength = possibleLength;
            HowMany = 1;
            CountDesiredInPlank = countDesiredInPlank;
            CurrentCountDesiredInPlank = 1;
            Cuts = new List<int>(countDesiredInPlank);
            InitialCount = new List<decimal>(countDesiredInPlank);
            FinalCount = new List<decimal>(countDesiredInPlank);
            Names = new List<string>(countDesiredInPlank);
            Sleeves = new List<string>(countDesiredInPlank);
        }

        /// <summary>
        /// Метод добавляет планку
        /// </summary>
        /// <param name="length">длина планки, которую нужно добавить </param>
        /// <param name="name">название заказа</param>
        /// <param name="sleeve">характеристики гильзы</param>
        public void Cut(int length, string name, decimal initialCount, string sleeve)
        {
            InitialCount.Add(initialCount);
            Cuts.Add(length);
            Names.Add(name);
            Sleeves.Add(sleeve);
            FreeLength -= length;
            CurrentCountDesiredInPlank++;
        }

        public void CalculateWeights()
        {
            throw new NotImplementedException("Метод еще не не реализован");
        }

        public double GetWasteInPersents()
        {
            return FreeLength / (double)PossibleLength * 100;
        }

        public override string ToString()
        {
            string s = "\\o/==========================================================\\o/\nРазрезы: \n";
            for (var i = 0; i < Cuts.Count; i++)
            {
                s = s + "Заказ: " + Names[i] + " Длина: " + Cuts[i] + " Гильза: " + Sleeves[i] + " Было: " + InitialCount[i] + " Стало: " + FinalCount[i] + "\n";
            }
            s += "Сколько раз нужно изготовить: " + HowMany + "; Отходы: " + FreeLength +   "; ";
            s = s + String.Format("потери в процентах: {0:0.00}", GetWasteInPersents());
            s += "\n\\o/==========================================================\\o/\n";
            return s;

        }
    }
}
