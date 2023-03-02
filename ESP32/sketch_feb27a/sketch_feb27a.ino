#include <WiFi.h>
#include <Wire.h>
#include <PubSubClient.h>

int pump1 = 16;
int pump2 = 17;
int button = 4;

const char* ssid = "Jj 5G";
const char* pass = "adhf0975";

const char* mqtt_server = "mqtt.flespi.io";

WiFiClient espClient;
PubSubClient client(espClient);
long lastMsg = 0;
char msg[50];
int value = 0;

void setup() {
  pinMode(pump1, OUTPUT);
  pinMode(pump2, OUTPUT);
  pinMode(button, INPUT);
  Serial.begin(9600);

  setupWifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
}

void setupWifi(){

  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, pass);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

void callback(char* topic, byte* message, unsigned int length) {
  Serial.print("Message arrived on topic: ");
  Serial.print(topic);
  Serial.print(". Message: ");
  String messageTemp;
  
  for (int i = 0; i < length; i++) {
    Serial.print((char)message[i]);
    messageTemp += (char)message[i];
  }
  Serial.println();

  // Feel free to add more if statements to control more GPIOs with MQTT

  // If a message is received on the topic esp32/output, you check if the message is either "on" or "off". 
  // Changes the output state according to the message
  if (String(topic) == "Bar/Tester") {
    Serial.print("Changing output to ");
    if(messageTemp == "on"){
      Serial.println("on");
      digitalWrite(pump1, LOW);
      digitalWrite(pump2, LOW);
      delay(100);
      digitalWrite(pump1, HIGH);
      delay(100);
      digitalWrite(pump1, LOW);
      digitalWrite(pump2, HIGH);
      delay(100);
      digitalWrite(pump2, LOW);      
    }
    else if(messageTemp == "off"){
      Serial.println("off");
      digitalWrite(pump1, HIGH);
      digitalWrite(pump2, HIGH);
    }
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    client.setBufferSize(255);
    if (client.connect("mqtt-board-995a3702","po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr", "po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr")) {
      Serial.println("connected");
      // Subscribe
      client.subscribe("Bar/Tester");
      client.setCallback(callback);
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
    
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  long now = millis();
  if (now - lastMsg > 5000) {
    lastMsg = now;
  }

}
