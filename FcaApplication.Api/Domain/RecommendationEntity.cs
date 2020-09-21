using System.Linq;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;

namespace FcaApplication.Api.Domain
{
    public class RecommendationEntity
    {
        public string Entity { get; set; }

        public double Sentiment { get; set; }

        public string Mention { get; set; }

        internal static RecommendationEntity Build(EntitiesResult entity)
        {
            return new RecommendationEntity
            {
                Entity = entity.Type,
                Sentiment = entity.Sentiment.Score.GetValueOrDefault(),
                Mention = entity.Mentions.FirstOrDefault().Text
            };
        }
    }
}
