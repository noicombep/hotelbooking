using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAll();
        Task<RoomType?> GetById(int id);
        Task Add(RoomType type);
        Task Update(RoomType type);
        Task Delete(int id);
    }
}
