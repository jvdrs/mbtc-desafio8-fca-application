namespace FcaApplication.Api.Models
{
    public class BadRequestModel
    {
        public string Code { get; private set; }

        public string Message { get; private set; }

        public BadRequestModel(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static BadRequestModel Build(string code, string message) => new BadRequestModel(code, message);
    }
}
