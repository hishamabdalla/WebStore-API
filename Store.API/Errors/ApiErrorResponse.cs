namespace Store.API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; } = string.Empty;

        public ApiErrorResponse(int statusCode, string? errorMessage=null)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage ?? GetDefualtMessageForStatusCode(statusCode);
        }

        private string GetDefualtMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "a bad request, you have made",
                401 => "Authorized, you are not ",
                404 => "Resource was not fount",
                500 => "Server Error",
                _ => null
            };

            return message;
        }
    }
}
