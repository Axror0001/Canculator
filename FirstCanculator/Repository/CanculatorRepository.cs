using FirstCanculator.Data;
using FirstCanculator.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstCanculator.Repository
{
    public class CanculatorRepository : ICanculatorRepository
    {
        private readonly AppDbContext _context;
        public CanculatorRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<CanculatorModels> Create(CanculatorModels canculator)
        {
            try
            {
                if (canculator?.Result.HasValue != null && canculator.Action != null)
                {
                    var result = await _context.Canculators.AddAsync(canculator);
                    await _context.SaveChangesAsync();
                    return canculator;
                }
                else
                {
                    Console.WriteLine("Data not saved to database");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<CanculatorModels>> GetAll()
        {
            try
            {
                var result = await _context.Canculators.OrderByDescending(item => item.Id).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("No data found in the database.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
