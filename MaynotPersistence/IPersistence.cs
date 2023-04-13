using System;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public interface IPersistence
    {
        void SaveAsync(String path, MaynotGameState state);
        MaynotGameState LoadAsync(String path);
    }
}
