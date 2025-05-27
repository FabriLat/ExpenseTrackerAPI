using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IInvitationService
    {
        bool InviteUser(int invitatedId, int ownerId, int teamId);

        bool AcceptInvite(int userId, int invitationId);

        bool DeclineInvitation(int userId, int invitationId);
    }
}
