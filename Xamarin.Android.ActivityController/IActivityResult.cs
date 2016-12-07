using System;
using Android.Content;

namespace Android.App
{
    public interface IActivityResult
    {
        Result ResultCode { get; }
        int RequestCode { get; }
        Intent Data { get; }
    }

    public interface ITypedActivityResult : IActivityResult
    {
        void Parse(Result resultCode, int requestCode, Intent data);
    }
}
