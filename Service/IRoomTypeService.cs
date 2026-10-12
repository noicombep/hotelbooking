using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{

    public interface IRoomTypeService
    {
        Task<List<RoomTypeDTO>> GetAll();
        Task<ApiResponse<string>> Create(string name);
        Task<ApiResponse<string>> Update(int id, string name);
        Task<ApiResponse<string>> Delete(int id);
    }
}
