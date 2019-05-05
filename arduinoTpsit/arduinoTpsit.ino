char input_connection;
long i = 0;
int analogInPin = A0;
int sensorValue = 0;


// put your setup code here, to run once:
void setup() {
  Serial.begin(9600);
  pinMode(analogInPin, INPUT);

  pinMode(13, OUTPUT);

}

// put your main code here, to run repeatedly:
void loop() {
  sensorValue = analogRead(analogInPin);
  Serial.println(sensorValue);
  if(sensorValue > 550) {  // se il sensore del valore è maggiore di 550 il led resta spento
    digitalWrite(13, LOW);  //il LED si spegne
  }
  else {
    digitalWrite(13, HIGH); // se è minore di 550 il led si accende
  }
  delay(1000);
}
