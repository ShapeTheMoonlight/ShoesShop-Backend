﻿namespace ShoesShop.Application.Requests.Queries.OutputVMs
{
    public class ImageVm
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public bool IsPreview { get; set; }
        public string Url { get; set; }
    }
}