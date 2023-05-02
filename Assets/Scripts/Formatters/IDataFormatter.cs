public interface IDataFormatter     //Interface for data formatters.
{ 
    T FormatData<T>(string data);
}