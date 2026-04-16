using ABCPharmacyAPI.Models;
using ABCPharmacyAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABCPharmacyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineController(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllMedicines()
        {
            var data = await _medicineRepository.GetAllAsync();
            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddMedicine(MedicineModel medicine)
        {
            var response = await _medicineRepository.AddMedicineAsync(medicine);
            //return Ok(new { message = "Medicine added successfully" });
            return Ok(response);
        }
    }
}
