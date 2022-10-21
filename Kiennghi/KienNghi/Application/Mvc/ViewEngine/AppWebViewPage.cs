using System.Web;
using System.Web.Mvc;

namespace  KienNghi.Mvc
{
    /// <summary>
    /// Base class for all views in Abp system.
    /// </summary>
    /// <typeparam name="TModel">Type of the View Model</typeparam>
    public abstract class AppWebViewPage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Gets the root path of the application.
        /// </summary>
        public string ApplicationPath
        {
            get
            {
                var appPath = HttpContext.Current.Request.ApplicationPath;
                if (appPath == null)
                {
                    return "/";
                }
                //appPath = appPath.EnsureEndsWith('/');

                return appPath;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AppWebViewPage()
        {
            //using (_unitOfWorkManager.NewUnitOfWork())
            //{
            //    LoggedOnReadOnlyUser = UserIsAuthenticated ? _membershipService.GetUser(Username, true) : null;
            //    UsersRole = LoggedOnReadOnlyUser == null ? _roleService.GetRole(AppConstants.GuestRoleName, true) : LoggedOnReadOnlyUser.Roles.FirstOrDefault();
            //    CurrentSetting = _settingService.GetSettings(true);
            //}
        }


        protected bool UserIsAuthenticated => System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

        //protected bool UserIsAdmin => User.IsInRole(AppConstants.AdminRoleName);

        protected string Username => UserIsAuthenticated ? System.Web.HttpContext.Current.User.Identity.Name : null;

    }

}

