using Models;

namespace Hosted_Enriched_DynamoDb
{
    public interface ISaveToDB
    {
     public   Task SaveToDynamo(EnrichedEvent enrichedUser);
    }
}