﻿using _3dlogyERP.WebUI.Dtos.MaterialDtos;
using _3dlogyERP.WebUI.Dtos.MaterialTypeDtos;
using _3dlogyERP.WebUI.Dtos.StockCategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

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

        // Stok Kategorilerini API'den Getir
        var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
        if (stockCategoryResponse.IsSuccessStatusCode)
        {
            var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
            var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
            ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Code
            }).ToList();
        }

        return View();
    }

    [HttpPost]
    [Route("MalzemeEkle")]
    public async Task<IActionResult> CreateMaterial(MaterialCreateDto createMaterialDto)
    {
        if (!ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();

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

            // Stok Kategorilerini API'den Getir
            var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
            if (stockCategoryResponse.IsSuccessStatusCode)
            {
                var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code
                }).ToList();
            }

            return View(createMaterialDto);
        }

        // Yeni Malzeme Ekleme
        var postClient = _httpClientFactory.CreateClient();
        var jsonData = JsonConvert.SerializeObject(createMaterialDto);
        var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var responseMessage = await postClient.PostAsync($"{_baseUrl}/Material", stringContent);

        if (responseMessage.IsSuccessStatusCode)
        {
            TempData["Success"] = "Malzeme başarıyla eklendi";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Malzeme eklenirken bir hata oluştu";
        return View(createMaterialDto);
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
            return Json(new { success = false });
        }
    }
    [HttpPost]
    [Route("MalzemeDuzenle/{id}")]
    public async Task<IActionResult> UpdateMaterial(MaterialUpdateDto updateMaterialDto)
    {
        if (!ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();

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

            // Stok Kategorilerini API'den Getir
            var stockCategoryResponse = await client.GetAsync($"{_baseUrl}/StockCategory");
            if (stockCategoryResponse.IsSuccessStatusCode)
            {
                var stockCategoryData = await stockCategoryResponse.Content.ReadAsStringAsync();
                var stockCategories = JsonConvert.DeserializeObject<List<StockCategoryListDto>>(stockCategoryData);
                ViewBag.StockCategories = stockCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,  // Kullanıcıya görünen isim
                    Value = x.Code  // Seçildiğinde gönderilecek değer
                }).ToList();
            }

            return View(updateMaterialDto);
        }

        // Güncelleme işlemi
        var putClient = _httpClientFactory.CreateClient();
        var jsonData = JsonConvert.SerializeObject(updateMaterialDto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await putClient.PutAsync($"{_baseUrl}/Material/{updateMaterialDto.Id}", content);

        if (responseMessage.IsSuccessStatusCode)
        {
            TempData["Success"] = "Malzeme başarıyla güncellendi";
            return RedirectToAction("Index");
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        TempData["Error"] = $"Malzeme güncellenirken bir hata oluştu: {responseContent}";
        return View(updateMaterialDto);
    }


    [HttpGet]
    [Route("StokKategori/{categoryCode}")]
    public async Task<IActionResult> GetByStockCategory(string categoryCode)
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync($"{_baseUrl}/Material/by-stock-category/{categoryCode}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<MaterialListDto>>(jsonData);
            return View("Index", values);
        }

        return View("Index", new List<MaterialListDto>());
    }
}