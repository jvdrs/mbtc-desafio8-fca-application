using FcaApplication.Api.Models;

namespace FcaApplication.Api.Helpers
{
    public static class BadRequestHelper
    {
        public static BadRequestModel ReturnMessage(string code, string message)
        {
            return new BadRequestModel(code, message);
        }
    }
}
