using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Actor;

namespace BonFireAPI.Models.ResponseDTOs.Actor
{
    public class ActorGetResponse : BaseGetResponse<ActorResponse>
    {
        public ActorGetFilterRequest Filter { get; set; }
    }
}
