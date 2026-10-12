using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelBookingDbContext _context;

        public RoomRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        public async Task<List<Room>> GetAll()
        {
            return await _context.Rooms
                .Include(r => r.RoomImages)
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        // ================= GET BY ID =================
        public async Task<Room?> GetById(int id)
        {
            return await _context.Rooms
                .Include(r => r.RoomImages)
                .Include(r => r.Bookings)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // ================= ADD =================
        public async Task Add(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        // ================= UPDATE =================
        public async Task Update(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        // ================= DELETE =================
        public async Task Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return;

            //// 🔥 xóa ảnh trước
            //var images = await _context.RoomImages
            //    .Where(x => x.RoomId == id)
            //    .ToListAsync();

            //_context.RoomImages.RemoveRange(images);

            _context.Rooms.Remove(room);

            await _context.SaveChangesAsync();
        }

    }
}