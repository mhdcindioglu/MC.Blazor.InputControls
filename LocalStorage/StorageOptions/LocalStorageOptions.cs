using System.Text.Json;

namespace MC.LocalStorage.StorageOptions
{
    public class LocalStorageOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions();
    }
}
