using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MostGameTask3
{
    /// <summary>
    /// Сам класс имеет в себе лист из других таких же объектов класса (не стал создавать отдельный класс, либо хранить только строки)
    /// интересно было бы узнать мнение специалистов, плохо это или хорошо)
    /// по логике работы - есть два класса: публичный для создания главного инстанса
    /// приватный класс уже нужен для создания объекта, который помещается в лист тех строк, которые совпадают по индексу
    /// всю информацию о них можно будет получить, так как лист публичный
    /// сеттеры приватные, т.к. подразумевается, что вся информация делается внутри класса и изменять их снаружи никак нельзя
    /// </summary>
    public class CountIndexPetrenko
    {
        public double Index { get; private set; }
        public string String { get; private set; }
        public string Comment { get; private set; }

        public List<CountIndexPetrenko> StringsWithSameIndex { get; private set; }

        private const double _minIndex = 0.5;
        private int _trueStrLen;

        private string _separatorChars = " .,;:?\'\")[]{}<>!|/\\~+#%&^*-=";
        private string _rusChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        public CountIndexPetrenko(string str)
        {
            var splittedStr =  CutComment(str);
            String = splittedStr[0];
            Comment = splittedStr[1];
            if (!ValidateComment(Comment))
                throw new ArgumentException("Comment is empty or too big");
            TakeIndex();
            StringsWithSameIndex = new();
        }

        private CountIndexPetrenko (string str, string comment)
        {
            String = str;
            Comment = comment;
            TakeIndex();
        }

        private string[] CutComment(string str)
        {
            string[] splittedStringByContentAndComment = new string[2];
            splittedStringByContentAndComment = str.Split('|');
            return splittedStringByContentAndComment;
        }

        private void TakeIndex()
        {
            double result;

            _trueStrLen = String.Length - CountNonLetetter();
            result = CountBaseIndex();
            result *= _trueStrLen;
            Index = result;
        }

        private int CountNonLetetter()
        {
            int separatorCount = String.Count(c => _separatorChars.Contains(Char.ToLower(c)));
            return separatorCount;
        }

        private double CountBaseIndex()
        {
            double result;

            if (_trueStrLen % 2 == 0)
                result = _trueStrLen * _trueStrLen / 2;
            else
                result = (_trueStrLen * _trueStrLen / 2) + _minIndex;
            return result;
        }

        public void CheckString(string str)
        {
            var splittedString = CutComment(str);
            if (splittedString.Length != 2)
                throw new ArgumentException("Comparing string has a bad comment format");
            ValidateComment(splittedString[1]);
            CountIndexPetrenko comparingString = new(splittedString[0], splittedString[1]);
            Regex r = new Regex(@"[абвгдеёжзийклмнопрстуфхцчшщъыьэюя]");

            if (r.IsMatch(comparingString.String))
            {
                Console.WriteLine($"Your string has a russian letters");
                return;
            }
            if (comparingString.Index == Index)
                StringsWithSameIndex.Add(comparingString);
            else
                Console.WriteLine($"string {comparingString.String} has a differrent index");
        }

        public void CheckString(List<string> strings)
        {
            foreach(var str in strings)
                CheckString(str);
        }

        public void GetAllStringsWithSameIndex ()
        {
            foreach (var str in StringsWithSameIndex)
                Console.WriteLine(str.String);
        }

        private bool ValidateComment(string comment)
        {
            if (comment is null)
                return false;
            string cutString = comment.Trim();
            var words = cutString.Split(' ');
            if (words.Length > 5)
                return false;
            if (words[0] == "")
                throw new ArgumentException("Comment is empty");
            return true;
        }
    }
}//TODO сделать проверку на то, что родитель из русских букв. Проверка коммента по длине.
//проверка на несколько комментов и на пустой коммент (от 1 до 5 слов)