using System;


public class TestDataObject
{
  public TestDataObject(Type type, int id, string name)
  {
    Type = type;
    ValidId = id;
    ValidName = name;
  }

  public Type Type { get; private set; }
  public int ValidId { get; private set; }
  public string ValidName { get; private set; }
}

