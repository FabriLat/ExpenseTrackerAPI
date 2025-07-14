using Application.interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.services
{
    public class InvitationService : IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public InvitationService(IInvitationRepository invitationRepository, ITeamService teamRepository, IUserService userRepository)
        {
            _invitationRepository = invitationRepository;
            _teamService = teamRepository;
            _userService = userRepository;
        }

        public bool InviteUser(int invitatedId, int ownerId, int teamId)
        {
            Team? team = _teamService.GetTeamByIdCompleteData(teamId);
            if (team == null)
            { return false; }

            if(team.Users.Count > 5)
            {
                return false;
            }

            User? user = _userService.GetByIdCompleteData(invitatedId);
            if(user == null)
            { return false; }


            List<Invitation> userInvitations = _invitationRepository.GetByUserId(invitatedId);
            int alreadyInvited = userInvitations.Where(u => u.TeamId == teamId && u.State == 0).Count();

            if (alreadyInvited > 0)
            {
                return false;
            }

            if(team.OwnerId == ownerId && invitatedId != ownerId)
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

        public List<Invitation> GetByUserId(int userId)
        {
            var invitations = _invitationRepository.GetByUserId(userId);
            List<Invitation> pendingInvitations = invitations.Where(i => i.State == 0).ToList();
            return pendingInvitations;
        }


        public bool AcceptInvite(int userId, int invitationId)
        {
            Invitation? invitation = _invitationRepository.GetByIdWithTeamAndInvitedUser(invitationId);
            if (invitation == null)
                return false;

            User user = invitation.InvitedUser;

            if (invitation != null && user != null)
            {
                var team = invitation.Team;
                if (invitation.InvitedUserId == userId && invitation.State == 0 && team.Users.Count() < 5)
                {
                    invitation.State = 1;
                    invitation.Team.Users.Add(user);
                    _invitationRepository.Update(invitation);
                    return true;
                }
            }
            return false;
        }

        public bool RejectInvitation(int userId, int invitationId)
        {
            Invitation? invitation = _invitationRepository.GetById(invitationId);
            
            if( invitation != null && invitation.InvitedUserId == userId)
            {
                invitation.State = 2;
                _invitationRepository.Update(invitation);
                return true;
            }
            return false;

        }
    }
}
