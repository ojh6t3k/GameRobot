#include <UnityRobot.h>
#include "ToneModule.h"

ToneModule toneModule(0);

void OnToneChanged(byte id, word note)
{
  if(id == 0)
  {
    digitalWrite(8, LOW);
    digitalWrite(9, LOW);
    digitalWrite(10, LOW);
    digitalWrite(11, LOW);
    digitalWrite(12, LOW);
    digitalWrite(13, LOW);
    digitalWrite(14, LOW);    
    
    if(note == NOTE_MUTE)
    {      
      noTone(7);
    }
    else
    {      
      if(note == NOTE_C1 || note == NOTE_C2
        || note == NOTE_C3 || note == NOTE_C4
        || note == NOTE_C5 || note == NOTE_C6
        || note == NOTE_C7)
      {
        digitalWrite(8, HIGH);
      }
      else if(note == NOTE_D1 || note == NOTE_D2
        || note == NOTE_D3 || note == NOTE_D4
        || note == NOTE_D5 || note == NOTE_D6
        || note == NOTE_D7)
      {
        digitalWrite(9, HIGH);
      }
      else if(note == NOTE_E1 || note == NOTE_E2
        || note == NOTE_E3 || note == NOTE_E4
        || note == NOTE_E5 || note == NOTE_E6
        || note == NOTE_E7)
      {
        digitalWrite(10, HIGH);
      }
      else if(note == NOTE_F1 || note == NOTE_F2
        || note == NOTE_F3 || note == NOTE_F4
        || note == NOTE_F5 || note == NOTE_F6
        || note == NOTE_F7)
      {
        digitalWrite(11, HIGH);
      }
      else if(note == NOTE_G1 || note == NOTE_G2
        || note == NOTE_G3 || note == NOTE_G4
        || note == NOTE_G5 || note == NOTE_G6
        || note == NOTE_G7)
      {
        digitalWrite(12, HIGH);
      }
      else if(note == NOTE_A1 || note == NOTE_A2
        || note == NOTE_A3 || note == NOTE_A4
        || note == NOTE_A5 || note == NOTE_A6
        || note == NOTE_A7)
      {
        digitalWrite(13, HIGH);
      }
      else if(note == NOTE_B1 || note == NOTE_B2
        || note == NOTE_B3 || note == NOTE_B4
        || note == NOTE_B5 || note == NOTE_B6
        || note == NOTE_B7)
      {
        digitalWrite(14, HIGH);
      }
      
      tone(7, note);
    }
  }  
}

void OnUpdate(byte id)
{
  toneModule.update(id);
}

// When recieved end of update
void OnAction(void)
{
  //TODO: Synchronizing module's action
  toneModule.action();
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  toneModule.reset();
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
  toneModule.reset();
}

void OnReady(void)
{
}

void setup()
{
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(11, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(13, OUTPUT);
  pinMode(14, OUTPUT);
  
  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attach(CMD_READY, OnReady);
  UnityRobot.begin(57600);
  
  toneModule.attach(OnToneChanged);
}

void loop()
{
  UnityRobot.process();
}
