using System;
using System.Collections.Generic;

[Serializable]
public class Question   //Question Model
{
    public string category;
    public string question;
    public List<string> choices;
    public string answer;    
}
