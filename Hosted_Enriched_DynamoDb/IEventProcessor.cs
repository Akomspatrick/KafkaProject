using Models;

namespace Hosted_Enriched_DynamoDb
{
    public interface IEventProcessor
    {
        Task SendToInterestedParties(EnrichedEvent enuser );
    }
}