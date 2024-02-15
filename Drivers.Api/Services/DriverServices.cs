using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Drivers.Api.DriverServices;

public class DriverServices
{
    private readonly IMongoCollection<Drive> _driversCollection;
    public DriverServices(
        IOptions<DatabaseSettings> databaseSettings)
        {
            //Iniciar mi cliente de mongo
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            //Base de datos de mongo conexion
            var mongoDB =
            mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driversCollection =
            mongoDB.GetCollection<Drive>
                (databaseSettings.Value.CollectionName);
        }
        public async Task<List<Drive>> GetAsync() =>
        await _driversCollection.Find(_ => true).ToListAsync();
        public async Task <Drive> GetDriverById(string Id)
        {
            return await _driversCollection.FindAsync(new BsonDocument{{"_Id", new ObjectId(Id)}}).Result.FirstAsync();
        }
        public async Task InsertDriver(Drive driver)
        {
            await _driversCollection.InsertOneAsync(driver);
        }
        //AQUI PARA EDITAR CONTINUAR
       public async Task UpdateDriver(Drive driver)
        {
            var filter = Builders<Drive>.Filter.Eq(s=>s.Id, driver.Id);
            await _driversCollection.ReplaceOneAsync(filter, driver);
        }
        public async Task DeleteDriver(string Id)
        {
            var filter = Builders<Drive>.Filter.Eq(s=>s.Id, Id);
            await _driversCollection.DeleteOneAsync(filter);
        }
        

}