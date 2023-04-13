using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public class FilePersistence : IPersistence
    {
        public void SaveAsync(String path, MaynotGameState state)
        {
            try
            {
                using StreamWriter writer = new(path);
                String jsonString = JsonSerializer.Serialize(state);
                Console.WriteLine(jsonString);
                writer.Write(jsonString);
            }
            catch
            {
                throw new DataException();
            }
        }

        public MaynotGameState LoadAsync(String path)
        {
            try
            {
                using StreamReader reader = new(path);
                string jsonString = File.ReadAllText(path);
                Console.WriteLine(jsonString);
                MaynotGameState state = JsonSerializer.Deserialize<MaynotGameState>(jsonString)!;

                return state;
            }
            catch
            {
                throw new DataException();
            }
        }
    }
}
