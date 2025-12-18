using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.Actor
{
    public class ActorGetRequest : BaseGetRequest
    {
        public ActorGetFilterRequest? Filter { get; set; }
    }
}
