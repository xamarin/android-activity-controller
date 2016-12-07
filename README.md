Xamarin.Android ActivityController
==================================

The `ActivityController` makes managing Android Activities more .NET friendly.

Traditionally, launching subsequent Activities and waiting for them to return their results has been somewhat painful.
With the `ActivityController` you can use `async/await` by starting activities through the `StartActivityForResultAsync (..)` method.  

You can use `ActivityController` as a replacement for `AppCompatActivity`.  The underlying lifecycle of your activity is handled for you.


```csharp
public class MainController : ActivityController
{
    protected override void OnCreate(Android.OS.Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.Main);

        FindViewById<Button>(Resource.Id.myButton).Click += Button_Click;
    }

    async void Button_Click(object sender, EventArgs e)
    {
        var contactPickerIntent = new Intent(Intent.ActionPick, ContactsContract.CommonDataKinds.Phone.ContentUri);

        var result = await StartActivityForResultAsync(contactPickerIntent);

        var contactUri = result?.Data?.Data;

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
```

Your `ActivityController` subclass can override some of the typical methods you would expect in an `Activity`.

You can also access the instance of the underlying `AppCompatActivity` directly via the `Activity` property.


You will also need to subclass `ControllerActivity<TController>` in order to be able to decorate it with the `[Activity]` attribute
which will be merged into your app's _AndroidManifest.xml_ file.  This subclass can be an empty implementation.  The generic type argument should be the type of your `ActivityController` subclass:

```csharp
[Activity(Theme = "@style/Theme.AppCompat")]
public class MainActivity : ControllerActivity<MainController> { }
```  

Finally, if you need to, you can also alter your subclass of `ControllerActivity` directly to override more methods
and make other changes. 

```csharp
[Activity(Theme = "@style/Theme.AppCompat")]
public class MainActivity : ControllerActivity<MainController>
{
    public override bool OnTouchEvent(Android.Views.MotionEvent e)
    {
        // Do something interesting

        return base.OnTouchEvent(e);
    }
}
```
