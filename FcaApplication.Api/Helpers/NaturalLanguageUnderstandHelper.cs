using FcaApplication.Api.Domain;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.NaturalLanguageUnderstanding.v1;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using System;
using System.Text;

namespace FcaApplication.Api.Helpers
{
    public static class NaturalLanguageUnderstandHelper
    {
        private static NaturalLanguageUnderstandingService naturalLanguageUnderstanding;        

        static NaturalLanguageUnderstandHelper()
        {
            var authenticator = new IamAuthenticator(
                apikey: Environment.GetEnvironmentVariable("IBM.NLU.APIKEY"));

            naturalLanguageUnderstanding = new NaturalLanguageUnderstandingService("2020-08-01", authenticator);
            naturalLanguageUnderstanding.SetServiceUrl(Environment.GetEnvironmentVariable("IBM.NLU.ENDPOINTURL"));
        }

        public static ServiceResponse<NaturalLanguageUnderstand> Analyze(string text, string ratedCar)
        {
            var result = naturalLanguageUnderstanding.Analyze(
                features: GetFeaturesSettings(),
                text: text,
                language: "pt"
                );

            if (ResultIsInvalid(result))
            {
                return ServiceResponseHelper.WithBusinessErrorMessage<NaturalLanguageUnderstand>("Falha no processamento do nlu");
            }

            if (ResultHasEntities(result))
            {
                return ServiceResponseHelper.WithResult(new NaturalLanguageUnderstand());
            }

            var response = NaturalLanguageUnderstand.Analyze(ratedCar, result.Result?.Sentiment?.Document?.Label, result.Result.Entities);

            return ServiceResponseHelper.WithResult(response);
        }

        private static bool ResultHasEntities(IBM.Cloud.SDK.Core.Http.DetailedResponse<AnalysisResults> result)
        {
            return result.Result.Entities.Count == 0;
        }

        private static bool ResultIsInvalid(IBM.Cloud.SDK.Core.Http.DetailedResponse<AnalysisResults> result)
        {
            return result == null || result.StatusCode == 400 || result.Result == null;
        }

        private static Features GetFeaturesSettings()
        {
            return new Features()
            {
                Entities = new EntitiesOptions()
                {                    
                    Sentiment = true,
                    Mentions = true,
                    Model = Environment.GetEnvironmentVariable("IBM.WKS.MODELID"),
                },
                Sentiment = new SentimentOptions()
                {
                    Document = true                    
                }
            };
        }
    }
}
