using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll();
        Task<Room?> GetById(int id);

        Task Add(Room room);
        Task Update(Room room);
        Task Delete(int id);
    }

}