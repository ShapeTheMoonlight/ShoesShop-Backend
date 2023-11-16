﻿namespace ShoesShop.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public string UserName { get; set; }
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public string Phone { get; set; }

        public Address Address { get; set; }
        public FavoritesList Favorites { get; set; }
        public ICollection<Shopcart> Shopcarts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Rewiews { get; set; }
    }
}
