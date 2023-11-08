using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IUserRepository
    {
        #region account
        Task<bool> IsUserExistPhoneNumber(string phoneNumber);
        Task CreateUser(User user);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> GetUserById(long userId);
        void UpdateUser(User user);
        Task SaveChange();

        #endregion
        #region admin
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter);

        Task CreateRole(Role role);
        void UpdateRole(Role role);
        Task<List<Permission>> GetAllActivePermission();
        Task<Role> GetRoleById(long id);
        Task RemoveAllPermissionSelectedRole(long roleId);
        Task AddPermissionToRole(List<long> selectedPermission, long roleId);


        Task<List<Role>> GetAllActiveRoles();
        Task RemoveAllUserSelectedRole(long userId);
        Task AddUserToRole(List<long> selectedRole, long userId);
        bool CheckPermission(long permissionId, string phoneNumber);
        Task<bool> IsExistsProductFavorite(long productId, long userId);
        Task<bool> IsExistsProductCompare(long productId, long userId);
        Task AddUserFavorite(UserFavorite userFavorite);
        Task AddUserCompare(UserCompare userCompare);
        Task<List<UserCompare>> GetUserCompares(long userId);
        Task<int> UserFavoriteCount(long userId);
        Task<List<UserFavorite>> GetUserFavorites(long userId);
        void UpdateUserCompare(UserCompare userCompare);
        Task<UserCompare> GetUserCompare(long userId, long productId);
        Task RemoveAllRangeUserCompare(long userId);
        Task RemoveAllRangeUserFavorite(long userId);
        Task RemoveProductInUserCompare(long id);
        Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares);
        Task<UserFavoritesViewModel> UserFavorites(UserFavoritesViewModel userFavorites);
        #endregion

    }
}
