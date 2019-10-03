using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CuttingTask;

namespace TestingConsole
{
    class Tester
    {
        static void Main()
        {
            #region Считывание Ширины Количества Имён и проверка
            List<int> desired = new List<int>(150);
            using (StreamReader reader = new StreamReader("widthes.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null)
                {
                    desired.Add(Convert.ToInt32(strNum));
                }
            }

            List<string> names = new List<string>(desired.Count);
            using (StreamReader reader = new StreamReader("names.txt"))
            {
                string name;
                while ((name = reader.ReadLine()) != null)
                {
                    names.Add(name);
                }
            }

            List<decimal> counts = new List<decimal>(desired.Count);
            using (StreamReader reader = new StreamReader("counts.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null)
                {
                    counts.Add(Convert.ToDecimal(strNum));
                }
            }

            if (counts.Count != desired.Count || desired.Count != names.Count)
            {
                throw new Exception("Не равное количество параметров");
            }
            #endregion

            const int POSSIBLE_LENGTH = 6000;
            const int LIMIT_ON_CUTS = 6;
            List<CommonInfo> commonInfo = new List<CommonInfo>(desired.Count);

            for (var i = 0; i < desired.Count; i++)
            {
                commonInfo.Add(new CommonInfo(names[i], desired[i], counts[i]));
            }

            Solver solver = new Solver();
            object[] results = solver.CalculateCuts(POSSIBLE_LENGTH, LIMIT_ON_CUTS, commonInfo);

            var planks = (List<Plank>)results[0];
            var residualInfo = (List<CommonInfo>)results[1];

            Console.WriteLine("Планки");
            foreach (var plank in planks)
            {
                Console.WriteLine(plank);
            }
            Console.WriteLine("Остаточная информация");
            foreach (var info in residualInfo)
            {
                Console.WriteLine(info);
            }
            Console.ReadKey(true);
        }
    }
}
