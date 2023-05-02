using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class RestDataReader : IDataReader     //RESTful data reader.
{

    public async Task<string> GetData(string url)   //Gets data from url.
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)await request.GetResponseAsync();
            using var streamReader = new StreamReader(response.GetResponseStream());
            string jsonData = streamReader.ReadToEnd();
            return jsonData;
        }
        catch(WebException ex)
        {
            Debug.LogException(ex);
            throw ex;
        }
    }    
}
