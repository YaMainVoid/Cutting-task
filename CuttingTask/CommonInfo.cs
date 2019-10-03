using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingTask
{
    public class CommonInfo
    {
        private string _name, _sleeve;
        private int _width;
        private decimal _count;

        public string Name
        {
            get { return _name; }
            private set { if (string.IsNullOrEmpty(value)) throw new Exception("Название заказа отсутствует"); else _name = value; }
        }
        public decimal Count
        {
            get { return _count; }
            private set { if (value < 0) throw new Exception("Количество не должно быть отрицательным"); else _count = value; }
        }
        public int Width
        {
            get { return _width; }
            private set { if (value <= 0) throw new Exception("Ширина не может быть отрицательной"); else _width = value; }
        }
        public string Sleeve
        {
            get { return _sleeve; }
            private set { if (string.IsNullOrEmpty(value)) throw new Exception("Название гильзы отсутствует"); else _sleeve = value; }
        }

        public CommonInfo(string name, int width, decimal count, string sleeve = "777")
        {
            Name = name;
            Width = width;
            Count = count;
            Sleeve = sleeve;
        }

        public override string ToString()
        {
            string s = "";

            s = $"Номер заказа: {Name} Ширина: {Width} Количество: {Count} Гильза: {Sleeve}";

            return s;
        }
    }
}
