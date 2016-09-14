using System;

using GaugeDbWrapper.Wrapper;
using UIKit;

namespace GaugeDbWrapper.iOS
{
	public partial class ViewController : UIViewController
	{
		int count = 1;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			Button.AccessibilityIdentifier = "myButton";
			Button.TouchUpInside += delegate {
				var title = string.Format ("{0} clicks!", count++);
				Button.SetTitle (title, UIControlState.Normal);
			};


			// Testing in iOS

			var singleUser = new DataStorage.ClassWithAttributes() { FirstName = "Ali", LastName = "Waris", Address1 = "Pakistan" };
			var userList = new System.Collections.Generic.List<DataStorage.ClassWithAttributes>();
			userList.Add(singleUser);
			userList.Add(new DataStorage.ClassWithAttributes() { FirstName = "Muhammad Waqas", LastName = "Ahmad", Address1 = "Pakistan" });
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

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

