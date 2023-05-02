using System;
using System.Collections.Generic;

[Serializable]
public class Question 
{
    public string category;
    public string question;
    public List<string> choices;
    public string answer;    
}
