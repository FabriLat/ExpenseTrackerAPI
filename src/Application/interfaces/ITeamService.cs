using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;

namespace Application.interfaces
{
    public interface ITeamService
    {
        TeamDTO? CreateTeam(CreateTeamDTO createTeamDto, int creatorId);

        bool DeleteTeam(int teamId, int userId);

        TeamDTO? GetById(int id);

        bool UpdateTeam(int id, int userId ,UpdateTeamDTO team);

        bool LeaveTeam(int userId, LeaveTeamDTO leaveTeamDTO);

        bool RemoveUser(int userId, int teamId, int ownerId);
    }
}
