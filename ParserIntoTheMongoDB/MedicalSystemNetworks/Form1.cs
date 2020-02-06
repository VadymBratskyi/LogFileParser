using NeuronLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalSystemNetworks {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			var aboutForm = new AboutBox1();
			aboutForm.ShowDialog();
		}

		private void imageToolStripMenuItem_Click(object sender, EventArgs e) {
			var openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "JPG|*.jpg|PNG|*.png";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				var imgConvertor = new PictureConvector();
				var inputs = imgConvertor.Convert(openFileDialog.FileName);
				var result = Program.Controller.ImageNetwork.FeedForward(inputs).Output;
			}
			
		}
	}
}
