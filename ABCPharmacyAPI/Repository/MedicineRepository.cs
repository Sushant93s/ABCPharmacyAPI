using ABCPharmacyAPI.Models;
using System.Text.Json;

namespace ABCPharmacyAPI.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly string filePath = "Data/medicines.json";

        /// <summary>
        /// Add new Medicine in Medicine data source.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> AddMedicineAsync(MedicineModel model)
        {
            try
            {
                var counter = 0;
                var response = await GetAllAsync();

                var medicines = response.Data as List<MedicineModel> ?? new List<MedicineModel>();

                counter = medicines.Count;

                var checkDuplicate = medicines.Where(o => o.MedicineName == model.MedicineName && o.Brand == model.Brand).ToList();

                if (checkDuplicate.Any())
                {
                    return new ResponseModel
                    {
                        Message = "Duplicate medicine entered.",
                        Data = checkDuplicate,
                        StatusCode = System.Net.HttpStatusCode.Ambiguous
                    };
                }

                model.Id = medicines.Count + 1;
                medicines.Add(model);

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await JsonSerializer.SerializeAsync(stream, medicines);
                }

                counter = medicines.Count;
                response = await GetAllAsync();
                medicines = response.Data as List<MedicineModel> ?? new List<MedicineModel>();
                //if (counter == medicines.Count)
                //{
                //    return 1;
                //}
                //else
                //    return 0;

                return new ResponseModel
                {
                    Message = "Medicine Added ✅",
                    Data = checkDuplicate,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all the Medicines saved in data source
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel> GetAllAsync()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    CreateDirectory(filePath);
                }

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var result = await JsonSerializer.DeserializeAsync<List<MedicineModel>>(stream)
                           ?? new List<MedicineModel>();
                    return new ResponseModel
                    {
                        Data = result,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Message = "Total items: " + result.Count
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create directory and Medicine file data if not exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int CreateDirectory(string path)
        {
            int retVal = 0;
            // 1. Extract the directory path ("Data")
            string directoryPath = Path.GetDirectoryName(path);

            // 2. Create the directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 3. Create the file if it doesn't exist
            if (!File.Exists(filePath))
            {
                // Create the file and immediately close the stream to avoid locking it
                File.Create(filePath).Dispose();

                File.WriteAllText(filePath, "[]");

                retVal = 1;
            }

            return retVal;
        }
    }
}
