using Models;
using System.Net;
using System.Runtime.ConstrainedExecution;

namespace Hosted_Enriched_DynamoDb
{
    public class InterestedParty :  SendToTealium, IInterestParty
    {
        private readonly ISaveToDB? _ISaveToDB;

        public InterestedParty(ISaveToDB? iSaveToDB)
        {
            _ISaveToDB = iSaveToDB;
        }


        public async Task Process(EnrichedEvent user)
        {
            _ISaveToDB.SaveToDynamo(user);
            var response = await this.postToTealium(user);
            user.Status = (response.StatusCode == HttpStatusCode.OK) ? user.Status = "FAILEDTOSENDENRICHED" : user.Status = "SENTENRICHED";
            user.Status = "FAILEDTOSENDENRICHED";
            _ISaveToDB.SaveToDynamo(user);
                
            
            
        }
    }
}
