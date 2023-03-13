using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace Infrastructure
{
    public class DrinkRepository : IDrinkRepository
    {
        private DbContextOptions<DbContext> _options;

        public DrinkRepository()
        {
            _options = new DbContextOptionsBuilder<DbContext>().UseSqlite("Data Source = db.db").Options;
        }

        public async void DespenseDrink(drink drink)
        {
            //TODO Make Flespi Request
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithClientId("APIDRINK")
                    .WithCredentials("po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr", "po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr")
                    .WithTcpServer("mqtt.flespi.io", 1883)
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("Bar/Tester")
                    .WithPayload("{value1:" + drink.AlcCL + ",value2:" + drink.SodaCL + "}")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                await mqttClient.DisconnectAsync();

                Console.WriteLine("Message has been sendt");
            }

            if (FindDrinkInDatabase(drink.Name))
            {
                UpdateDrink(drink);
            }
            else
            {
                CreateDrinkInDatabase(drink);
            }
        }

        #region Database Functions
        private void CreateDrinkInDatabase(drink drink)
        {
            using (var context = new DbContext(_options, ServiceLifetime.Transient))
            {
                _ = context._drinkEntries.Add(drink) ?? throw new ArgumentException("Failed to create drink");
                context.SaveChanges();
            }
        }

        private void UpdateDrink(drink drink_)
        {
            using (var context = new DbContext(_options, ServiceLifetime.Scoped))
            {
                drink drinkToUpdate = context._drinkEntries.Where(x => x.Name == drink_.Name).ToList().FirstOrDefault() ?? throw new KeyNotFoundException("Could not find User");
                drinkToUpdate.Amount++;
                _ = context._drinkEntries.Update(drinkToUpdate) ?? throw new KeyNotFoundException("Could not find User");
                context.SaveChanges();
            }
        }

        private bool FindDrinkInDatabase(string name)
        {
            using (var context = new DbContext(_options, ServiceLifetime.Scoped))
            {
                drink drinkFromDatabase = context._drinkEntries.Where(x => x.Name == name).ToList().FirstOrDefault();

                if (drinkFromDatabase != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
