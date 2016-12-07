using System;
using Android.Content;

namespace Android.App
{
    public class ContactPickerActivityResult : ActivityResult
    {
        public ContactPickerActivityResult(Result resultCode, int requestCode, Intent data)
            : base(resultCode, requestCode, data) { }
        
        public Android.Net.Uri SelectedContactUri { get { return Data?.Data; } }
    }
}
