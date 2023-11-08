using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Utils
{
    public static class PathExtentions
    {
        #region user avatar
        public static string UserAvatarOrgin = "/img/userAvatar/orgin/";
        public static string UserAvatarOrginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/userAvatar/orgin/");


         public static string UserAvatarThumb = "/img/userAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/userAvatar/Thumb/");

        #endregion

        #region product categories
        public static string CategoryOrgin = "/img/category/orgin/";
        public static string CategoryOrginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/category/orgin/");


        public static string CategoryThumb = "/img/category/Thumb/";
        public static string CategoryThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/category/Thumb/");

        #endregion

        #region product 
        public static string ProductOrgin = "/img/Product/orgin/";
        public static string ProductOrginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product/orgin/");


        public static string ProductThumb = "/img/Product/Thumb/";
        public static string ProductThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product/Thumb/");

        #endregion

        #region slider
        public static string SliderOrgin = "/img/Slider/orgin/";
        public static string SliderOrginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Slider/orgin/");


        public static string SliderThumb = "/img/Slider/Thumb/";
        public static string SliderThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Slider/Thumb/");
        #endregion

    }
}
