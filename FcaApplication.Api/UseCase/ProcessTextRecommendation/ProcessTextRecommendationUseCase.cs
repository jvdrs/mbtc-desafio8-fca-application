using FcaApplication.Api.Domain;
using FcaApplication.Api.Helpers;

namespace FcaApplication.Api.UseCase.ProcessAudioRecommendation
{
    public class ProcessTextRecommendationUseCase
    {
        ServiceResponse<NaturalLanguageUnderstand> nluRecommendation;
        Error error;

        public ServiceResponse<NaturalLanguageUnderstand> Handle(string text, string ratedCar)
        {
            if (!ProcessText(text, ratedCar) )
            {
                return ServiceResponseHelper.WithError<NaturalLanguageUnderstand>(error);
            }

            return nluRecommendation;
        }        

        public bool ProcessText(string text, string ratedCar)
        {
            nluRecommendation = NaturalLanguageUnderstandHelper.Analyze(text, ratedCar);

            if (nluRecommendation.HasError)
            {
                error.Code = Constants.BadRequestAudioCode;
                error.Message = nluRecommendation.Error.Message;

                return false;
            }

            return true;
        }
    }
}
