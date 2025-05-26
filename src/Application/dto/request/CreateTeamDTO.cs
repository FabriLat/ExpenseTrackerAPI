using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.dto.request
{
    public class CreateTeamDTO
    {
        [MinLength(1)]
        public string TeamName { get; set; }
        
    }
}
