using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;

namespace Android.App
{
    public partial class ActivityController
    {
        public Task<ContactPickerActivityResult> PickContactAsync()
        {
            var contactPickerIntent = new Intent(Intent.ActionPick, ContactsContract.CommonDataKinds.Phone.ContentUri);

            return StartActivityForResultAsync<ContactPickerActivityResult>(contactPickerIntent);
        }

        public Task<MediaPickerActivityResult> PickPhotoAsync(string title)
        {
            var intent = Intent.CreateChooser (new Intent(Intent.ActionPick).SetType("image/*"), title);

            return StartActivityForResultAsync<MediaPickerActivityResult>(intent);
        }

        public Task<MediaPickerActivityResult> TakePhotoAsync(string title)
        {
            var intent = Intent.CreateChooser(new Intent(MediaStore.ActionImageCapture).SetType("image/*"), title);

            return StartActivityForResultAsync<MediaPickerActivityResult>(intent);
        }

        public Task<MediaPickerActivityResult> PickVideoAsync(string title)
        {
            var intent = Intent.CreateChooser(new Intent(Intent.ActionPick).SetType("video/*"), title);

            return StartActivityForResultAsync<MediaPickerActivityResult>(intent);
        }

        public Task<MediaPickerActivityResult> TakeVideoAsync(string title, int? videoQuality = null, int? videoSizeLimit = null, TimeSpan? videoDurationLimit = null)
        {
            var intent = Intent.CreateChooser(new Intent(MediaStore.ActionImageCapture).SetType("video/*"), title);

            if (videoQuality.HasValue)
                intent.PutExtra(MediaStore.ExtraVideoQuality, videoQuality.Value);
            if (videoSizeLimit.HasValue)
                intent.PutExtra(MediaStore.ExtraSizeLimit, videoSizeLimit.Value);
            if (videoDurationLimit.HasValue)
                intent.PutExtra(MediaStore.ExtraDurationLimit, (int)videoDurationLimit.Value.TotalSeconds);
            
            return StartActivityForResultAsync<MediaPickerActivityResult>(intent);
        }
    }
}
