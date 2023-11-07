using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Controllers
{
  public partial class SettingsController : BaseAdminController
    {
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IPermissionService _permissionService;
        public  SettingsController(ISettingService settingService,IStoreContext storeContext,ILocalizationService localizationService,IPermissionService permissionService)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _permissionService = permissionService;

        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var simpleLMSSettings = await _settingService.LoadSettingAsync<SimpleLMSSettings>(storeScope);
            var model = new ConfigurationModel
            {
                //YouTubeApiKey = simpleLMSSettings.YouTubeApiKey,
                //VimeoAppId=simpleLMSSettings.VimeoAppId,
                VimeoClient=simpleLMSSettings.VimeoClient,
                VimeoAccess=simpleLMSSettings.VimeoAccess,
                VimeoSecret=simpleLMSSettings.VimeoSecret,
                //VdoCipherKey=simpleLMSSettings.VdoCipherKey
            };
            return View("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Settings/Configure.cshtml", model);
        }

        [HttpPost]
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var simpleLMSSettings = await _settingService.LoadSettingAsync<SimpleLMSSettings>(storeScope);

            //get previous picture identifiers


            //simpleLMSSettings.YouTubeApiKey = model.YouTubeApiKey;

            //simpleLMSSettings.VimeoAppId = model.VimeoAppId;

            simpleLMSSettings.VimeoClient = model.VimeoClient;

            simpleLMSSettings.VimeoAccess = model.VimeoAccess;

            simpleLMSSettings.VimeoSecret = model.VimeoSecret;
            //simpleLMSSettings.VdoCipherKey = model.VdoCipherKey;
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            //await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.YouTubeApiKey, storeScope, false);
            //await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.VimeoAppId, storeScope, false);
            await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.VimeoClient, storeScope, false);
            await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.VimeoAccess, storeScope, false);
            await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.VimeoSecret, storeScope, false);
            //await _settingService.SaveSettingAsync(simpleLMSSettings, x => x.VdoCipherKey, storeScope, false);
            //now clear settings cache
            await _settingService.ClearCacheAsync();
           

            return await Configure();
        }
    }
}
