using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Models;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using System;
using System.Text.Json;

namespace Joints
{
    internal class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //var config = new StreamConfig<StringSerDes, StringSerDes>
            //{
            //    ApplicationId = "test-app",
            //    BootstrapServers = "localhost:9092",
            //};


            var config = new StreamConfig<StringSerDes, StringSerDes>();
            //config.ApplicationId = "test-app";
            //config.BootstrapServers = "pkc-ymrq7.us-east-2.aws.confluent.cloud:9092";
            //config.SaslMechanism = SaslMechanism.Plain;
            //config.SaslUsername = "TYS7GRIFCRWDYTW7";
            //config.SaslPassword = "JTErBeQnf+6NwhMBSF9skk2nW9afUeoE8aDFS10GMyTe/4fYG5qlSYbht0XW+FoV";
            //config.SecurityProtocol = SecurityProtocol.SaslSsl;
            //config.AutoOffsetReset = AutoOffsetReset.Earliest;
            //config.NumStreamThreads = 1;
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



            //IConfiguration config2 = new ConfigurationBuilder().AddIniFile("getting_started.properties").Build()
                ;
    
            StreamBuilder builder = new StreamBuilder();
            string topic1 = "clickevents"; // the new topic that i want to join
            string topic2 = "user-topic";
            string topic3 = "detail-topic";
           

            var stream1 = builder.Stream<string, string>(topic1);
            var stream2 = builder.Stream<string, string>(topic2);
            var stream3 = builder.Stream<string, string>(topic3);
            stream1
            .Join(stream2, user_EventsJoiner(), JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
            .Join(stream3, user_EventsJoiner_Detail(), JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
            .To("full-join");
            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);
            await stream.StartAsync();
        }

        private static Func<string, string, string> user_EventsJoiner()
        {
            return (v1, v2) =>
            {
                ClickEvents? us1 = null;
                User? us2 = JsonSerializer.Deserialize<User>(v1);
                if (v2 != null) us1 = JsonSerializer.Deserialize<ClickEvents>(v2);
                var result = new UserClickEvents { key = us1.key, Type = us1?.Type, Events = us1?.Events, IPAddress = us1.IPAddress, UserAgent = us1.UserAgent, FirstName = us2.FirstName, Lastname= us2.Lastname };
                var s = JsonSerializer.Serialize<UserClickEvents>(result);

                return s;
            };
        }
        private static Func<string, string, string> user_EventsJoiner_Detail()
        {
            return (v1, v2) =>
            {
                UserClickEvents? us1 = JsonSerializer.Deserialize<UserClickEvents>(v1);
                UserDetails? us2 = JsonSerializer.Deserialize<UserDetails>(v2);

                var result = new Enriched { key = us1.key, Type = us1?.Type, Events = us1?.Events, IPAddress = us1.IPAddress, UserAgent = us1.UserAgent, FirstName = us1.FirstName, Lastname = us1.Lastname ,
                       City = getCity(us1.key), Address= us2.Address, Country= us2.Country,Status="ENRICHED", TimeStamp = DateTime.UtcNow };
                var s = JsonSerializer.Serialize<Enriched>(result);
                Console.WriteLine(s);
                return s;

            };
        }
        private static string getCity(string key)
        {
            return key.Contains('1') ? "LONDON" : "BRISTOL";
        }
    }
}
