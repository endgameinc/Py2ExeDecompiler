using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlatUI
{
	public class FlatLabel : Label
	{
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}

		public FlatLabel()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			Font = new Font("Segoe UI", 8);
			ForeColor = Color.White;
			BackColor = Color.Transparent;
			Text = Text;
		}
	}
}
