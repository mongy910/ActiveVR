int joyPin1 = 0;                 // slider variable connecetd to analog pin 0 
int joyPin2 = 4;                 // slider variable connecetd to analog pin 1 
int value1 = 0;                  // variable to read the value from the analog pin 0 
int value2 = 0;                  // variable to read the value from the analog pin 1 
void setup() { 
             // initializes digital pins 0 to 7 as outputs 
  Serial.begin(9600); 
} 
 
int treatValue(int data) { 
  return (data * 9 / 1024) ; 
} 
 
void loop() { 
  // reads the value of the variable resistor 
  value1 = analogRead(joyPin1);   
  // this small pause is needed between reading 
  // analog pins, otherwise we get the same value twice 
  delay(100);             
  // reads the value of the variable resistor 
  value2 = analogRead(joyPin2);   
 
  value1 = treatValue(value1); 
value2 = treatValue(value2); 
    
Serial.println(String(value1) + String(value2)); 
delay(100); 
} 

