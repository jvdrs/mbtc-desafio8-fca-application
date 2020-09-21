using FcaApplication.Api.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FcaApplication.Api.Models
{
    public class NaturalLanguageUnderstandModel
    {
        [JsonProperty("recommendation")]
        public string Recommendation { get; set; }

        [JsonProperty("entities")]
        public List<RecommendationEntityModel> Entities { get; set; }

        public NaturalLanguageUnderstandModel()
        {
            Entities = new List<RecommendationEntityModel>();
        }

        public static NaturalLanguageUnderstandModel ToModel(NaturalLanguageUnderstand domain)
        {
            if (domain == null || string.IsNullOrWhiteSpace(domain.Recommendation))
            {
                return new NaturalLanguageUnderstandModel();
            }

            var model = new NaturalLanguageUnderstandModel
            {
                Recommendation = domain.Recommendation,
                Entities = domain.Entities.Select(RecommendationEntityModel.ToModel).ToList()
            };

            return model;
        }
    }
}
