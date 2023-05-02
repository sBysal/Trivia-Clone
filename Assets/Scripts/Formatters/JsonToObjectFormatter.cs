using UnityEngine;

public class JsonToObjectFormatter : IDataFormatter      //Formats json string to any of object type.
{   

    public T FormatData<T>(string jsonData)
    {
        T obj = JsonUtility.FromJson<T>(jsonData);
        return obj; 
    }
}
