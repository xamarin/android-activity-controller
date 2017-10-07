using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Support.V7.App;
using Android.Widget;

namespace Android.App
{
    public partial class ActivityController
    {
        public string AssociatedActivityId { get; internal set; } = Guid.NewGuid().ToString();

        public TController GetActivity<TActivity, TController>() where TActivity : ActivityController where TController : ControllerActivity<TActivity>
        {
            var info = ActivityControllerRegistry.Get<TActivity>(AssociatedActivityId);

            return (TController)info.ControllerActivity;
        }

        public AppCompatActivity Activity
        {
            get
            {
                return ActivityControllerRegistry.Get(AssociatedActivityId).AppCompatActivity;
            }
        }

        // Stores pending startActivityForResult invocations
        Dictionary<int, TaskCompletionSource<ActivityResult>> activityCompletionSources = new Dictionary<int, TaskCompletionSource<ActivityResult>> ();

        public Task<ActivityResult> StartActivityForResultAsync(Type activityType)
        {
            var intent = new Intent(Activity, activityType);
            return StartActivityForResultAsync(intent);
        }
        
        public Task<ActivityResult> StartActivityForResultAsync(Intent intent)
        {
            return StartActivityForResultAsync(intent, ActivityControllerRegistry.NextRequestCode());
        }

        public Task<ActivityResult> StartActivityForResultAsync(Intent intent, int requestCode)
        {
            if (activityCompletionSources.ContainsKey(requestCode))
                throw new ArgumentException("There is already an activity request started for this requestCode", nameof(requestCode));

            var tcs = new TaskCompletionSource<ActivityResult>();

            activityCompletionSources.Add(requestCode, tcs);

            Activity.StartActivityForResult(intent, requestCode);

            return tcs.Task;
        }

        public Task<TActivityResult> StartActivityForResultAsync<TActivityResult>(Intent intent) where TActivityResult : ActivityResult
        {
            return StartActivityForResultAsync<TActivityResult>(intent, ActivityControllerRegistry.NextRequestCode());
        }

        public async Task<TActivityResult> StartActivityForResultAsync<TActivityResult>(Intent intent, int requestCode) where TActivityResult : ActivityResult
        {
            var result = await StartActivityForResultAsync(intent, requestCode);

            if (result == null)
                return null;
            
            return (TActivityResult)Activator.CreateInstance(typeof(TActivityResult), result.ResultCode, result.RequestCode, result.Data);
        }

        internal void WireupHandlers<TActivityController>(ControllerActivity<TActivityController> activity) where TActivityController : ActivityController
        {
            activity.OnStartHandler = OnStart;
            activity.OnCreateHandler = (p) => this.OnCreate (p);
            activity.OnActivityResultHandler = HandleOnActivityResult;
            activity.OnStopHandler = OnStop;
            activity.OnResumeHandler = OnResume;
            activity.OnPauseHandler = OnPause;
            activity.OnDestroyHandler = OnDestroy;
            activity.OnRestoreInstanceStateHandler = OnRestoreInstanceState;
            activity.OnSaveInstanceStateHandler = OnSaveInstanceState;
            activity.OnDestroyHandler = OnDestroy;
            activity.OnBackPressedHandler = OnBackPressed;
            activity.OnRestartHandler = OnRestart;
            activity.OnLowMemoryHandler = OnLowMemory;
            activity.OnPostResumeHandler = OnPostResume;
            activity.OnAttachedToWindowHandler = OnAttachedToWindow;
            activity.OnDetachedFromWindowHandler = OnDetachedFromWindow;
            activity.OnRequestPermissionsResultHandler = OnRequestPermissionsResult;
            activity.OnCreateContextMenuHandler = OnCreateContextMenu;
            activity.OnNewIntentHandler = OnNewIntent;
        }

        void HandleOnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (activityCompletionSources.ContainsKey(requestCode))
            {
                var tcs = activityCompletionSources[requestCode];

                if (resultCode == Result.Canceled)
                {
                    tcs.TrySetResult(null);
                    return;
                }

                tcs.TrySetResult(new ActivityResult(resultCode, requestCode, data));
            }
            else {
                OnActivityResult(requestCode, resultCode, data);
            }
        }

        protected virtual void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnCreate(Bundle savedInstanceState)
        {
        }

        protected virtual void Finish()
        {
            Activity.Finish();
            ActivityControllerRegistry.Destroy(AssociatedActivityId);
        }

        public TView FindViewById<TView>(int resourceId) where TView : global::Android.Views.View
        {
            return Activity.FindViewById<TView>(resourceId);
        }

        public void SetContentView(int layoutResId)
        {
            Activity.SetContentView(layoutResId);
        }

        protected virtual void OnSaveInstanceState(Bundle outState)
        {
        }

        protected virtual void OnRestoreInstanceState(Bundle savedInstanceState)
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnPause()
        {
        }

        protected virtual void OnResume()
        {
        }

        protected virtual void OnBackPressed()
        {
        }

        protected virtual void OnRestart()
        {
        }

        public virtual void OnLowMemory()
        {
        }

        protected virtual void OnPostResume()
        {
        }

        public virtual void OnAttachedToWindow()
        {
        }

        public virtual void OnDetachedFromWindow()
        {
        }

        public virtual void OnRequestPermissionsResult(int requestCode, string[] permissions, Content.PM.Permission[] grantResults)
        {
        }

        public virtual void OnCreateContextMenu(Views.IContextMenu menu, Views.View v, Views.IContextMenuContextMenuInfo menuInfo)
        {
        }

        protected virtual void OnNewIntent(Intent intent)
        {
        }
    }
}
