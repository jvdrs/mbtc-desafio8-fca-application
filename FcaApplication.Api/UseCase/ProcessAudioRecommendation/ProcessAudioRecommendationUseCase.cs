using FcaApplication.Api.Domain;
using FcaApplication.Api.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace FcaApplication.Api.UseCase.ProcessAudioRecommendation
{
    public class ProcessAudioRecommendationUseCase
    {
        ServiceResponse<NaturalLanguageUnderstand> nluRecommendation;
        Error error;
        private static string supportedMimeTypes = "audio/x-flac, audio/flac";
        private string filePath;
        private string transcriptText;

        public ServiceResponse<NaturalLanguageUnderstand> Handle(IFormFile audio, string ratedCAr)
        {
            if (!FileIsValid(audio) ||
                !FileHasValidSize(audio.Length) ||
                !FileHasValidMimeType(audio.ContentType) ||
                !CreateImageSuccessully(audio) ||
                !Transcript() ||
                !ProcessText(ratedCAr))
            {
                return ServiceResponseHelper.WithError<NaturalLanguageUnderstand>(error);
            }

            return nluRecommendation;
        }


        private bool FileIsValid(IFormFile audio)
        {
            if (audio == null)
            {
                error.Code = Constants.BadRequestCode;
                error.Message = "Arquivo nao informado.";

                return false;
            }

            return true;

        }

        public bool FileHasValidSize(long size)
        {
            if (size <= 0)
            {
                error.Code = Constants.BadRequestCode;
                error.Message = "Arquivo de audio vazio.";
                
                return false;
            }

            return true;
        }

        public bool FileHasValidMimeType(string contentType)
        {
            if (supportedMimeTypes.IndexOf(contentType, StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                error.Code = Constants.BadRequestCode;
                error.Message = $"Tipo de arquivo invalido: {contentType}";

                return false;
            }

            return true;
        }

        public bool CreateImageSuccessully(IFormFile file)
        {
            try
            {
                filePath = $@"{Environment.CurrentDirectory}\uploads\{file.FileName}";

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                error.Code = Constants.ExceptionCode;
                error.Message = ex.ToString();

                return false;
            }
        }

        public bool Transcript()
        {
            var result = AudioHelper.Recognize(filePath);

            if (result.HasError)
            {
                error.Code = Constants.BadRequestAudioCode;
                error.Message = result.Error.Message;

                return false;
            }

            transcriptText = result.Result;

            return true;
        }

        public bool ProcessText(string carName)
        {
            nluRecommendation = NaturalLanguageUnderstandHelper.Analyze(transcriptText, carName);

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
