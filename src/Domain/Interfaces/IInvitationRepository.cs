using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IInvitationRepository : IBaseRepository<Invitation>
    {
        Invitation? GetByIdWithTeamAndInvitedUser(int invitationId);

        List<Invitation> GetByUserId(int userId);



    }
}
