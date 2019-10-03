using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingTask;

namespace CuttingTask
{
    public class Solver
    {
        private decimal possibleWaste = 0.04m;
        private void Sort(List<CommonInfo> commonInfo, Func<CommonInfo, CommonInfo, decimal> compareFunc)
        {
            for (var i = 0; i < commonInfo.Count; i++)
            {
                for (var j = i + 1; j < commonInfo.Count; j++)
                {
                    if (compareFunc(commonInfo[i], commonInfo[j]) > 0)
                    {
                        var temp = commonInfo[i];
                        commonInfo[i] = commonInfo[j];
                        commonInfo[j] = temp;
                    }
                }
            }
        }

        private List<CommonInfo> RemoveZeroInfo(List<CommonInfo> commonInfo)
        {
            List<CommonInfo> newInfo = new List<CommonInfo>(commonInfo.Count);
            foreach (var info in commonInfo)
            {
                if (info.Count != 0)
                {
                    newInfo.Add(info);
                }
            }
            return newInfo;
        }

        private List<Plank> FillPlanks(int possibleLength, int limitOnCuts, List<CommonInfo> commonInfo, int diameter)
        {
            Sort(commonInfo, (a, b) => b.Count - a.Count);
            List<Plank> planks = new List<Plank>(50);

            // пройдемся по всем ширинам 
            foreach (var chunkInfo in commonInfo)
            {
                // если не найдено подходящих досок то создади новую
                if (!planks.Any(plank => plank.FreeLength >= chunkInfo.Width && plank.CurrentCountDesiredInPlank < plank.CountDesiredInPlank))
                {
                    planks.Add(new Plank(possibleLength, limitOnCuts, diameter));
                }

                // режем (т. е. добавляем), где можем
                foreach (var plank in planks.Where(plank => plank.FreeLength >= chunkInfo.Width && plank.CurrentCountDesiredInPlank <= plank.CountDesiredInPlank))
                {
                    plank.Cut(chunkInfo.Width, chunkInfo.Name, chunkInfo.Count, chunkInfo.Sleeve);
                    break;
                }
            }

            foreach (var plank in planks)
            {
                int min = int.MaxValue;
                foreach (var count in plank.InitialCount)
                {
                    if (min > count)
                    {
                        min = (int)count;
                    }
                }
                plank.HowMany = min;
                for (var i = 0; i < plank.Cuts.Count; i++)
                {
                    plank.FinalCount.Add(plank.InitialCount[i] - min);
                }
            }

            return planks;
        }

        private object[] SeparatePlanks(List<Plank> planks)
        {
            List<Plank> suitablePlanks = new List<Plank>((int)(planks.Count / 1.3));
            List<CommonInfo> commonInfo = new List<CommonInfo>(planks.Count);

            // проходит по всем планкам: если отходы больше 4 процетов то не включает эту планку а разбирает ее
            // в commonInfo так же если отсалось более 1 раза на производство то он закинит эту инфу в commonInfo
            foreach (var plank in planks)
            {
                if (plank.FreeLength / (decimal)plank.PossibleLength <= possibleWaste)
                {
                    suitablePlanks.Add(plank);

                    for (var i = 0; i < plank.FinalCount.Count; i++)
                    {
                        if ((int)plank.FinalCount[i] == 0)
                        {
                            continue;
                        }
                        commonInfo.Add(new CommonInfo(plank.Names[i], plank.Cuts[i], plank.FinalCount[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < plank.Cuts.Count; i++)
                    {
                        commonInfo.Add(new CommonInfo(plank.Names[i], plank.Cuts[i], plank.FinalCount[i]));
                    }
                }
                
            }
            return new object[2] { suitablePlanks, commonInfo };
        }

        public object[] CalculateCuts(int possibleLength, int limitOnCuts, List<CommonInfo> commonInfo, int diameter = 500)
        {
            // commonInfo = RemoveZeroInfo(commonInfo);
            List<Plank> planks = new List<Plank>(200);

            for (int i = 0; i < 10; i++)
            {
                var tmpPlanks = FillPlanks(possibleLength, limitOnCuts, commonInfo, diameter);
                var separated = SeparatePlanks(tmpPlanks);
                var donePlanks = (List<Plank>)separated[0];
                commonInfo = (List<CommonInfo>)separated[1];
                // commonInfo = RemoveZeroInfo(commonInfo);
                foreach (var plank in donePlanks)
                {
                    planks.Add(plank);
                }
            }

            foreach (var plank in planks)
            {
                if (plank.HowMany == 0)
                {
                    for (int i = 0; i < plank.Cuts.Count; i++)
                    {
                        commonInfo.Add(new CommonInfo(plank.Names[i], plank.Cuts[i], plank.FinalCount[i]));
                    }
                }
            }
            planks = planks.Where(plank => plank.HowMany != 0).ToList();

            return new object[2] { planks, commonInfo };
        }
    }
}
