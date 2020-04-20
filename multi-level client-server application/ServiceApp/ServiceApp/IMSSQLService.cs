using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceApp
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IMSSQLService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IMSSQLService
    {
        [OperationContract]
        string ConnectToDbAdapter(string login, string password, string server = "./MSSQLSERVER01");
        [OperationContract]
        string InsertionQueryAdapter(string Query);
        [OperationContract]
        string UpdationQueryAdapter(string Query);
        [OperationContract]
        string SelectionQueryAdapter(string Query);
        [OperationContract]
        string DeletionQueryAdapter(string Query);
        [OperationContract]
        Container GetQueryResult();
        [OperationContract]
        string CloseConnection();
    }
}
