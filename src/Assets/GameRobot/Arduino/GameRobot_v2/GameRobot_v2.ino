#include <UnityRobot.h>
#include <WheelController.h>
#include <PanTiltController.h>
#include <Herkulex.h>
#include <ADCModule.h>

// define
#define RADAR_SPEED 1000 //msec

// Herkulex ID
#define WHEEL_FL 1
#define WHEEL_BL 2
#define WHEEL_BR 3
#define WHEEL_FR 4
#define PAN 5
#define TILT 6
#define RADAR 7

// Module
WheelController driver(0);
PanTiltController panTilt(1);
ADCModule adcForward(2, A0);
ADCModule adcRight(3, A1);
ADCModule adcLeft(4, A2);
ADCModule adcBackward(5, A3);
byte angleModule_id = 6;
short angleModule_value;
boolean angleModule_run;
byte imuModule_id = 7;
short imuModule_roll;
short imuModule_pitch;
boolean imuModule_increase;


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
    Herkulex.moveSpeedOne(WHEEL_FL, (int)driver.lSpeed(2048, 1), 500, LED_PINK);
    Herkulex.moveSpeedOne(WHEEL_BL, (int)driver.lSpeed(2048, 1), 500, LED_PINK);
    Herkulex.moveSpeedOne(WHEEL_BR, (int)driver.rSpeed(2048, 1), 500, LED_PINK);
    Herkulex.moveSpeedOne(WHEEL_FR, (int)driver.rSpeed(2048, 1), 500, LED_PINK);
  }
  
  if(panTilt.updated() == true)
  {
    Herkulex.moveOneAngle(PAN, (int)panTilt.panAngle(180, 1), 0, LED_PINK);
    Herkulex.moveOneAngle(TILT, (int)panTilt.tiltAngle(180, -1), 0, LED_PINK);
  }
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  driver.reset();
  panTilt.reset();
  Herkulex.moveOneAngle(PAN, 0, 0, LED_PINK);
  Herkulex.moveOneAngle(TILT, 0, 0, LED_PINK);
  
  angleModule_run = true;
  Herkulex.moveOneAngle(RADAR, 50, 0, 0);
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
  Herkulex.moveSpeedOne(WHEEL_FL, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_BL, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_BR, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_FR, 0, 500, LED_PINK);
  
  angleModule_run = false;
}

void OnReady(void)
{
  adcForward.flush();
  adcRight.flush();
  adcLeft.flush();
  adcBackward.flush();
  
  UnityRobot.select(angleModule_id);
  UnityRobot.push(angleModule_value);
  UnityRobot.flush();
  
  UnityRobot.select(imuModule_id);
  UnityRobot.push(imuModule_roll);
  UnityRobot.push(imuModule_pitch);
  UnityRobot.flush();
}

void setup()
{
  Herkulex.beginSerial1(115200); //open serial
  Herkulex.reboot(WHEEL_FL);
  Herkulex.reboot(WHEEL_BL);
  Herkulex.reboot(WHEEL_BR);
  Herkulex.reboot(WHEEL_FR);
  Herkulex.reboot(PAN);
  Herkulex.reboot(TILT);
  delay(500);
  Herkulex.initialize(); //initialize motors
  delay(200);
  
  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attach(CMD_READY, OnReady);
  UnityRobot.begin(57600);
  
  angleModule_run = true;
  imuModule_increase = true;
  imuModule_roll = 0;
  imuModule_pitch = 0;
}

void loop()
{
  UnityRobot.process();
  
  if(angleModule_run == true)
  {
    float angle = Herkulex.getAngle(RADAR);
    angleModule_value = (short)(angle * 10);
    if(angle > 45)
       Herkulex.moveOneAngle(RADAR, -50, RADAR_SPEED, 0);
    else if(angle < -45)
       Herkulex.moveOneAngle(RADAR, 50, RADAR_SPEED, 0);
  }
  
  if(imuModule_increase == true)
  {
    imuModule_roll++;
    imuModule_pitch++;
    
    if(imuModule_roll > 300)
      imuModule_increase = false;
  }
  else
  {
    imuModule_roll--;
    imuModule_pitch--;
    
    if(imuModule_roll < -300)
      imuModule_increase = true;
  }
}
