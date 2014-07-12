using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Epirb.Core;

namespace Epirb {

	public class DetailListAdapter : BaseAdapter<Detail> {
		Activity context = null;
		IList<Detail> details = new List<Detail>();
		
		public DetailListAdapter (Activity context, IList<Detail> details) : base ()
		{
			this.context = context;
			this.details = details;
		}
		
		public override Detail this[int position]
		{
			get { return details[position]; }
		}
		
		public override long GetItemId (int position)
		{
			return position;
		}
		
		public override int Count
		{
			get { return details.Count; }
		}
		
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = details[position];

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
					context.LayoutInflater.Inflate(
					Resource.Layout.DetailListItem, 
					parent, 
					false)) as LinearLayout;

			// Find references to each subview in the list item's view
			var txtName = view.FindViewById<TextView>(Resource.Id.DetailName);
			var txtDescription = view.FindViewById<TextView>(Resource.Id.DetailValue);
			var txtConcat = view.FindViewById<TextView>(Resource.Id.ConcatText);

			//Assign item's values to the various subviews
			txtName.SetText (item.Name, TextView.BufferType.Normal);
			txtDescription.SetText (item.Value, TextView.BufferType.Normal);
			txtConcat.SetText (item.Concat, TextView.BufferType.Normal);

			return view;
		}
	}
}