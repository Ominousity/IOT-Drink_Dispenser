#include <WiFi.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>

//Pin Setup
int pump1 = 14;
int pump2 = 13;
int button = 17;
int redLight = 12;
int yellowLight = 4;
int greenLight = 16;

//Despinser Values
double value1;
double value2;

//MultiThreading
TaskHandle_t OfflineDespense;
StaticJsonBuffer<200> jsonBuffer;

//WIFI
WiFiClient espClient;
const char* ssid = "Jj 5G";
const char* pass = "adhf0975";

//Flespi / JSON
PubSubClient client(espClient);
const char* mqtt_server = "mqtt.flespi.io";
long lastMsg = 0;
char msg[50];
int value = 0;

void setup() {
  pinMode(pump1, OUTPUT);
  pinMode(pump2, OUTPUT);
  digitalWrite(pump1, HIGH);
  digitalWrite(pump2, HIGH);
  pinMode(button, INPUT);
  pinMode(redLight, OUTPUT);
  pinMode(yellowLight, OUTPUT);
  pinMode(greenLight, OUTPUT);  
  Serial.begin(9600);
  digitalWrite(redLight, HIGH);
  setupWifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  xTaskCreatePinnedToCore(OfllineDespenseCode, "OfflineDespense", 10000, NULL, 1, &OfflineDespense, 1);
}

void setupWifi(){

  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, pass);

  while (WiFi.status() != WL_CONNECTED) {
    digitalWrite(redLight, HIGH);
    delay(250);
    digitalWrite(redLight, LOW);            
    delay(250);
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
  char messageTemp[255];
  
  for (int i = 0; i < length; i++) {
    Serial.print((char)message[i]);
    messageTemp[i] = (char)message[i];
  }
  Serial.println();
  JsonObject& root = jsonBuffer.parseObject(messageTemp);
  if (!root.success()){
    Serial.println("Failed to pass to Json");
  }
  value1 = root["value1"];
  value2 = root["value2"];
  if (String(topic) == "Bar/Tester") {
    testDrinkDespenser();      
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    digitalWrite(redLight, HIGH);
    // Attempt to connect
    client.setBufferSize(255);
    if (client.connect("mqtt-board-995a3702","po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr", "po9JQvvuiSXMTzHnDhwmtNhRnpC3yXDNbAlPTE70lUC4h9fLeBVvd2cqgFjCsKdr")) {
      Serial.println("connected");
      digitalWrite(redLight, LOW);
      digitalWrite(greenLight, HIGH);      
      // Subscribe
      client.subscribe("Bar/Tester");
      client.setCallback(callback);
      digitalWrite(yellowLight, HIGH);
      delay(250);
      digitalWrite(yellowLight, LOW);
      delay(250);
      digitalWrite(yellowLight, HIGH);
      delay(250);
      digitalWrite(yellowLight, LOW);      
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

void OfllineDespenseCode(void * parameter){
  for(;;){
    if (digitalRead(button)==HIGH){
        testDrinkDespenser();
      }
    }
}

void testDrinkDespenser(){
  digitalWrite(greenLight, LOW);
  Serial.println(value1);
  Serial.println(value2);

  if (value1 <= 0.1){
    for (int i = 0; i <= 7; i++) {
      digitalWrite(yellowLight, HIGH);
      delay(250);
      digitalWrite(yellowLight, LOW);
      digitalWrite(redLight, HIGH);
      delay(250);
      digitalWrite(redLight, LOW);      
    }
  }
  else if(value2 == 0){
    digitalWrite(yellowLight, HIGH);
    Serial.println("on Shot");
    digitalWrite(pump1, LOW);
    delay(value1 * 1000);
    digitalWrite(pump1, HIGH);
    digitalWrite(greenLight, HIGH);
    digitalWrite(yellowLight, LOW);      
  }
  else{
    digitalWrite(yellowLight, HIGH);
    Serial.println("on Mix");
    digitalWrite(pump1, LOW);
    delay(value1 * 1000);
    digitalWrite(pump1, HIGH);
    delay(1000);
    digitalWrite(pump2, LOW);
    delay(value2 * 1000);
    digitalWrite(pump2, HIGH);
    digitalWrite(greenLight, HIGH);
    digitalWrite(yellowLight, LOW);    
  }
}
