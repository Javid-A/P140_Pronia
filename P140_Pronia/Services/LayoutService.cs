﻿using Newtonsoft.Json;
using P140_Pronia.ViewModels;

namespace P140_Pronia.Services
{
    public class LayoutService
    {
        private readonly IHttpContextAccessor _http;

        public LayoutService(IHttpContextAccessor http)
        {
            _http = http;
            
        }
        public BasketItem GetBasket()
        {
            string basketStr = _http.HttpContext.Request.Cookies["basket"] ?? "";

            BasketItem basket = JsonConvert.DeserializeObject<BasketItem>(basketStr)!;
            basket.Count =  basket.CookieItems.Sum(ci => ci.Quantity);
            basket.TotalPrice = basket.CookieItems.Sum(ci => ci.Price);
            return basket ?? new BasketItem();
        }
    }
}
