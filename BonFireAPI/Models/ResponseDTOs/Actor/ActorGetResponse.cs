using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Actor;

namespace BonFireAPI.Models.ResponseDTOs.Actor
{
    public class ActorGetResponse : BaseGetResponse<Common.Entities.Actor>
    {
        public ActorGetFilterRequest Filter { get; set; }
    }
}
