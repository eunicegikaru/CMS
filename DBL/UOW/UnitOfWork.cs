using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{

        public class UnitOfWork : IUnitOfWork
        {
            private readonly IClientsRepository _clientsRepository;

            public UnitOfWork(string connectionstring)
            {
                if (string.IsNullOrWhiteSpace(connectionstring))
                    throw new ArgumentNullException(nameof(connectionstring));

                _clientsRepository = new ClientsRepository(connectionstring);
            }

            public IClientsRepository ClientsRepository => _clientsRepository;
        }
    
}
