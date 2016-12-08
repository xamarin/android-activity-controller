using System;
using System.IO;
using Android.Content;

namespace Android.App
{
    public class MediaPickerActivityResult : ActivityResult
    {
        public MediaPickerActivityResult(Result resultCode, int requestCode, Intent data)
            : base(resultCode, requestCode, data) { }

        public Android.Net.Uri SelectedMediaUri { get { return Data?.Data; } }

        public Stream GetMediaStream(Context context)
        {
            var uri = SelectedMediaUri;
            if (uri == null)
                return null;
            
            return context.ContentResolver.OpenInputStream(uri);
        }
    }
}
