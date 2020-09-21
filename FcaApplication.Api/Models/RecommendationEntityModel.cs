using FcaApplication.Api.Domain;

namespace FcaApplication.Api.Models
{
    public class RecommendationEntityModel
    {
        public string Entity { get; set; }

        public double Sentiment { get; set; }

        public string Mention { get; set; }

        internal static RecommendationEntityModel ToModel(RecommendationEntity entity)
        {
            return new RecommendationEntityModel
            {
                Entity = entity.Entity,
                Sentiment = entity.Sentiment,
                Mention = entity.Mention,
            };
        }
    }
}
