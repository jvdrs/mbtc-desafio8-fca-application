using FcaApplication.Api.Domain;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.SpeechToText.v1;
using System;
using System.Linq;
using System.Text;

namespace FcaApplication.Api.Helpers
{
    public static class AudioHelper
    {
        private static SpeechToTextService speechToText;

        static AudioHelper()
        {
            var authenticator = new IamAuthenticator(
                apikey: Environment.GetEnvironmentVariable("IBM.SPEECHTOTEXT.APIKEY"));

            speechToText = new SpeechToTextService(authenticator);
            speechToText.SetServiceUrl(Environment.GetEnvironmentVariable("IBM.SPEECHTOTEXT.ENDPOINTURL"));
        }

        public static ServiceResponse<string> Recognize(string filePath)
        {
            var result = speechToText.Recognize(
                audio: System.IO.File.ReadAllBytes(filePath),
                contentType: Environment.GetEnvironmentVariable("IBM.SPEECHTOTEXT.CONTENTTYPE"),
                model: Environment.GetEnvironmentVariable("IBM.SPEECHTOTEXT.LANGUAGE"));

            if (result.StatusCode == 400 || result.Result == null)
            {
                return ServiceResponseHelper.WithBusinessErrorMessage<string>("Falha no processamento do audio");
            }

            var transcriptionList = new StringBuilder();

            if (result.Result.Results.Any())
            {
                foreach (var data in result.Result.Results.Where(a => a.Final.GetValueOrDefault()).SelectMany(alt => alt.Alternatives))
                {
                    transcriptionList.Append(data.Transcript);
                }
            }

            return ServiceResponseHelper.WithResult(transcriptionList.ToString());
        }
    }
}
