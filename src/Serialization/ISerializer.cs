namespace Serialization;

public interface ISerializer
{
    string Serialize<T>(T data) where T : class;
    T Deserialize<T>(string data) where T : class;
}