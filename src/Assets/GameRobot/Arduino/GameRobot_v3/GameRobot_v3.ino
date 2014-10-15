#include <UnityRobot.h>
#include <WheelController.h>
#include <PanTiltController.h>
#include <Servo.h>

// pin
#define PWM_R 18
#define DIR1_R 10
#define DIR2_R 11
#define PWM_L 16
#define DIR1_L 8
#define DIR2_L 9
#define PAN 22
#define TILT 23

WheelController driver(0);
PanTiltController panTilt(1);

Servo pan;
Servo tilt;

void OnUpdate(byte id)
{
  driver.update(id);
  panTilt.update(id);
}

// When recieved end of update
void OnAction(void)
{
  //TODO: Synchronizing module's action
  if(driver.updated() == true)
  {
    int speed = (int)driver.rSpeed(255, 1);
    if(speed > 0)
    {
      digitalWrite(DIR1_R, LOW);
      digitalWrite(DIR2_R, HIGH);
      analogWrite(PWM_R, speed);
    }
    else if(speed < 0)
    {
      digitalWrite(DIR1_R, HIGH);
      digitalWrite(DIR2_R, LOW);
      analogWrite(PWM_R, -speed);      
    }
    else
    {
      digitalWrite(DIR1_R, HIGH);
      digitalWrite(DIR2_R, HIGH);
      analogWrite(PWM_R, 0);
    }
    speed = (int)driver.lSpeed(255, 1);
    if(speed > 0)
    {
      digitalWrite(DIR1_L, LOW);
      digitalWrite(DIR2_L, HIGH);
      analogWrite(PWM_L, speed);
    }
    else if(speed < 0)
    {
      digitalWrite(DIR1_L, HIGH);
      digitalWrite(DIR2_L, LOW);
      analogWrite(PWM_L, -speed);
    }
    else
    {
      digitalWrite(DIR1_L, HIGH);
      digitalWrite(DIR2_L, HIGH);
      analogWrite(PWM_L, 0);
    }
  }
  
  if(panTilt.updated() == true)
  {
    pan.write(90 + (int)panTilt.panAngle(180, -1));
    tilt.write(90 + (int)panTilt.tiltAngle(180, -1));   
  }
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  driver.reset();
  panTilt.reset();
  pan.write(90);
  tilt.write(90);
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
  pan.write(90);
  tilt.write(90);
  
  digitalWrite(DIR1_R, HIGH);
  digitalWrite(DIR2_R, HIGH);
  analogWrite(PWM_R, 0);
  digitalWrite(DIR1_L, HIGH);
  digitalWrite(DIR2_L, HIGH);
  analogWrite(PWM_L, 0);
}

void OnReady(void)
{
}

void setup()
{
  pan.attach(PAN);
  tilt.attach(TILT);
  
  pinMode(PWM_R, OUTPUT);
  pinMode(DIR1_R, OUTPUT);
  pinMode(DIR2_R, OUTPUT);
  pinMode(PWM_L, OUTPUT);
  pinMode(DIR1_L, OUTPUT);
  pinMode(DIR2_L, OUTPUT);
  
  OnExit();
  
  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attach(CMD_READY, OnReady);
  UnityRobot.begin(57600);
}

void loop()
{
  UnityRobot.process();
}
