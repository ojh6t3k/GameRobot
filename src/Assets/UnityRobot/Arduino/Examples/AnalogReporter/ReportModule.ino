ReportModule::ReportModule(byte id, word interval)
{
  _id = id;
  _interval = interval;
  reset();
}
  
void ReportModule::update(byte id)
{
  if(id == _id)
  {
    UnityRobot.pop(&_interval);
  }
}
  
void ReportModule::action()
{
  _intervalMicros = (unsigned long)_interval;
  reset();
}

void ReportModule::reset()
{
  _num = 0;
  _preMicros = micros();
}
  
void ReportModule::flush()
{  
  for(int i=0; i<_num; i++)
    _dataTemp[i] = _data[i];
  _numTemp = _num;
  _num = 0;
  
  UnityRobot.select(_id);
  UnityRobot.push(_numTemp);
  for(int i=0; i<_numTemp; i++)
    UnityRobot.push(_dataTemp[i]);
  UnityRobot.flush();
}

void ReportModule::process(int data)
{
  if(_num < MAX_DATANUM)
  {
    unsigned long deltaMicros = micros() - _preMicros;
    if(deltaMicros >= _intervalMicros)
    {
      _data[_num++] = (short)data;
      _preMicros = micros();
    }
  }
}
