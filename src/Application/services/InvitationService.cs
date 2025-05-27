using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.services
{
    public class InvitationService : IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;
        private readonly ITeamService _teamService;

        public InvitationService(IInvitationRepository invitationRepository, ITeamService teamService)
        {
            _invitationRepository = invitationRepository;
            _teamService = teamService;
        }

        public bool AcceptInvite(int userId, int invitationId)
        {
            throw new NotImplementedException();
        }

        public bool DeclineInvitation(int userId, int invitationId)
        {
            throw new NotImplementedException();
        }

        public bool InviteUser(int invitatedId, int ownerId, int teamId)
        {
            var team = _teamService.GetById(teamId);

            if(team.ownerId == ownerId && invitatedId != ownerId)
            {
                Invitation newInvitation = new Invitation();
                newInvitation.InvitedUserId = invitatedId;
                newInvitation.OwnerUserId = ownerId;
                newInvitation.TeamId = teamId;
                _invitationRepository.Add(newInvitation);
                return true;
            }
            return false;
        }
    }
}
