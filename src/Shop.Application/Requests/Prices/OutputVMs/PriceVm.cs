﻿namespace ShoesShop.Application.Requests.Prices.OutputVMs
{
    public class PriceVm
    {
        public Guid PriceId { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? Sale { get; set; }
        public DateTime? SaleEndDate { get; set; }
    }
}