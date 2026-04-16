using ABCPharmacyAPI.Models;

namespace ABCPharmacyAPI.Repository
{
    public interface IMedicineRepository
    {
        Task <List<MedicineModel>> GetAllAsync();
        Task<int> AddMedicineAsync(MedicineModel model);
    }
}
