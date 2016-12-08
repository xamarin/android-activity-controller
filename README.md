Xamarin.Android ActivityController
==================================


The `ActivityController` makes managing Android Activities more .NET friendly.

[![](https://img.shields.io/jenkins/s/https/jenkins.mono-project.com/Components-AndroidActivityController.svg)](https://jenkins.mono-project.com/view/Components/job/Components-AndroidActivityController/)
[![](https://img.shields.io/nuget/vpre/Xamarin.Android.ActivityController.svg)](https://www.nuget.org/packages/Xamarin.Android.ActivityController/)

Traditionally, launching subsequent Activities and waiting for them to return their results has been somewhat painful.
With the `ActivityController` you can use `async/await` by starting activities through the `StartActivityForResultAsync (..)` method.  

You can use `ActivityController` as a replacement for `AppCompatActivity`.  The underlying lifecycle of your activity is handled for you.

Your `ActivityController` subclass can override some of the typical methods you would expect in an `Activity`.  It must be associated with a subclass of `ControllerActivity<TController>`.  Here is a boiler plate implementation:

```csharp
[Activity(MainLauncher = true, Label = "Your Activity", Theme = "@style/Theme.AppCompat")]
public class MainActivity : ControllerActivity<MainActivity.MainController>
{
    public class MainController : ActivityController
    {
        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FirstLayout);

            // Your code            
        }
    }
}
```

Now you are ready to call `StartActivityForResultAsync (..)` from your controller:

```csharp
async void Button_Click(object sender, EventArgs e)
{
    var contactPickerIntent = new Intent(Intent.ActionPick, ContactsContract.CommonDataKinds.Phone.ContentUri);

    var result = await StartActivityForResultAsync(contactPickerIntent);

    var contactUri = result?.Data?.Data;

	// Get Contact Name from the ContentResolver
	// var displayName = ...
	
    if (contactUri != null)
        Toast.MakeText(Activity, "You Picked: " + displayName, ToastLength.Long).Show();
}
```


You can also access the instance of the underlying `AppCompatActivity` directly via the `Activity` property of your `ActivityController`, as seen in the snippet above.


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

### Helpers

To make some common tasks easier, `ActivityController` also contains some helper methods which construct the appropriate `Intent` and returns a strongly typed version of `ActivityResult` with more useful properties.

The helper methods include:

 - PickContactAsync

```csharp
var result = await PickContactAsync ();
var contactUri = result.SelectedContactUri;
```  

 - PickPhotoAsync
 - TakePhotoAsync
 - PickVideoAsync
 - TakeVideoAsync

```csharp
var result = await PickPhotoAsync ("Title");
var stream = result.GetMediaStream ();
```


