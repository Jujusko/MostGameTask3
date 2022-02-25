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
    /// 
    public class CountIndexPetrenko
    {
        private enum RusOrEng
        {
            Rus,
            Eng
        }
        public double Index { get; private set; }
        public string String { get; private set; }
        public string Comment { get; private set; }

        public List<CountIndexPetrenko> StringsWithSameIndex { get; private set; }

        private const double _minIndex = 0.5;
        private int _trueStrLen;

        private string _separatorChars = " .,;:?\'\")[]{}<>!|/\\~+#%&^*-=";
        //private string _rusChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        //private string _engSymbols = "abcdefghijklmnopqrstuvwxyz";
        private string _badComment = "String have a bad comment format";
        private string _badString = "String have a bad string format";
        public CountIndexPetrenko(string str)
        {
            var splittedStr =  CutComment(str);
            String = splittedStr[0];
            Comment = splittedStr[1];
            if (!ValidateString(String, RusOrEng.Rus))
                throw new ArgumentException("String has a english letters");
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

        private string[]? CutComment(string str)
        {
            string[] splittedStringByContentAndComment;
            splittedStringByContentAndComment = str.Split('|');
            if (splittedStringByContentAndComment.Length > 2 || splittedStringByContentAndComment.Length == 1)
                return null;
                //throw new ArgumentException("Bad string format (input string format \"text\"|\"comment\")");
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
        private bool ValidateComment(string comment)
        {
            if (comment is null)
                return false;
            string cutString = comment.Trim();
            var words = cutString.Split(' ');
            if (words.Length > 5 || words[0] == "")
            {
                Console.WriteLine(_badComment);
                return false;
            }
            return true;
        }

        private bool ValidateString (string text, RusOrEng rusOrEng)
        {
            if (rusOrEng is RusOrEng.Rus)
            {
                Regex r = new Regex(@"[abcdefghijklmnopqrstuvwxyz]");

                if (r.IsMatch(text))
                {
                    Console.WriteLine(_badString);
                    return false;
                }
            }
            else if (rusOrEng is RusOrEng.Eng)
            {
                Regex r = new Regex(@"[абвгдеёжзийклмнопрстуфхцчшщъыьэюя]");

                if (r.IsMatch(text))
                {
                    Console.WriteLine(_badString);
                    return false;
                }
            }
            return true;
        }

        //method for child string
        public void CheckString(string str)
        {
            var splittedString = CutComment(str);
            if (splittedString is null)
            {
                Console.WriteLine(_badString);
                return;
            }
            if (!ValidateString(splittedString[0], RusOrEng.Eng))
                return;
            if (!ValidateComment(splittedString[1]))
                return;
            CountIndexPetrenko comparingString = new(splittedString[0], splittedString[1]);
            Regex r = new Regex(@"[абвгдеёжзийклмнопрстуфхцчшщъыьэюя]");

            if (r.IsMatch(comparingString.String))
            {
                Console.WriteLine(_badString);
                return;
            }
            if (comparingString.Index == Index)
                StringsWithSameIndex.Add(comparingString);
            else
                Console.WriteLine($"string {comparingString.String} has a differrent index");
        }

        //method for child string
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

    }
}//TODO сделать проверку на то, что родитель из русских букв. Проверка коммента по длине.
//проверка на несколько комментов и на пустой коммент (от 1 до 5 слов)