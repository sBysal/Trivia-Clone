using System.Threading.Tasks;

interface IDataReader    //Data reader interface for derive new data readers.
{
    Task<string> GetData(string url); 
}
