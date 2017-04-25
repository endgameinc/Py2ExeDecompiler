using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	[DefaultEvent("CheckedChanged")]
	public class FlatRadioButton : Control
	{
		private MouseState State = MouseState.None;
		private int W;
		private int H;
		private _Options O;

		private bool _Checked;
		public bool Checked
		{
			get { return _Checked; }
			set
			{
				_Checked = value;
				InvalidateControls();
				if (CheckedChanged != null)
				{
					CheckedChanged(this);
				}
				Invalidate();
			}
		}

		public event CheckedChangedEventHandler CheckedChanged;
		public delegate void CheckedChangedEventHandler(object sender);

		protected override void OnClick(EventArgs e)
		{
			if (!_Checked)
				Checked = true;
			base.OnClick(e);
		}

		private void InvalidateControls()
		{
			if (!IsHandleCreated || !_Checked)
				return;
			foreach (Control C in Parent.Controls)
			{
				if (!object.ReferenceEquals(C, this) && C is FlatRadioButton)
				{
					((FlatRadioButton)C).Checked = false;
					Invalidate();
				}
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			InvalidateControls();
		}

		[Flags()]
		public enum _Options
		{
			Style1,
			Style2
		}

		[Category("Options")]
		public _Options Options
		{
			get { return O; }
			set { O = value; }
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Height = 22;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			State = MouseState.Down;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			State = MouseState.Over;
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			State = MouseState.Over;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			State = MouseState.None;
			Invalidate();
		}

		private Color _BaseColor = Color.FromArgb(45, 47, 49);
		private Color _BorderColor = Helpers.FlatColor;
		private Color _TextColor = Color.FromArgb(243, 243, 243);

		public FlatRadioButton()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;
			Cursor = Cursors.Hand;
			Size = new Size(100, 22);
			BackColor = Color.FromArgb(60, 70, 73);
			Font = new Font("Segoe UI", 10);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.UpdateColors();

			Bitmap B = new Bitmap(Width, Height);
			Graphics G = Graphics.FromImage(B);
			W = Width - 1;
			H = Height - 1;

			Rectangle Base = new Rectangle(0, 2, Height - 5, Height - 5);
			Rectangle Dot = new Rectangle(4, 6, H - 12, H - 12);

			var _with10 = G;
			_with10.SmoothingMode = SmoothingMode.HighQuality;
			_with10.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			_with10.Clear(BackColor);

			switch (O)
			{
				case _Options.Style1:
					//-- Base
					_with10.FillEllipse(new SolidBrush(_BaseColor), Base);

					switch (State)
					{
						case MouseState.Over:
							_with10.DrawEllipse(new Pen(_BorderColor), Base);
							break;
						case MouseState.Down:
							_with10.DrawEllipse(new Pen(_BorderColor), Base);
							break;
					}

					//-- If Checked 
					if (Checked)
					{
						_with10.FillEllipse(new SolidBrush(_BorderColor), Dot);
					}
					break;
				case _Options.Style2:
					//-- Base
					_with10.FillEllipse(new SolidBrush(_BaseColor), Base);

					switch (State)
					{
						case MouseState.Over:
							//-- Base
							_with10.DrawEllipse(new Pen(_BorderColor), Base);
							_with10.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
							break;
						case MouseState.Down:
							//-- Base
							_with10.DrawEllipse(new Pen(_BorderColor), Base);
							_with10.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
							break;
					}

					//-- If Checked
					if (Checked)
					{
						//-- Base
						_with10.FillEllipse(new SolidBrush(_BorderColor), Dot);
					}
					break;
			}

			_with10.DrawString(Text, Font, new SolidBrush(_TextColor), new Rectangle(20, 2, W, H), Helpers.NearSF);

			base.OnPaint(e);
			G.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(B, 0, 0);
			B.Dispose();
		}

		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);

			_BorderColor = colors.Flat;
		}
	}
}
