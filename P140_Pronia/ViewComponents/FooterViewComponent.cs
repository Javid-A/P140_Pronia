﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P140_Pronia.DAL;
using P140_Pronia.Entities;

namespace P140_Pronia.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly ProniaDbContext _context;

        public FooterViewComponent(ProniaDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
    }
}
