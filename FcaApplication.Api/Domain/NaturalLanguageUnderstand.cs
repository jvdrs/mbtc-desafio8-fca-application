using FcaApplication.Api.Domain.Enums;
using FcaApplication.Api.Domain.Repositories;
using FcaApplication.Api.Helpers;
using FcaApplication.Api.Repository.GetConsumoByRatedCar;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FcaApplication.Api.Domain
{
    public class NaturalLanguageUnderstand
    {
        public string Recommendation { get; set; }

        public List<RecommendationEntity> Entities { get; set; }

        public NaturalLanguageUnderstand()
        {
            Recommendation = "";
            Entities = new List<RecommendationEntity>();
        }

        public bool MustRetornRecommendation(string sentimentText)
        {
            return !sentimentText.Equals("positive");
        }

        public static NaturalLanguageUnderstand Analyze(string ratedCar, string label, List<EntitiesResult> entities)
        {
            var domain = new NaturalLanguageUnderstand();
            ratedCar = NormalizeRatedCarName(ratedCar);

            var entityToRecommend = CalculateSentimentToRecomend(entities);


            //REGRA 1: Não deve haver recomendação de veículo se o sentimento geral identificado nas entidades reconhecidas pelo NLU for positivo;
            if (MustReturnRecommendationBasedGeneralSentiment(label, entities) &&
                entityToRecommend != EntityType.MODELO
                //|| IsDuplicatedModelEntityType(entities)
                )
            {
                domain.Recommendation = GetSegmentationByType(ratedCar, entityToRecommend);
            }
            
            domain.Entities = GetRecommendationList(entities);

            return domain;
        }

        //TODO: Refactoring.....strategy pattern
        private static string GetSegmentationByType(string ratedCar, EntityType entityToRecommend)
        {
            var repository = new GetDataByEntityRatedCarRepository();
            var recommendedCar = repository.GetOrder(entityToRecommend, ratedCar);

            return recommendedCar;
        }

        private static bool IsDuplicatedModelEntityType(List<EntitiesResult> entities)
        {
            var duplicatedModel = entities.Count(f => f.Type.Equals("modelo", StringComparison.InvariantCultureIgnoreCase));

            return duplicatedModel == entities.Count();
        }

        private static List<RecommendationEntity> GetRecommendationList(List<EntitiesResult> entities)
        {
            return entities
                    .Select(RecommendationEntity.Build).ToList();
        }

        private static EntityType CalculateSentimentToRecomend(List<EntitiesResult> entities)
        {
            var validEntities = entities
                .Where(f => !f.Type.Equals("modelo", StringComparison.InvariantCultureIgnoreCase) &&
                    f.Sentiment.Score < 0);

            if (validEntities.Any())
            {
                var groupedByEntity = validEntities.GroupBy(f => f.Type)
                                .Select(
                                    g => new
                                    {
                                        g.Key,
                                        Value = g.Sum(s => s.Sentiment.Score),
                                    })
                                .OrderBy(o => o.Value)
                                .Select(f => f.Key)
                                .FirstOrDefault();

                return (EntityType)Enum.Parse(typeof(EntityType), groupedByEntity);
            }

            return EntityType.MODELO;
        }

        private static EntitiesResult GetModelFromEntities(List<EntitiesResult> entities)
        {
            return entities.FirstOrDefault(f => f.Type.Equals("modelo", StringComparison.InvariantCultureIgnoreCase));
        }

        //TODO: Aplicar logica mais inteligente. Dictionary
        /// <summary>
        /// Tratamento manual para nome dos carros recomendado. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string NormalizeRatedCarName(string text)
        {
            text = text.RemoveAccents();

            if (text.IndexOf("argo", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "ARGO";
            }

            if (text.IndexOf("toro", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "TORO";
            }

            if (text.IndexOf("ducato", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "DUCATO";
            }

            if (text.IndexOf("fiorino", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "FIORINO";
            }

            if (text.IndexOf("cronos", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "CRONOS";
            }

            if (text.IndexOf("fiat 500", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "FIAT 500";
            }

            if (text.IndexOf("marea", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "MAREA";
            }

            if (text.IndexOf("linea", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "LINEA";
            }            

            if (text.IndexOf("renegade", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return "RENEGADE";
            }

            return "DESCONHECIDO";
        }

        private static bool MustReturnRecommendationBasedGeneralSentiment(string label)
        {
            return !label.Equals("positive", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool MustReturnRecommendationBasedGeneralSentiment(string label, List<EntitiesResult> entities)
        {
            return (!string.IsNullOrWhiteSpace(label) && !label.Equals("positive", StringComparison.InvariantCultureIgnoreCase)) ||
                entities
                    .Where(f => !f.Type.Equals("modelo", StringComparison.InvariantCultureIgnoreCase))
                    .Sum(s => s.Sentiment.Score) < .5;
        }
    }
}
