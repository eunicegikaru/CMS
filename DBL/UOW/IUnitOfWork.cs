using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        IClientsRepository ClientsRepository { get; }
    }
}
