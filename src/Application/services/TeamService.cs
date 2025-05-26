using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.services
{
    public class TeamService : ITeamService
    {

        private readonly ITeamRepository _teamRepository;
        private readonly IUserService _userService;
        
        public TeamService(ITeamRepository teamRepository, IUserService userService)
            {
                _teamRepository = teamRepository;
                _userService = userService;
            }

        public TeamDTO? CreateTeam(CreateTeamDTO createTeamDto, int creatorId)
        {
            if(createTeamDto.TeamName.Trim().Length > 0 && creatorId > 0)
            {
                Team newTeam = new Team();
                newTeam.TeamName = createTeamDto.TeamName.Trim();
                newTeam.CreatedDate = DateOnly.FromDateTime(DateTime.Today);
                newTeam.OwnerId = creatorId;
                _teamRepository.Add(newTeam);
                return TeamDTO.Create(newTeam);
            }
            return null;
        }

        public TeamDTO? GetById(int id)
        {
            Team? team = _teamRepository.GetById(id);
            if(team != null)
            {
                TeamDTO dto = TeamDTO.Create(team);
                return dto;
            }
            return null;
        }

        public bool UpdateTeam(int id, int userId, UpdateTeamDTO team)
        {
            var teamToUpdate = _teamRepository.GetById(id);
            var user = _userService.GetById(userId);

            if (user != null)
            {
                if (teamToUpdate != null && user.Id == teamToUpdate.OwnerId)
                {
                    teamToUpdate.TeamName = team.TeamName;
                    _teamRepository.Update(teamToUpdate);
                    return true;
                }
            }
            return false;
        }

        public bool DeleteTeam(int teamId, int userId)
        {
            Team? teamToDelete = _teamRepository.GetById(teamId);

            if (teamToDelete != null)
            {
                Console.WriteLine("asdasdas");
                if(teamToDelete.OwnerId == userId)
                {
                    _teamRepository.Delete(teamToDelete);
                    return true;
                }
            }
            return false;
        }
    }
}
