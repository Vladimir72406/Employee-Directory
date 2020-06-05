using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmployee.DataBase
{
    public static class InstanceDB
    {        
        private static IRepository repository;

        public static IRepository getInstance()
        {
            if (repository == null)
            {
                repository = new RepositoryMSSQLRF();
                //repository = new RepositoryMSSQLCoreEF();
            }

            return repository;
        }

        


    }
}
