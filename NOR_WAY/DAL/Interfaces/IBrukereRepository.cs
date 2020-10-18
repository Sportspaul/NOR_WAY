using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBrukereRepository
    {
        Task<bool> LoggInn(BrukerModel bruker);

        Task<bool> NyAdmin(BrukerModel bruker);
    }
}