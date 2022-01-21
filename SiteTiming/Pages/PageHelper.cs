using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SiteTiming.Pages
{
    public static class PageHelper
    {
        public static bool IsPageInitializedYordan(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            bool result = false;

            var isInitializedProp = jsRuntime.GetType().GetProperties().FirstOrDefault(f => f.Name == "IsInitialized");
            if (isInitializedProp != null)
            {
                bool isJsInitialized = (bool)isInitializedProp.GetValue(jsRuntime);
                if (isJsInitialized == true)
                {

                    return true;

                    //if (navigationManager != null
                    //&& !navigationManager.Uri.Contains("_blazor/initializers", StringComparison.InvariantCultureIgnoreCase)
                    //&& !navigationManager.Uri.Contains("favicon.ico", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    result = true;
                    //}
                }
            }

            return result;
        }
    }
}
