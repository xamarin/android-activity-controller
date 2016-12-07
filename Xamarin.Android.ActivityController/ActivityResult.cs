using System;
using Android.Content;

namespace Android.App
{
    public class ActivityResult : IActivityResult
    {
        public ActivityResult(Result resultCode, int requestCode, Intent data)
        {
            ResultCode = resultCode;
            RequestCode = requestCode;
            Data = data;
        }

        public Result ResultCode { get; private set; }
        public int RequestCode { get; private set; }
        public Intent Data { get; private set; }
    }
}
