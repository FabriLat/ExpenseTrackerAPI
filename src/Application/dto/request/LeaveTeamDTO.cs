using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.dto.request
{
    public class LeaveTeamDTO
    {
        public int TeamId { get; set; }
        public int? NewOwnerId { get; set; }
    }
}
