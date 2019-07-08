using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace Android.App
{
    internal static class ActivityControllerRegistry
    {
        internal const string ASSOC_ACTIVITY_ID = "ASSOC_ACTIVITY_ID";

        static int requestCode = 1001;
        static readonly object requestCodeLockObj = new object();
        internal static int NextRequestCode()
        {
            lock (requestCodeLockObj)
            {
                var r = requestCode++;
                if (requestCode >= int.MaxValue)
                    requestCode = 1001;
                return r;
            }
        }

        static readonly Dictionary<string, AssociatedActivityInfo> associatedActivities = new Dictionary<string, AssociatedActivityInfo>();

        internal static AssociatedActivityInfo<TController> Associate<TController>(string associatedActivityId, IControllerActivity activity) where TController : ActivityController
        {
            AssociatedActivityInfo<TController> info;

            if (associatedActivities.ContainsKey(associatedActivityId))
            {
                info = (AssociatedActivityInfo<TController>)Get(associatedActivityId);
            }
            else 
            {
                info = new AssociatedActivityInfo<TController>
                {
                    Id = associatedActivityId,
                    ControllerActivity = activity,
                    AppCompatActivity = activity.AppCompatActivity,
                };
                associatedActivities.Add(info.Id, info);
            }

            if (info.ActivityController == null)
            {
                info.ActivityController = Activator.CreateInstance<TController>();
                info.ActivityController.AssociatedActivityId = associatedActivityId;
            }

            info.ControllerActivity = activity;
            info.AppCompatActivity = activity.AppCompatActivity;
            info.ActivityController.WireupHandlers(activity);

            return info;
        }

        internal static void Destroy(string associatedActivityId)
        {
            associatedActivities.Remove(associatedActivityId);
        }

        internal static AssociatedActivityInfo<TController> Get<TController>(string associatedActivityId) where TController : ActivityController
        {
            return (AssociatedActivityInfo<TController>)associatedActivities[associatedActivityId];
        }

        internal static AssociatedActivityInfo Get(string associatedActivityId)
        {
            return associatedActivities[associatedActivityId];
        }
    }

    internal class AssociatedActivityInfo<TController> : AssociatedActivityInfo where TController : ActivityController
    {
        public IControllerActivity ControllerActivity { get; set; }
        public TController ActivityController { get; set; }
    }

    internal class AssociatedActivityInfo
    {
        public string Id { get; set; }
        public AppCompatActivity AppCompatActivity { get; set; }
    }
}
