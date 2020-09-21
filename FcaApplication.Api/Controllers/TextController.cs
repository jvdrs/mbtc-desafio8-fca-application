using System;
using System.IO;
using FcaApplication.Api.Helpers;
using FcaApplication.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FcaApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class TextController : Controller
    {     
        private BadRequestModel invalidDataRequest { get; set; } = default(BadRequestModel);
        private NaturalLanguageUnderstandModel naturalLanguageUnderstandModel;

        [HttpPost]
        public ActionResult ProcessTextData([FromForm] TextModel model)
        {
            var carName = model.Car;
            var text = model.Text;

            if (!ProcessText(carName, text))
            {
                return new BadRequestObjectResult(BadRequestHelper.ReturnMessage(invalidDataRequest.Code, invalidDataRequest.Message));
            }

            return new OkObjectResult(naturalLanguageUnderstandModel);
        }

        public bool ProcessText(string carName, string text)
        {
            var result = NaturalLanguageUnderstandHelper.Analyze(text, carName);

            if (result.HasError)
            {
                invalidDataRequest = BadRequestModel.Build(Constants.BadRequestAudioCode, result.Error.Message);
                return false;
            }

            naturalLanguageUnderstandModel = NaturalLanguageUnderstandModel.ToModel(result.Result);

            return true;
        }
    }
}
