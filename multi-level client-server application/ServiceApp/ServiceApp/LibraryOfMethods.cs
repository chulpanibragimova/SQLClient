using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginMethods
{
    public static class LibraryOfMethods
    {
        //Получение текущей даты из Юникс-даты
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            unixTimeStamp += 10800;
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }
        //изменение размера массива 
        public static Array ResizeArray(Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
             
            return temp;
        }
        //регистронезависимое сравнение строк
        public static bool REq(string a, string b)
        {
            if (a == null) a = "";
            if (b == null) b = "";
            a = a.ToLower();
            b = b.ToLower();
            return (a == b);
        }
    }
}
