using Confluent.Kafka;
using Kafka.Public;
using Kafka.Public.Loggers;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net;
using Newtonsoft.Json;
using Models;
using Streamiz.Kafka.Net.Stream;

namespace Hosted_Enriched_DynamoDb
{
    public class ConsumerHostingService : BackgroundService
    {

        // private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IEventProcessor _eventProcessor;// endToInterestedParties_sendToInterestedParties
        //public ConsumerHostingService()
        //{

        //    //_eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
        //}
        public ConsumerHostingService(IEventProcessor eventProcessor)
        {

            _eventProcessor = eventProcessor;// ?? throw new ArgumentNullException(nameof(eventProcessor));
        }



        private EnrichedEvent Enrich(User user)
        {
            return new EnrichedEvent
            {
                FirstName = user.FirstName,
                key = user.key,
                Lastname = user.Lastname,
                Status = ExtractValuesFromAPI(),
                cp= ExtractcpValuesFromAPI(),
                tealium_account = "stepstone",
                tealium_datasource = "9mh7hg",
                tealium_event = "buttonClick",
                tealium_profile = "sandbox",
                tealium_trace_id = "UylMmpGD"

            };
        }

        private string ExtractcpValuesFromAPI()
        {
            var data = new cp { 
                data= Faker.Name.FullName(),
                 sid = Faker.Identification.UkNhsNumber()   ,
                  type = Faker.Company.Name(),
                   version = "1.0",
                
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);

         
        }

        private string ExtractValuesFromAPI() => "ENRICHED";

  


        protected async  Task ExecuteAsyncStreamit(CancellationToken stoppingToken)
        {
            string topic = "user-topic";
            var config = new StreamConfig<StringSerDes, StringSerDes>();

            config.ApplicationId = "test-app";
            config.BootstrapServers = "pkc-n00kk.us-east-1.aws.confluent.cloud:9092";
            config.SaslMechanism = SaslMechanism.Plain;
            config.SaslUsername = "6NC7KNF5YBEMW3JM";
            config.SaslPassword = "tXemvB11XpmP7qnolV8aMW9C8gJKYpwcCaawgYNWx5oeaYJPTJszr8LEMtuWQhLr";
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.AutoOffsetReset = AutoOffsetReset.Earliest;

            config.NumStreamThreads = 1;
            config.SchemaRegistryUrl = "http://localhost:8081";
            //config.BasicAuthUserInfo = "user:password";
            config.BasicAuthCredentialsSource = 0;


            StreamBuilder builder = new StreamBuilder();
            string topic1 = "clickevents"; // the new topic that i want to join
            string topic2 = "user-topic";
            string topic3 = "detail-topic";


            // var stream1 = builder.Stream<string, string>(topic1);
            var stream2 = builder.Stream<string, string>(topic2);
            //var stream3 = builder.Stream<string, string>(topic3);

            stream2.ForeachAsync(
                                ProcessEvent(),
                                RetryPolicy
                                    .NewBuilder()
                                    .NumberOfRetry(10)
                                    .RetryBackOffMs(100)
                                    .RetriableException<Exception>()
                                    .RetryBehavior(EndRetryBehavior.SKIP)
                                    .Build());







            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);
           await  stream.StartAsync();
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "user-consumer-group",
                BootstrapServers = "pkc-n00kk.us-east-1.aws.confluent.cloud:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SaslPassword = "tXemvB11XpmP7qnolV8aMW9C8gJKYpwcCaawgYNWx5oeaYJPTJszr8LEMtuWQhLr",
                SaslUsername = "6NC7KNF5YBEMW3JM",
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl


           // config.NumStreamThreads = 1;
           // config.SchemaRegistryUrl = "http://localhost:8081";
            //config.BasicAuthUserInfo = "user:password";
          //  config.BasicAuthCredentialsSource = 0;
        };
            string topic = "user-topic";
            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);
            CancellationTokenSource token = new();
            try
            {
                while (true)
                {
                    var responce = consumer.Consume(token.Token);
                    if (responce.Message != null)
                    {
                        var user = JsonConvert.DeserializeObject<User>(responce.Message.Value);
                        Console.WriteLine($" Id : {user.key}, " + $"First Name :{user.FirstName}");
                        consumer.Commit(responce);

                        _eventProcessor.SendToInterestedParties(Enrich(user));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static Func<ExternalRecord<string, string>, ExternalContext, Task> ProcessEvent()
        {
            return async (record, _) =>
            {
                Console.WriteLine("Doing some work under ground...");
               
                await  Task.CompletedTask;
                var x = record;
                //await database
                //    .GetCollection<Person>("adress")
                //    .InsertOneAsync(new Person()
                //    {
                //        name = record.Key,
                //        address = new Address()
                //        {
                //            city = record.Value
                //        }
                //    });
            };
        }

       
    }
}
