using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Hosted_Enriched_DynamoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string access_key = "";
string secret_key = "";


var credentials = new BasicAWSCredentials(access_key, secret_key);
var config = new AmazonDynamoDBConfig
{
    RegionEndpoint = Amazon.RegionEndpoint.EUWest2,

};
var client = new AmazonDynamoDBClient(credentials, config);
 List<InterestedParty> _interestedParties = new List<InterestedParty>();
var  yy = new SaveToDB(new DynamoDBContext(client));
_interestedParties.Add(new InterestedParty(yy)); 

//builder.Services.AddSingleton<ISaveToDB, SaveToDB>();

//builder.Services.AddSingleton<IAmazonDynamoDB>(client);
//builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddSingleton<IEventProcessor>( new EventProcessor(_interestedParties));
builder.Services.AddHostedService<ConsumerHostingService > ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
