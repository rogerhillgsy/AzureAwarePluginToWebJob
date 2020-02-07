using Newtonsoft.Json;

namespace Arup.AzureRequestListener
{
    public class Response
    {
        public int Status { get; set; } = 200;
        public string Result { get; set; } = "";
        public string Message { get; set; } = "";

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Response FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Response>(json);

        }
    }
}