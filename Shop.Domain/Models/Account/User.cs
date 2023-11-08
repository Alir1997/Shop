using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.Models.Wallet;

namespace Shop.Domain.Models.Account
{
    public class User: BaseEntity
    {
        [Display(Name ="نام")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage ="{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string LastName { get; set; }

        [Display(Name = "شماره تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string PhoneNumber { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string Password { get; set; }

        [Display(Name = "اواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string Avatar { get; set; }

        [Display(Name = "مسدود شده/نشده")]
        public bool IsBlocked { get; set; }


        [Display(Name = " کد احراز هویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{نمی تواند بیشتر از {1} کاراکتر باشد{0")]
        public string MobileActiveCode { get; set; }

        [Display(Name = "تایید شده / نشده")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "جنسیت")]
        public UserGender UserGender { get; set; }

        #region relations
        public ICollection<UserWallet> UserWallets { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserCompare> UserCompares { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; }
        #endregion
    }
    public enum UserGender
    {
        [Display(Name = "آقا")]
        Male,
        [Display(Name = "خانوم")]
        Femail,
        [Display(Name = "نا مشخص")]
        Unknow
    }
}
