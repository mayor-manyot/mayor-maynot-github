using System;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public interface IPersistence
    {
        Task SaveAsync(String path, MaynotGameState state);
        Task<MaynotGameState> LoadAsync(String path);
    }
}
