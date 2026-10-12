using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repository;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly HotelBookingDbContext _context;

    public RoomTypeRepository(HotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomType>> GetAll()
    {
        return await _context.RoomTypes
            .Include(r => r.Rooms) // 🔥 để count
            .ToListAsync();
    }

    public async Task<RoomType?> GetById(int id)
    {
        return await _context.RoomTypes
            .Include(r => r.Rooms)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task Add(RoomType type)
    {
        _context.RoomTypes.Add(type);
        await _context.SaveChangesAsync();
    }

    public async Task Update(RoomType type)
    {
        _context.RoomTypes.Update(type);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var type = await _context.RoomTypes.FindAsync(id);
        if (type != null)
        {
            _context.RoomTypes.Remove(type);
            await _context.SaveChangesAsync();
        }
    }
}