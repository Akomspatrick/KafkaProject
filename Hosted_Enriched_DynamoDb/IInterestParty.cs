using Amazon.Runtime.Internal.Transform;
using Models;
using System.Net;

namespace Hosted_Enriched_DynamoDb
{
    public interface IInterestParty
    {
        Task Process(EnrichedEvent user);
    }

    public abstract class SendToTealium
    {
        public readonly HttpClient HttpClient = new ();
        public readonly int maxRetry = 5;
        public readonly string url = "https://collect.tealiumiq.com/event";
        public async virtual Task<HttpResponseMessage> postToTealium(EnrichedEvent user)
        {
            var tealiumDataLayer = new Dictionary<string, string>
            {
                {"key",user.key},
                { "FirstName",user.FirstName} ,
                { "Lastname",user.Lastname },
                { "Status",user.Status } ,
                { "tealium_account" ,user.tealium_account } ,
                { "tealium_datasource" ,user.tealium_datasource } ,
                { "tealium_event" ,user.tealium_event } ,
                { "tealium_profile" ,user.tealium_profile } ,
                { "tealium_trace_id" , user.tealium_trace_id},
                { "cp" ,user.cp } ,
                
                
            };
            return await  SendEnrichedData(tealiumDataLayer, HttpClient);
        }


        private async Task<HttpResponseMessage> SendEnrichedData(Dictionary<string, string> tealiumDataLayer, HttpClient httpClient , int retry=0)
        {
            HttpResponseMessage responseMessage = null;
            try
            {   
               
                string postData = Newtonsoft.Json.JsonConvert.SerializeObject(tealiumDataLayer);
                var content = new StringContent(postData);
                do
                {
                     responseMessage = await httpClient.PostAsync(url, content);
                    ++retry;

                } while (retry <maxRetry && responseMessage.StatusCode != HttpStatusCode.OK);
                 
                return responseMessage;
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                return responseMessage;
                //Log Error
            } 
        }
    }
}