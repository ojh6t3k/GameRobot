#include <UnityRobot.h>
#include <WheelController.h>
#include <PanTiltController.h>
#include <DigitalModule.h>
#include <ServoModule.h>
#include <Servo.h>
#include <Herkulex.h>

// Herkulex ID
#define WHEEL_FL 1
#define WHEEL_BL 2
#define WHEEL_BR 3
#define WHEEL_FR 4
#define PAN 5
#define TILT 6

WheelController driver(0);
PanTiltController panTilt(1);
DigitalModule led(2, 47);
DigitalModule sound(3, 48);
ServoModule R_gripperModule(4, 49, -90, 90);
ServoModule L_gripperModule(5, 23, -90, 90);

Servo R_gripperServo;
Servo L_gripperServo;

void OnUpdate(byte id)
{
  driver.update(id);
  panTilt.update(id);
  led.update(id);
  sound.update(id);
  R_gripperModule.update(id);
  L_gripperModule.update(id);
}

// When recieved end of update
void OnAction(void)
{
  led.action();
  sound.action();
  
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
  
  if(R_gripperModule.updated() == true)
  {
    R_gripperServo.write(R_gripperModule.getValue(90, 1));
  }
  
  if(L_gripperModule.updated() == true)
  {
    L_gripperServo.write(L_gripperModule.getValue(90, 1));
  }
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  led.reset();
  sound.reset();
  driver.reset();
  panTilt.reset();
  R_gripperModule.reset();
  L_gripperModule.reset();
  Herkulex.moveOneAngle(PAN, 0, 0, LED_PINK);
  Herkulex.moveOneAngle(TILT, 0, 0, LED_PINK);
  R_gripperServo.write(90);
  L_gripperServo.write(90);
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
  led.reset();
  sound.reset();
  
  Herkulex.moveSpeedOne(WHEEL_FL, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_BL, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_BR, 0, 500, LED_PINK);
  Herkulex.moveSpeedOne(WHEEL_FR, 0, 500, LED_PINK);
}

void OnReady(void)
{
  led.flush();
  sound.flush();
}

void setup()
{
  led.begin();
  sound.begin();
  
  R_gripperServo.attach(R_gripperModule.getPin());
  L_gripperServo.attach(L_gripperModule.getPin());
  
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
}

void loop()
{
  UnityRobot.process();
}
