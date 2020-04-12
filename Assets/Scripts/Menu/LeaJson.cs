using System;
using System.Collections.Generic;

[Serializable]
public class LeaJson
{
    public int Rating;
    public List<User> Users;
}

[Serializable]
public class User
{
    public string Name;
    public long Result;
}