using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.dto.response
{
    public class TeamDTO
    {
        public int Id {  get; set; }

        public string teamName {  get; set; }

        public DateOnly creationDate { get; set; }

        public int ownerId { get; set; }


        public static TeamDTO Create(Team team)
        {
            TeamDTO dto = new TeamDTO();
            dto.Id = team.IdTeam;
            dto.teamName = team.TeamName;
            dto.ownerId = team.OwnerId;
            dto.creationDate = team.CreatedDate;
            return dto;
        }
 
    }
}
