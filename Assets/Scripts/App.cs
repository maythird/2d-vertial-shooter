using System;
using UnityEngine;

public class Person
{
    public string Name { get; set; }

    public Person(string name)
    {
        this.Name = name;
    }
}


public class App
{
    //생성자
    public App()
    {
        
    }

    public void Test(Action callback)
    {
        callback();
    }
}