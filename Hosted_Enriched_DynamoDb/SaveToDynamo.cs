using Amazon.DynamoDBv2.DataModel;
using Models;

namespace Hosted_Enriched_DynamoDb
{
    public class SaveToDB:ISaveToDB
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public SaveToDB(   IDynamoDBContext dynamoDbContext )
        {
        _dynamoDbContext =dynamoDbContext;
      
    }

        public    Task SaveToDynamo(EnrichedEvent enrichedUser)
        {
             Task p  =  _dynamoDbContext.SaveAsync(enrichedUser);
 
            return Task.CompletedTask;
   

        }
    }
}
