using FcaApplication.Api.Domain.Enums;

namespace FcaApplication.Api.Domain.Repositories
{
    public interface IRecommendRepository
    {
        string GetOrder(EntityType entityType, string ratedCar);
    }
}
