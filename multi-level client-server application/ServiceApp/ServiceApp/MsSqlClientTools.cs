using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Security;
using OriginMethods;

namespace MsSqlClient
{
    public static class MsSqlClientTools
    {
        public static string ConnectionString = @"Initial Catalog=Расчет;Integrated Security=True";
        public static SqlConnection connection = null;
        /*
         Data Source: указывает на название сервера. По умолчанию это ".\SQLEXPRESS". 
         Поскольку в строке используется слеш, то в начале строки ставится символ @.
         Если имя сервера базы данных отличается, то соответственно его и надо использовать.

         Initial Catalog: указывает на название базы данных на сервере

         Integrated Security: устанавливает проверку подлинности

            Application Name: название приложения.
            Может принимать в качестве значения любую строку. Значение по умолчанию: ".Net SqlClient Data Provide"

AttachDBFileName: хранит полный путь к прикрепляемой базе данных

Connect Timeout: временной период в секундах,
через который ожидается установка подключения.
Принимает одно из значений из интервала 0–32767. По умолчанию равно 15. 
В качестве альтернативного названия параметра может использоваться Connection Timeout

Data Source: название экземпляра SQL Servera, 
с которым будет идти взаимодействие. Это может быть 
название локального сервера, например, "EUGENEPC/SQLEXPRESS",
либо сетевой адрес.

В качестве альтернативного названия параметра можно 
использовать Server, Address, Addr и NetworkAddress

Encrypt: устанавливает шифрование SSL при подключении.
Может принимать значения true, false, yes и no. По умолчанию значение false

Initial Catalog: хранит имя базы данных

В качестве альтернативного названия параметра можно использовать Database

Integrated Security: задает режим аутентификации.
Может принимать значения true, false, yes, no и sspi. По умолчанию значение false

В качестве альтернативного названия параметра 
может использоваться Trusted_Connection

Packet Size: размер сетевого пакета в байтах. 
Может принимать значение, которое кратно 512. По умолчанию равно 8192

Persist Security Info: указывает, должна ли конфиденциальная информация передаваться обратно при подключении.
Может принимать значения true, false, yes и no. По умолчанию значение false

Workstation ID: указывает на рабочую станцию - имя локального компьютера, на котором запущен SQL Server

Password: пароль пользователя

User ID: логин пользователя
         */

        public static string[,] ResultAsStringArray;//Сюда возвращается результат запросов в виде двумерного массива строк

        public static string loggedUsr="";
        public static string ConnectToDb(string login, string password, string server= "./MSSQLSERVER01")
        {
            string curConnect = @"Data Source="+server+";"+ ConnectionString;///Создаём текущую строку подключения
            curConnect += ";User ID=" + login;//С логином 
            curConnect += ";Password=" + password;// И паролем
            //Каждая такая строка будет уникальной, по этому создаётся адаптивно при подключении к базе данных.

            
            // Создание подключения
            connection = new SqlConnection(curConnect);
            try
            {
                // Открываем подключение
                connection.Open();//В теории тут мы должны поймать ошибку о неправильном пароле.
                loggedUsr = login;
                return "Connection established";
            }
            catch (SqlException ex)
            {
                return "-1"+ex.Message;
            } 
        }
        public static string InsertionQuery(string Query)
        {
            try
            {
                SqlCommand command = new SqlCommand(Query, connection);//Создаём SQL команду
                int tmp = command.ExecuteNonQuery();//Выполняем без возврата таблицы, потому что Insert этого не делает. Вместо этого 
                ResultAsStringArray = null;
                ResultAsStringArray = new string[1, 2];
                ResultAsStringArray[0, 0] = "InsertedString";
                ResultAsStringArray[0, 1] = tmp.ToString();// мы получаем число изменённых строк
                //Массив строк создаётся для последующего преобразования в DataGrid
                return "QueryExecuted";
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public static string UpdationQuery(string Query)
        {
            try
            {
                SqlCommand command = new SqlCommand(Query, connection);
                int tmp = command.ExecuteNonQuery();
                ResultAsStringArray = null;
                ResultAsStringArray = new string[1, 2];
                ResultAsStringArray[0, 0] = "UpdatedString";
                ResultAsStringArray[0, 1] = tmp.ToString();
                return "QueryExecuted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string SelectionQuery(string Query)
        {
            try
            {
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int width = reader.VisibleFieldCount;
                    ResultAsStringArray = null;
                    ResultAsStringArray = new string[width, 1];
                    for(int i=0; i<width; i++)
                    {
                        ResultAsStringArray[i, 0] = reader.GetName(i);//Здесь, возможно придётся изменить на i+1 в reader.GetName 
                    }
                    int tmp = 1;
                    while (reader.Read())
                    {
                        if (tmp > 0)//Данный блок производит ресайз и копирование массива полученных строк
                        {
                            string[,] tempArr = (string[,])LibraryOfMethods.ResizeArray(ResultAsStringArray, new int[2] { ResultAsStringArray.GetLength(0), tmp + 1 });
                            for (int i = 0; i < ResultAsStringArray.GetLength(0); i++)
                            {
                                for (int j = 0; j < ResultAsStringArray.GetLength(1); j++)
                                {
                                    tempArr[i, j] = ResultAsStringArray[i, j];
                                }
                            }
                            ResultAsStringArray = tempArr;
                        }
                        //Далее считываем полученные данные
                        for (int i = 0; i < width; i++)
                        {
                            if (reader.GetFieldType(i).ToString() == "System.Int32")
                            {
                                ResultAsStringArray[i, tmp] = reader.GetInt32(i).ToString();
                            }
                            else if (reader.GetFieldType(i).ToString() == "System.Decimal")
                            {
                                ResultAsStringArray[i, tmp] = reader.GetDecimal(i).ToString();
                            }
                            else if (reader.GetFieldType(i).ToString() == "System.Int64")
                            {
                                ResultAsStringArray[i, tmp] = reader.GetInt64(i).ToString();
                            }
                            else if (reader.GetFieldType(i).ToString() == "System.DateTime")
                            {
                                ResultAsStringArray[i, tmp] = reader.GetDateTime(i).Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
                            }
                            else if (reader.GetFieldType(i).ToString() == "System.Double")
                            {
                                ResultAsStringArray[i, tmp] = reader.GetDouble(i).ToString();
                            }
                            else
                            {
                                ResultAsStringArray[i, tmp] = reader.GetString(i);
                            } 
                        }
                        tmp++;
                    }
                    reader.Close();
                    return "QueryExecuted";
                }
                else
                {
                    return "NoSuchData";
                } 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string DeletionQuery(string Query)
        {
            try
            {
                SqlCommand command = new SqlCommand(Query, connection);
                int tmp = command.ExecuteNonQuery();
                ResultAsStringArray = null;
                ResultAsStringArray = new string[1, 2];
                ResultAsStringArray[0, 0] = "DeletedString";
                ResultAsStringArray[0, 1] = tmp.ToString();
                return "QueryExecuted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string ExecutionQuery(string Query)
        {
            try
            {
                SqlCommand command = new SqlCommand(Query, connection);
                int tmp = command.ExecuteNonQuery();
                ResultAsStringArray = null;
                ResultAsStringArray = new string[1, 2];
                ResultAsStringArray[0, 0] = "Result";
                ResultAsStringArray[0, 1] = tmp.ToString();
                return "QueryExecuted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static DataView GenerateTables(string[,] Data)//Преобразование двумерного массива в DataView для DataGrid
        {
            if (Data != null)
            {

                int indexColums = Data.GetLength(0), indexRows = Data.GetLength(1);
                DataTable dt = new DataTable("SQL view");
                for (int k = 0; k < indexColums; k++)
                {
                    DataColumn dc = new DataColumn(Data[k, 0]);
                    dc.DataType = System.Type.GetType("System.String");
                    dc.ReadOnly = true;
                    dt.Columns.Add(dc);
                }

                for (int i = 1; i < indexRows; i++)
                {
                    DataRow addRow = dt.NewRow();
                    for (int k = 0; k < indexColums; k++)
                    {
                        addRow[Data[k, 0]] = Data[k, i];
                    }
                    dt.Rows.Add(addRow);
                }
                return dt.AsDataView();
            }
            else
            {
                return new DataView();
            }
        }

    }
}
