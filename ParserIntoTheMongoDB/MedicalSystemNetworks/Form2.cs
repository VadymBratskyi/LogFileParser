using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalSystemNetworks {
	public partial class Form2 : Form {

		private List<TextBox> Inputs = new List<TextBox>();
		public Form2() {
			InitializeComponent();
			var propInfo = typeof(Pacient).GetProperties();
			for (var i = 0; i < propInfo.Length; i++) {
				var txt = CreateTextBox(i, propInfo[i]);
				Controls.Add(txt);
				Inputs.Add(txt);
			}
		}

		public bool? ShowForm() {
			var form = new Form2();
			if (form.ShowDialog() == DialogResult.OK) {
				var pacient = new Pacient();
				foreach (var txt in form.Inputs) {
					pacient.GetType().InvokeMember(txt.Tag.ToString(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, pacient, new[] { txt.Text });
				}
				//var result = Program.Controller.NeuronNetworks.FeedForward()?.Output;
				return false;
			}
			return null;
		}

		private void Form2_Load(object sender, EventArgs e) {
			
		}

		private TextBox CreateTextBox(int number, PropertyInfo property) {
			var y = number * 25 +12;
			var textbox = new TextBox();
			textbox.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left
			| AnchorStyles.Right);
			textbox.Location = new Point(12, y);
			textbox.Name = "textBox"+ number;
			textbox.Size = new Size(499, 20);
			textbox.TabIndex = 0;
			textbox.ForeColor = SystemColors.GrayText;
			textbox.Text = property.Name;
			textbox.Tag = property.Name;
			textbox.GotFocus += (object sender, EventArgs e) => {
				var txt = sender as TextBox;
				if (txt.Text == txt.Tag.ToString()) {
					txt.Text = "";
					txt.ForeColor = SystemColors.WindowText;
				}
			};
			textbox.LostFocus += (object sender, EventArgs e) => {
				var txt = sender as TextBox;
				if (txt.Text == "") {
					txt.ForeColor = SystemColors.GrayText;
					txt.Text = txt.Tag.ToString();
				}
			};

			return textbox;
		}

		private void button1_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
