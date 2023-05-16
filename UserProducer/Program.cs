// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;




IConfiguration config = new ConfigurationBuilder().AddIniFile("getting_started.properties").Build();
using var producer = new ProducerBuilder<string, string>(config.AsEnumerable()).Build();

try
{
    // string? state;
    string topic = "user-topic";
    int? Maxcount = 1000;
    // while ((state = Console.ReadLine()) != null)
    for (int i = 0; i < Maxcount; i++)
    {
        // this data shoul be coming from an extenal source
        var user = new User
        {
            key = i.ToString().PadRight(6, 'A'),
            FirstName = Faker.Name.First(),
            Lastname = Faker.Name.Last()

        };

        var response = await producer.ProduceAsync(topic, new Message<string, string> { Key = user.key, Value = JsonConvert.SerializeObject(user) });
        Console.WriteLine(response.Value);
        Console.WriteLine($"INFO : {user.key} {user.FirstName}  {user.Lastname}");
        Thread.Sleep(1000);
    }

}
catch (ProduceException<Null, string> exc)
{

    Console.WriteLine(exc.Message);
}
Console.WriteLine("This is the Producer");
public class User
{
    public string key { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
}