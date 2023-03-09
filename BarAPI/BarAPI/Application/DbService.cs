using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class DbService : IDbService
    {
        public IDbRepository _dbRepository;

        public DbService(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public void RecreateDb()
        {
            _dbRepository.RecreateDb();
        }
    }
}
