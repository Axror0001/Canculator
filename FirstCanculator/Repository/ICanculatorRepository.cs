using FirstCanculator.Models;

namespace FirstCanculator.Repository
{
    public interface ICanculatorRepository
    {
        Task<CanculatorModels> Create(CanculatorModels canculator);

        Task<List<CanculatorModels>> GetAll();
    }
}
