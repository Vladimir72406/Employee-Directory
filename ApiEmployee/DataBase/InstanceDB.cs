using ApiEmployee.DataBase.MSSQLRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmployee.DataBase
{
    public static class InstanceDB
    {        
        private static IRepositoryEmployee repositoryEmployee;
        private static IRepositoryContact repositoryContact;

        public static IRepositoryEmployee getInstanceEmployee()
        {
            if (repositoryEmployee == null)
            {
                repositoryEmployee = new RepositoryMSSQLRF();
                //repository = new RepositoryMSSQLCoreEF();
            }

            return repositoryEmployee;
        }

        public static IRepositoryContact getInstanceContact()
        {
            if (repositoryContact == null)
            {
                repositoryContact = new RepositoryContactMSSQLRF();                
            }

            return repositoryContact;
        }

        


    }
}
