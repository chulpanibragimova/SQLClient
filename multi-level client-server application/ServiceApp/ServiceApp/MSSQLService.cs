using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MsSqlClient;

namespace ServiceApp
{
    public class Container//Контейнер для передачи данных
    {
        public string[] data;//Передаваемые данные
        public int a;//Нулевая размерность двумерного массива
        public int b; //Первая размерность двумерного массива
    }
    public static class PackerUnpacker
    {
        public static Container Pack(string[,] data)//Метод, преобразовывающий двумерный массив в одномерный
        {
            Container ret = new Container();//Создаём контейнер
            ret.a = data.GetLength(0);//Получаем нулевую размерность
            ret.b = data.GetLength(1);//Получаем первую размерность
            ret.data = new string[ret.a * ret.b];//Размечаем одномерный массив с кол-вом ячеек, равным передаваемому двумерному массиву
            for (int i = 0; i < ret.a; i++)//Два цикла для прохода по двумерному массиву
            {
                for (int k = 0; k < ret.b; k++)
                {
                    ret.data[i * ret.b + k] = data[i, k];//Запись двумерного массива в одномерный
                }
            }
            return ret;//Возвращаем контейнер.
        }
    }
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "MSSQLService" в коде и файле конфигурации.
    public class MSSQLService : IMSSQLService
    {
       public string ConnectToDbAdapter(string login, string password, string server = "./MSSQLSERVER01")
        {
            return MsSqlClientTools.ConnectToDb(login, password, server);
        }
        public string InsertionQueryAdapter(string Query)
        {
            return MsSqlClientTools.InsertionQuery(Query);
        }
        public string UpdationQueryAdapter(string Query)
        {
            return MsSqlClientTools.UpdationQuery(Query);
        }
        public string SelectionQueryAdapter(string Query)
        {
            return MsSqlClientTools.SelectionQuery(Query);
        }
        public string DeletionQueryAdapter(string Query)
        {
            return MsSqlClientTools.DeletionQuery(Query);
        }
        public Container GetQueryResult()
        {//За нас всю сетевую часть делает windows по этому никакой явной отправки не требуется. Всё скомпилируется само как надо, а последняя точка цепочки отправки для программиста - ключевое слово return
            return PackerUnpacker.Pack(MsSqlClientTools.ResultAsStringArray);//Преобразует результаты, которые хранятся в виде двумерного массива строк 
            //в одномерный массив строк внутри Container и отправляет его клиенту
        }
        public string CloseConnection()
        {
            MsSqlClientTools.connection.Close();
            return "Connection closed";
        }
    }
}
