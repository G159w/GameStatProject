namespace Models.Responses
{
    public class BaseGameResponse
    {
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public bool ApiKeyRequired { get; set; }
    }
}
