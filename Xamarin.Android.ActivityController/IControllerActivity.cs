using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace Android.App
{
    public interface IControllerActivity
    {
        Task<ActivityResult> StartActivityForResultAsync(Intent intent, int requestCode);
        Action<Bundle> OnCreateHandler { set; }
        Action<int, Result, Intent> OnActivityResultHandler { set; }
        Action<Bundle> OnSaveInstanceStateHandler { set; }
        Action<Bundle> OnRestoreInstanceStateHandler { set; }
        Action OnDestroyHandler { set; }
        Action OnStartHandler { set; }
        Action OnStopHandler { set; }
        Action OnPauseHandler { set; }
        Action OnResumeHandler { set; }
        Action OnBackPressedHandler { set; }
        Action OnRestartHandler { set; }
        Action OnLowMemoryHandler { set; }
        Action OnPostResumeHandler { set; }
        Action OnAttachedToWindowHandler { set; }
        Action OnDetachedFromWindowHandler { set; }
        Action<int, string[], Content.PM.Permission[]> OnRequestPermissionsResultHandler { set; }
        Action<Views.IContextMenu, Views.View, Views.IContextMenuContextMenuInfo> OnCreateContextMenuHandler { set; }
        Action<Intent> OnNewIntentHandler { set; }
        AppCompatActivity AppCompatActivity { get; }
    }
}