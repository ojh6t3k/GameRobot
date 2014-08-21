#include <UnityRobot.h>

#define MAX_DATANUM 30

class ReportModule
{
  int _id;
  
  byte _num;
  short _data[MAX_DATANUM];
  
  byte _numTemp;
  short _dataTemp[MAX_DATANUM];
  word _interval; // microsec
  
  unsigned long _intervalMicros;
  unsigned long _preMicros;
  
public:
  ReportModule(byte id, word interval);
  
  void update(byte id);
  void action();
  void reset();
  void flush();
  void process(int data);
};

ReportModule reporter(0, 1000);

void OnUpdate(byte id)
{
  reporter.update(id);
}

// When recieved end of update
void OnAction(void)
{
  //TODO: Synchronizing module's action
  reporter.action();
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  reporter.reset();
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
}

void OnReady(void)
{
  reporter.flush();
}

void setup()
{
  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attach(CMD_READY, OnReady);  
  UnityRobot.begin(115200);
}

void loop()
{
  UnityRobot.process();
  reporter.process(analogRead(A0));
}
