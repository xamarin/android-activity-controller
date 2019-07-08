﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace Android.App
{
    public abstract class ControllerFormsActivity<TController> : FormsAppCompatActivity, IControllerActivity 
           where TController : ActivityController
    {
        public TController GetController ()
        {
            var info = (AssociatedActivityInfo<TController>)ActivityControllerRegistry.Get(AssociatedActivityId);
            return info.ActivityController;
        }

        public string AssociatedActivityId { get; private set; }

        public Task<ActivityResult> StartActivityForResultAsync(Intent intent)
        {
            return StartActivityForResultAsync(intent, ActivityControllerRegistry.NextRequestCode());
        }

        public Task<ActivityResult> StartActivityForResultAsync(Intent intent, int requestCode)
        {
            var info = (AssociatedActivityInfo<TController>)ActivityControllerRegistry.Get(AssociatedActivityId);
            return info.ControllerActivity.StartActivityForResultAsync(intent, requestCode);
        }

        public Action<Bundle> OnCreateHandler { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AssociatedActivityId = savedInstanceState?.GetString(ActivityControllerRegistry.ASSOC_ACTIVITY_ID)
                                         ?? Intent?.GetStringExtra(ActivityControllerRegistry.ASSOC_ACTIVITY_ID)
                                         ?? Guid.NewGuid().ToString();

            ActivityControllerRegistry.Associate<TController> (AssociatedActivityId, this);

            base.OnCreate(savedInstanceState);

            OnCreateHandler?.Invoke(savedInstanceState);
        }

        public Action<int, Result, Intent> OnActivityResultHandler { get; set; }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            OnActivityResultHandler?.Invoke(requestCode, resultCode, data);
        }

        public Action<Bundle> OnSaveInstanceStateHandler { get; set; }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString(ActivityControllerRegistry.ASSOC_ACTIVITY_ID, AssociatedActivityId);

            OnSaveInstanceStateHandler?.Invoke(outState);

            base.OnSaveInstanceState(outState);
        }

        public Action<Bundle> OnRestoreInstanceStateHandler { get; set; }
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            OnRestoreInstanceStateHandler?.Invoke(savedInstanceState);
            base.OnRestoreInstanceState(savedInstanceState);
        }

        public Action OnDestroyHandler { get; set; }
        protected override void OnDestroy()
        {
            OnDestroyHandler?.Invoke();
            base.OnDestroy();
        }

        public Action OnStartHandler { get; set; }
        protected override void OnStart()
        {
            base.OnStart();
            OnStartHandler?.Invoke();
        }

        public Action OnStopHandler { get; set; }
        protected override void OnStop()
        {
            OnStopHandler?.Invoke();
            base.OnStop();
        }

        public Action OnPauseHandler { get; set; }
        protected override void OnPause()
        {
            OnPauseHandler?.Invoke();
            base.OnPause();
        }

        public Action OnResumeHandler { get; set; }
        protected override void OnResume()
        {
            base.OnResume();
            OnResumeHandler?.Invoke();
        }

        public Action OnBackPressedHandler { get; set; }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OnBackPressedHandler?.Invoke();
        }

        public Action OnRestartHandler { get; set; }
        protected override void OnRestart()
        {
            base.OnRestart();
            OnRestartHandler?.Invoke();
        }

        public Action OnLowMemoryHandler { get; set; }
        public override void OnLowMemory()
        {
            base.OnLowMemory();
            OnLowMemoryHandler?.Invoke();
        }

        public Action OnPostResumeHandler { get; set; }
        protected override void OnPostResume()
        {
            base.OnPostResume();
            OnPostResumeHandler?.Invoke();
        }

        public Action OnAttachedToWindowHandler { get; set; }
        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            OnAttachedToWindowHandler?.Invoke();
        }

        public Action OnDetachedFromWindowHandler { get; set; }
        public override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            OnDetachedFromWindowHandler?.Invoke();
        }

        public Action<int,string[],Content.PM.Permission[]> OnRequestPermissionsResultHandler { get; set; }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            OnRequestPermissionsResultHandler?.Invoke(requestCode, permissions, grantResults);
        }

        public Action<Views.IContextMenu,Views.View,Views.IContextMenuContextMenuInfo> OnCreateContextMenuHandler { get; set; }
        public override void OnCreateContextMenu(Views.IContextMenu menu, Views.View v, Views.IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            OnCreateContextMenuHandler?.Invoke(menu, v, menuInfo);
        }

        public Action<Intent> OnNewIntentHandler { get; set; }

        public AppCompatActivity AppCompatActivity => this;

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            OnNewIntentHandler?.Invoke(intent);
        }
    }
}
