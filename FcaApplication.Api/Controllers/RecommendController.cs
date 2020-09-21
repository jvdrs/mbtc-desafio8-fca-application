using FcaApplication.Api.Domain;
using FcaApplication.Api.Helpers;
using FcaApplication.Api.Models;
using FcaApplication.Api.UseCase.ProcessAudioRecommendation;
using Microsoft.AspNetCore.Mvc;

namespace FcaApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class RecommendController : Controller
    {        
        private BadRequestModel invalidDataRequest { get; set; } = default(BadRequestModel);               

        [HttpPost]
        public ActionResult ProcessAudioFile([FromForm] RecommendModel model)
        {
            var ratedCar = model.Car;
            var audio = model.Audio;
            var text = model.Text;
            var recommendation = default(ServiceResponse<NaturalLanguageUnderstand>);

            if (string.IsNullOrWhiteSpace(ratedCar))
            {
                return new OkObjectResult(new NaturalLanguageUnderstand());
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                var useCase = new ProcessTextRecommendationUseCase();
                recommendation = useCase.Handle(text, ratedCar);
            }
            else if (audio != null)
            {
                var useCase = new ProcessAudioRecommendationUseCase();
                recommendation = useCase.Handle(audio, ratedCar);

            }

            if (recommendation.HasError)
            {
                return new BadRequestObjectResult(BadRequestHelper.ReturnMessage(recommendation.Error.Code, recommendation.Error.Message));
            }

            return new OkObjectResult(recommendation.Result);
        }

    }
}
