using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Director;

namespace BonFireAPI.Models.ResponseDTOs.Director
{
    public class DirectorGetResponse : BaseGetResponse<DirectorResponse>
    {
        public DirectorGetFilterRequest Filter { get; set; }
    }
}
