using System;
using System.Collections.Generic;

namespace MarketplaceApp.Models
{
    public class ProfileVM
    {
        
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateTime MemberSince { get; set; }

      
        public int ItemsPostedCount { get; set; }
        public IEnumerable<Item>? PostedItems { get; set; } // القائمة الفعليّة للعرض

       
        public int FavoritesCount { get; set; }
        public IEnumerable<Item>? FavoriteItems { get; set; } // القائمة الفعليّة للعرض

       
        public int ActiveOffers { get; set; }
        public int SwapRequests { get; set; }
    }
}