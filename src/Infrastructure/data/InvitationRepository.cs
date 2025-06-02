using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.data
{
    public class InvitationRepository : BaseRepository<Invitation>, IInvitationRepository
    {
        private readonly GestionGastosContext _context;
        public InvitationRepository(GestionGastosContext context) : base(context)
        {
            _context = context;
        }

        public Invitation? GetByIdWithTeamAndInvitedUser(int invitationId)
        {
            return _context.Invitations
                    .Include(i => i.Team)
                    .Include(i => i.InvitedUser)
                    .FirstOrDefault(i => i.IdInvitation == invitationId);
        }
        
        public List<Invitation> GetByUserId(int userId)
        {
            var invitations = _context.Invitations.Where(i => i.InvitedUserId == userId);
            return invitations.ToList();
        }


    }
}
