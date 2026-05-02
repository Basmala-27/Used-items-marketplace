using System;
using System.Collections.Generic;

namespace MarketplaceApp.Models
{
    public class ProfileVM
    {
        // بيانات المستخدم الأساسية
        // استخدمنا ? عشان نشيل الـ Warning ونسمح بقيم null لو الداتا لسه مكملتش
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateTime MemberSince { get; set; }

        // الجزء الخاص بالمنتجات التي رفعها (Items Posted)
        public int ItemsPostedCount { get; set; }
        public IEnumerable<Item>? PostedItems { get; set; } // القائمة الفعليّة للعرض

        // الجزء الخاص بالمفضلات (Favorites)
        public int FavoritesCount { get; set; }
        public IEnumerable<Item>? FavoriteItems { get; set; } // القائمة الفعليّة للعرض

        // إحصائيات إضافية (اختياري حسب التصميم اللي بعتيه)
        public int ActiveOffers { get; set; }
        public int SwapRequests { get; set; }
    }
}