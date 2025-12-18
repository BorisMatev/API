using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.Director
{
    public class DirectorGetRequest : BaseGetRequest
    {
        public DirectorGetFilterRequest? Filter { get; set; }
    }
}
