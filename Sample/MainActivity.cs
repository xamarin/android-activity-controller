using System;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Android;

namespace XamActivityController
{
    [Activity(MainLauncher = true, Label = "Activity Controller Test", Theme = "@style/Theme.AppCompat")]
    public class MainActivity : ControllerActivity<MainController>
    {
    }

    public class MainController : ActivityController
    {
        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FirstLayout);

            FindViewById<Button>(Resource.Id.myButton).Click += Button_Click;
        }

        async void Button_Click(object sender, EventArgs e)
        {
            var contactPickerIntent = new Intent(Intent.ActionPick, ContactsContract.CommonDataKinds.Phone.ContentUri);

            var result = await StartActivityForResultAsync<ContactPickerActivityResult>(contactPickerIntent);

            var contactUri = result?.SelectedContactUri;

            if (contactUri != null)
                Toast.MakeText(Activity, "You Picked: " + GetDisplayName(contactUri), ToastLength.Long).Show();
        }

        string GetDisplayName(Android.Net.Uri uri)
        {
            var c = Activity.ContentResolver.Query(uri, null, null, null, null);
            c.MoveToFirst();
            return c.GetString(c.GetColumnIndex(ContactsContract.ContactNameColumns.DisplayNamePrimary));
        }
    }
}
