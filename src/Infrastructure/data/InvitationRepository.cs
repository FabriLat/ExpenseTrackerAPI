using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.data
{
    public class InvitationRepository : BaseRepository<Invitation>, IInvitationRepository
    {
        private readonly GestionGastosContext _context;
        public InvitationRepository(GestionGastosContext context) : base(context)
        {
            _context = context;
        }
    }
}
