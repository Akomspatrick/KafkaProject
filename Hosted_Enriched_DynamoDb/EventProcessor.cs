using Models;

namespace Hosted_Enriched_DynamoDb
{
    public class EventProcessor : IEventProcessor
    {
      
      private  List<InterestedParty> _interestedParties = new List<InterestedParty> ();

      
        public EventProcessor(List<InterestedParty> interestedParties)
        {
                     _interestedParties = interestedParties;
        }

        public Task SendToInterestedParties(EnrichedEvent enricheduser)
        {
            foreach (var item in _interestedParties)
            {
                item.Process(enricheduser);
               
            }
            return Task.CompletedTask;
        }
     
    }
}
