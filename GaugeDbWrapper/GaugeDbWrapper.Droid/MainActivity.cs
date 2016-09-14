using Android.App;
using Android.Widget;
using Android.OS;
using GaugeDbWrapper.Wrapper;
namespace GaugeDbWrapper.Droid
{
	[Activity (Label = "GaugeDbWrapper.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};

            // Testing in Android

            var singleUser = new DataStorage.ClassWithAttributes() { FirstName = "Ali", LastName = "Waris", Address1 = "Pakistan" };
            var userList = new System.Collections.Generic.List<DataStorage.ClassWithAttributes>();
            userList.Add(singleUser);
            userList.Add(new DataStorage.ClassWithAttributes() { FirstName = "Muhammad Waqas", LastName = "Ahmad", Address1 = "Pakistan"});
            userList.Add(new DataStorage.ClassWithAttributes() { FirstName = "Faiz", LastName = "Rasool", Address1 = "Pakistan" });
            userList.Add(new DataStorage.ClassWithAttributes() { FirstName = "Meng Leong", LastName = "Kwan", Address1 = "Malaysia" });
            
            // Storage
            DataStorage.DbManagerInstance.SaveUser(userList);

            // Retrieval
            var allUserList = DataStorage.DbManagerInstance.GetAllUsers();
            var userById = DataStorage.DbManagerInstance.GetUserById(3);

            // Removal
            var isDeleted = DataStorage.DbManagerInstance.DeleteUser(singleUser);
        }


    }
}


