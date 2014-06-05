#include <UnityRobot.h>
#include <MsTimer2.h>
#include <DCM.h>
#include <Servo.h> 

#define Uart1_Direction 32  //uart1 use

DCM motor1;
DCM motor2;
DCM motor3;
DCM motor4;
Servo myservo;  
Servo myservo1;  

byte id_Wheel = 1;
short arg_Wheel_left = 0;
short arg_Wheel_right = 0;

byte id_Pan = 2;
word arg_Pan_goal = 90;

byte id_Tilt = 3;
word arg_Tilt_goal = 90;

byte id_IR = 4;
word arg_IR_value = 0;

void OnUpdate(byte id)
{
  if(id == id_Wheel)
  {
    UnityRobot.pop(&arg_Wheel_left);
    UnityRobot.pop(&arg_Wheel_right);
  }
  else if(id == id_Pan)
  {
    UnityRobot.pop(&arg_Pan_goal);
  }
  else if(id == id_Tilt)
  {
    UnityRobot.pop(&arg_Tilt_goal);
  }
}

void OnAction(void)
{
  if(arg_Wheel_left > 0)
  {
    motor1.write(CW,arg_Wheel_left);
    motor3.write(CW,arg_Wheel_left);
  }
  else if(arg_Wheel_left < 0)
  {
    motor1.write(CCW,-arg_Wheel_left);
    motor3.write(CCW,-arg_Wheel_left);
  }
  else
  {
    motor1.write(LOOSE,0);
    motor3.write(LOOSE,0);
  }
  
  if(arg_Wheel_right > 0)
  {
    motor2.write(CW,arg_Wheel_right);
    motor4.write(CW,arg_Wheel_right);
  }
  else if(arg_Wheel_right < 0)
  {
    motor2.write(CCW,-arg_Wheel_right);
    motor4.write(CCW,-arg_Wheel_right);
  }
  else
  {
    motor2.write(LOOSE,0);
    motor4.write(LOOSE,0);
  }
    
  myservo.write(arg_Pan_goal);
  myservo1.write(arg_Tilt_goal); 
}

void OnStart(void)
{
  motor1.write(LOOSE,0);
  motor2.write(LOOSE,0);
  motor3.write(LOOSE,0);
  motor4.write(LOOSE,0);
  myservo.write(90);
  myservo1.write(90);
}

void OnExit(void)
{
  OnStart();
}

void setup()
{
  motor1.attachPins(47,48);  
  motor2.attachPins(49,23);
  motor3.attachPins(24,25);  
  motor4.attachPins(26,50);
    
  myservo.attach(15); 
  myservo1.attach(16); 
  
  OnExit();

  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attachSerial(&Serial1);
  UnityRobot.begin(57600);
  pinMode(Uart1_Direction, OUTPUT);     //uart1 use
  digitalWrite(Uart1_Direction, HIGH);  //uart1 use  
}

void loop()
{
  arg_IR_value = analogRead(A0);
  
  while(UnityRobot.availableInput())
    UnityRobot.processInput(); // process input from host
  
  if(UnityRobot.availableUpdate() == true) // check ready of host
  {
    UnityRobot.startUpdate();
    
    UnityRobot.select(id_IR);
    UnityRobot.push(arg_IR_value);
    UnityRobot.flush();
    
    UnityRobot.endUpdate();
  }  
}
