using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{
    public class UnitOfWork
    {
        private string connectionstring;
        private IClientsRepository? clientRepository;

        public UnitOfWork(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }
        public IClientsRepository ClientsRepository
        {
            get { return clientRepository ?? (clientRepository = new ClientsRepository(connectionstring)); }
        }
        public void Reset()
        {
            clientRepository = null;
        }
    }
}
