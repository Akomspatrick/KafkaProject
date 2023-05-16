
using System.Text;

namespace Models
{
    public class User //: ISerDes
    {
        public string key { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }

        //public object DeserializeObject(byte[] data, SerializationContext context)
        //{
        //    var bytesAsString = Encoding.UTF8.GetString(data);
        //    return JsonConvert.DeserializeObject<User>(bytesAsString);
        //}

        //public void Initialize(SerDesContext context)
        //{
        //    context.Config.BootstrapServers = "localhost:9092";
        //    context.Config.ApplicationId = "app-testing";
        //}

        //public byte[] SerializeObject(object data, SerializationContext context)
        //{
        //    var a = JsonConvert.SerializeObject(data);
        //    return Encoding.UTF8.GetBytes(a);
        //}
    }
}
