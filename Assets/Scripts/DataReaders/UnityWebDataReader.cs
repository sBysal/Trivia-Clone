using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebDataReader : IDataReader      //UnityWeb data reader.
{

    public async Task<string> GetData(string url)   //Gets data from url.
    {
        try
        {
            using var request = UnityWebRequest.Get(url);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }
            var response = request.downloadHandler.text;
            return response;
        }
        catch (WebException ex)
        {
            Debug.LogException(ex);
            throw ex;
        }
    }
}