using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
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

            //TODO Save How many Drinks have been despense
        }
    }
}
