using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.data
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {

        private readonly GestionGastosContext _context;
        public TeamRepository(GestionGastosContext context) : base(context)
        {
            _context = context;
        }

        public Team? GetByName(string teamName)
        {
            throw new NotImplementedException();
        }

        public Team? GetByOwnerId(int ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
