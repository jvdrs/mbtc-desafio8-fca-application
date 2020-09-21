using FcaApplication.Api.Domain;
using FcaApplication.Api.Models;
using FcaApplication.Api.UseCase.ProcessAudioRecommendation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace FcaApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class RecommendController : Controller
    {        
        private BadRequestModel invalidDataRequest { get; set; } = default(BadRequestModel);
        private readonly ILogger _logger;

        public RecommendController(ILogger<RecommendController> logger)
        {
            _logger = logger;

        }

        [HttpPost]
        public ActionResult ProcessAudioFile([FromForm] RecommendModel model)
        {
            var ratedCar = model.Car;
            var audio = model.Audio;
            var text = model.Text;
            var recommendation = default(ServiceResponse<NaturalLanguageUnderstand>);
            var now = DateTime.UtcNow;
            var transactionId = Guid.NewGuid().ToString();

            _logger.LogInformation($"[{now}] [{transactionId}] - {nameof(ProcessAudioFile)}: RatedCar: {ratedCar}");

            if (string.IsNullOrWhiteSpace(ratedCar))
            {
                return new OkObjectResult(new NaturalLanguageUnderstand());
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] -  StartTextProcess: Text: {text}");

                var useCase = new ProcessTextRecommendationUseCase();
                recommendation = useCase.Handle(text, ratedCar);

                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] -  EndTextProcess");
            }
            else if (audio != null)
            {
                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] -  StartAudioProcess: Text: {audio.FileName}, MimeType: {audio.ContentType}, Size: {audio.Length}");

                if (!audio.ContentType.ToLower().Contains("flac"))
                {
                    _logger.LogError($"[{DateTime.UtcNow}] [{transactionId}] -  InvalidMimeType: {audio.ContentType}");
                    return new OkObjectResult(new NaturalLanguageUnderstand());
                }

                var useCase = new ProcessAudioRecommendationUseCase();
                recommendation = useCase.Handle(audio, ratedCar);

                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] -  EndAudioProcess");
            }
            else
            {
                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] -  InvalidProcess");

                return new OkObjectResult(new NaturalLanguageUnderstand());
            }


            if (recommendation.HasError)
            {
                _logger.LogError($"[{DateTime.UtcNow}] [{transactionId}] -  ErrorProcess {recommendation.Error.Code} - {recommendation.Error.Message} ");

                return new OkObjectResult(new NaturalLanguageUnderstand());
                //return new BadRequestObjectResult(BadRequestHelper.ReturnMessage(recommendation.Error?.Code, recommendation.Error?.Message));
            }

            try
            {
                var jsonText = JsonConvert.SerializeObject(recommendation.Result);
                
                _logger.LogInformation($"[{DateTime.UtcNow}] [{transactionId}] - EndGeneralProcessing {jsonText}");
            }
            catch (Exception)
            {
                _logger.LogError($"[{DateTime.UtcNow}] [{transactionId}] - EndGeneralProcessing {recommendation.Result?.Recommendation}");
            }
            

            return new OkObjectResult(recommendation.Result);
        }

    }
}
