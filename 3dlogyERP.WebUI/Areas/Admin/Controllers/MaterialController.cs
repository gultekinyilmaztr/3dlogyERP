using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using _3dlogyERP.WebUI.Dtos.MaterialDtos;
using _3dlogyERP.WebUI.Dtos.StockCategoryDtos;
using _3dlogyERP.WebUI.Dtos.MaterialTypeDtos;

[Area("Admin")]
[Route("Admin/[controller]")]
public class MaterialController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public MaterialController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
        )
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _baseUrl = _configuration["ApiSettings:BaseUrl"];
    }

    [HttpGet]
    [Route("MalzemeListesi")]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync($"{_baseUrl}/Material");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<MaterialListDto>>(jsonData);
            return View(values);
        }
        return View(new List<MaterialListDto>());
    }

    [HttpGet]
    [Route("MalzemeEkle")]
    public async Task<IActionResult> CreateMaterial()
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            // Malzeme Tiplerini Getir
            var typeResponse = await client.GetAsync($"{_baseUrl}/MaterialType");
            if (typeResponse.IsSuccessStatusCode)
            {
                var typeData = await typeResponse.Content.ReadAsStringAsync();
                var types = JsonConvert.DeserializeObject<List<MaterialTypeListDto>>(typeData);
                ViewBag.MaterialTypes = types.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }

            // Stok Kategorilerini Getir
            var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
            if (stockCategoryResponse.IsSuccessStatusCode)
            {
                var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code.ToString()
                }).ToList();
            }

            return View(new MaterialCreateDto());
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Veriler yüklenirken bir hata oluştu";
            return View(new MaterialCreateDto());
        }
    }

    [HttpPost]
    [Route("MalzemeEkle")]
    public async Task<IActionResult> CreateMaterial(MaterialCreateDto createMaterialDto)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            if (!ModelState.IsValid)
            {
                // Malzeme Tiplerini Getir
                var typeResponse = await client.GetAsync($"{_baseUrl}/MaterialType");
                if (typeResponse.IsSuccessStatusCode)
                {
                    var typeData = await typeResponse.Content.ReadAsStringAsync();
                    var types = JsonConvert.DeserializeObject<List<MaterialTypeListDto>>(typeData);
                    ViewBag.MaterialTypes = types.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                }

                // Stok Kategorilerini Getir
                var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
                if (stockCategoryResponse.IsSuccessStatusCode)
                {
                    var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                    var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                    ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code.ToString()
                    }).ToList();
                }

                return View(createMaterialDto);
            }

            var jsonData = JsonConvert.SerializeObject(createMaterialDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"{_baseUrl}/Material", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Success"] = "Malzeme başarıyla eklendi";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Malzeme eklenirken bir hata oluştu";
            return View(createMaterialDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Beklenmeyen bir hata oluştu";
            return View(createMaterialDto);
        }
    }

    [HttpGet]
    [Route("MalzemeDuzenle/{id}")]
    public async Task<IActionResult> UpdateMaterial(int id)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            // Düzenlenecek malzemeyi getir
            var responseMessage = await client.GetAsync($"{_baseUrl}/Material/{id}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Malzeme bulunamadı";
                return RedirectToAction("Index");
            }

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var material = JsonConvert.DeserializeObject<MaterialUpdateDto>(jsonData);

            // Malzeme tiplerini getir
            var typeResponse = await client.GetAsync($"{_baseUrl}/MaterialType");
            if (typeResponse.IsSuccessStatusCode)
            {
                var typeData = await typeResponse.Content.ReadAsStringAsync();
                var types = JsonConvert.DeserializeObject<List<MaterialTypeListDto>>(typeData);
                ViewBag.MaterialTypes = types.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == material.MaterialTypeId
                }).ToList();
            }

            // Stok kategorilerini getir
            var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
            if (stockCategoryResponse.IsSuccessStatusCode)
            {
                var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code.ToString(),
                    Selected = x.Code == material.StockCategoryCode
                }).ToList();
            }

            return View(material);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Veriler yüklenirken bir hata oluştu";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [Route("MalzemeDuzenle/{id}")]
    public async Task<IActionResult> UpdateMaterial(MaterialUpdateDto updateMaterialDto)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            if (!ModelState.IsValid)
            {
                // Malzeme tiplerini getir
                var typeResponse = await client.GetAsync($"{_baseUrl}/MaterialType");
                if (typeResponse.IsSuccessStatusCode)
                {
                    var typeData = await typeResponse.Content.ReadAsStringAsync();
                    var types = JsonConvert.DeserializeObject<List<MaterialTypeListDto>>(typeData);
                    ViewBag.MaterialTypes = types.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                        Selected = x.Id == updateMaterialDto.MaterialTypeId
                    }).ToList();
                }

                // Stok kategorilerini getir
                var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
                if (stockCategoryResponse.IsSuccessStatusCode)
                {
                    var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                    var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                    ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code.ToString(),
                        Selected = x.Code == updateMaterialDto.StockCategoryCode
                    }).ToList();
                }

                return View(updateMaterialDto);
            }

            var jsonData = JsonConvert.SerializeObject(updateMaterialDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"{_baseUrl}/Material/{updateMaterialDto.Id}", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Success"] = "Malzeme başarıyla güncellendi";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Malzeme güncellenirken bir hata oluştu";
            return View(updateMaterialDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Beklenmeyen bir hata oluştu";
            return View(updateMaterialDto);
        }
    }

    [HttpPost]
    [Route("MalzemeSil/{id}")]
    public async Task<IActionResult> RemoveMaterial(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"{_baseUrl}/Material/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Success"] = "Malzeme başarıyla silindi";
                return Json(new { success = true });
            }

            TempData["Error"] = "Malzeme silinirken bir hata oluştu";
            return Json(new { success = false });
        }
        catch (Exception)
        {
            TempData["Error"] = "Beklenmeyen bir hata oluştu";
            return Json(new { success = false });
        }
    }

    [HttpGet]
    [Route("StokKategori/{categoryId}")]
    public async Task<IActionResult> GetByStockCategory(int categoryId)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var responseMessage = await client.GetAsync($"{_baseUrl}/Material/ByStockCategory/{categoryId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<MaterialListDto>>(jsonData);

                // StockCategories'i ViewBag'e ekle
                var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
                if (stockCategoryResponse.IsSuccessStatusCode)
                {
                    var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                    var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                    ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                        Selected = x.Id == categoryId
                    }).ToList();
                }

                return View("Index", values);
            }

            TempData["Error"] = "Malzemeler yüklenirken bir hata oluştu";
            return View("Index", new List<MaterialListDto>());
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Beklenmeyen bir hata oluştu";
            return View("Index", new List<MaterialListDto>());
        }
    }
}