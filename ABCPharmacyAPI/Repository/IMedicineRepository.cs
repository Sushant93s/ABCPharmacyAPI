using ABCPharmacyAPI.Models;

namespace ABCPharmacyAPI.Repository
{
    public interface IMedicineRepository
    {
        Task <ResponseModel> GetAllAsync();
        Task<ResponseModel> AddMedicineAsync(MedicineModel model);
    }
}
