﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.Helpers;
using P140_Pronia.ViewModels;
using System.Collections;
using System.Linq;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlantsController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantsController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            double plantCount = _context.Plants.Include(p => p.PlantImages).AsEnumerable().Count();
            ViewBag.PageCount = Math.Ceiling(plantCount / 2);
            ViewBag.CurrentPage = page;

            IEnumerable<Plant> model = _context.Plants.Include(p => p.PlantImages).Skip((page-1)*2).Take(2).AsEnumerable();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            PlantCreateVM model = new PlantCreateVM
            {
                Informations = await _context.Informations.ToListAsync(),
                Categories = _context.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlantCreateVM plant)
        {
            PlantCreateVM model = new PlantCreateVM
            {
                Informations = await _context.Informations.ToListAsync(),
                Categories = _context.Categories.ToList()
            };
            if (!ModelState.IsValid) return View(model);
            Plant newPlant = new Plant();

            bool skuResult = _context.Plants.Any(p => p.SKU.ToLower() == plant.SKU.ToLower());
            if (skuResult)
            {
                ModelState.AddModelError("SKU", "SKU code is duplicated");
                return View(model);
            }
            TempData["test"] = "test";

            #region Main and hover photo
            if (!plant.MainPhoto.IsValidLength(1) || !plant.HoverPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Photo size error");
                return View(model);
            }
            string mainPhotoName = await plant.MainPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
            PlantImage mainImage = new PlantImage
            {
                Path = mainPhotoName,
                IsMain = true
            };
            newPlant.PlantImages.Add(mainImage);

            string hoverPhotoName = await plant.HoverPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
            PlantImage hoverImage = new PlantImage
            {
                Path = hoverPhotoName,
                IsMain = null
            };
            newPlant.PlantImages.Add(hoverImage);

            #endregion

            #region Other photos
            foreach (var photo in plant.Photos)
            {
                if (!photo.IsValidLength(1))
                {
                    TempData["Incorrects"] += photo.FileName + ' ';
                    continue;
                }
                PlantImage other = new PlantImage
                {
                    Path = await photo.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images"),
                    Plant = newPlant,
                };
                newPlant.PlantImages.Add(other);
            }

            #endregion

            #region Categories and Informations
            foreach (int id in plant.CategoryIds)
            {
                PlantCategory category = new PlantCategory()
                {
                    CategoryId = id,
                    Plant = newPlant
                };
                newPlant.PlantCategories.Add(category);
            }

            foreach (int id in plant.InformationIds)
            {
                PlantInformation information = new PlantInformation()
                {
                    InformationId = id,
                    Plant = newPlant
                };
                newPlant.PlantInformations.Add(information);
            }
            #endregion



            newPlant.SKU = plant.SKU;
            newPlant.Name = plant.Name;
            newPlant.Price = plant.Price;
            newPlant.Description = plant.Description;

            await _context.Plants.AddAsync(newPlant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            if (id == 0) return BadRequest();

            PlantUpdateVM model = UpdatedPlant(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PlantUpdateVM plant)
        {
            if (id == 0) return BadRequest();
            PlantUpdateVM model = UpdatedPlant(id);
            if (!ModelState.IsValid) return View(model);

            Plant existedPlant = _context.Plants.Include(p => p.PlantCategories)
                                                .Include(p => p.PlantImages)
                                                .FirstOrDefault(p => p.Id == id)!;

            #region Category and Information
            var removableCategories = existedPlant.PlantCategories.Where(p => !plant.CategoryIds.Contains(p.CategoryId)).ToList();

            foreach (var item in removableCategories)
            {
                existedPlant.PlantCategories.Remove(item);
            }

            foreach (var categoryId in plant.CategoryIds)
            {
                if (existedPlant.PlantCategories.Any(p => p.CategoryId != categoryId))
                {
                    PlantCategory category = new PlantCategory
                    {
                        CategoryId = categoryId
                    };
                    existedPlant.PlantCategories.Add(category);
                }
            }

            var removableInformations = existedPlant.PlantInformations.Where(p => !plant.InformationIds.Contains(p.InformationId)).ToList();

            foreach (var item in removableInformations)
            {
                existedPlant.PlantInformations.Remove(item);
            }

            foreach (var informationId in plant.InformationIds)
            {
                if (existedPlant.PlantInformations.Any(p => p.InformationId != informationId))
                {
                    PlantInformation information = new PlantInformation
                    {
                        InformationId = informationId
                    };
                    existedPlant.PlantInformations.Add(information);
                }
            }
            #endregion


            var removableImages = existedPlant.PlantImages.Where(p => !plant.PlantImagesIds.Contains(p.Id)).ToList();
            foreach (var removable in removableImages)
            {
                existedPlant.PlantImages.Remove(removable);
                FileUploadExtension.DeleteImage(removable.Path, _env.WebRootPath, "assets", "images", "website-images");
            }

            if(plant.Photos is not null)
            {
                foreach (var photo in plant.Photos)
                {
                    string fileName = await photo.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
                    PlantImage image = new PlantImage
                    {
                        Path = fileName,
                        IsMain = false
                    };
                    existedPlant.PlantImages.Add(image);
                }
            }


            if(plant.MainPhoto is not null)
            {
                PlantImage existedImage = existedPlant.PlantImages.FirstOrDefault(p=>p.IsMain==true)!;
                string oldName = existedImage.Path;
                string fileName = await plant.MainPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
                existedImage.Path = fileName;

                FileUploadExtension.DeleteImage(oldName, _env.WebRootPath, "assets", "images", "website-images");
            }
            existedPlant.Name = plant.Name;
            existedPlant.Price = plant.Price;
            existedPlant.Description = plant.Description;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        PlantUpdateVM UpdatedPlant(int id)
        {
            PlantUpdateVM model = _context.Plants.Include(p => p.PlantImages)
                                                    .Include(p => p.PlantInformations)
                                                    .Include(p => p.PlantCategories)
                                                    .Select(p => new PlantUpdateVM
                                                    {
                                                        Id = p.Id,
                                                        Name = p.Name,
                                                        Price = p.Price,
                                                        Description = p.Description,
                                                        PlantImages = p.PlantImages.ToList(),
                                                        Categories = _context.Categories.ToList(),
                                                        Informations = _context.Informations.ToList(),
                                                        PlantCategories = p.PlantCategories.ToList(),
                                                        PlantInformations = p.PlantInformations.ToList(),
                                                        PlantImagesIds = p.PlantImages.Select(p => p.Id).ToList()
                                                    }).FirstOrDefault(p => p.Id == id)!;
            return model;
        }
    }
}
