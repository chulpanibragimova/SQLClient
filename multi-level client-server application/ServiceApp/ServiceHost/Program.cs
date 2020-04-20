using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; 
namespace ServiceHost
{
    class Program
    {
        static void Main()
        {
            
            using (var host = new System.ServiceModel.ServiceHost(typeof(ServiceApp.MSSQLService)))//выделение памяти при запуске
            {
                host.Open();
                Console.WriteLine("Service started....\nInput something to close.");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
