using LogParserWithMongoDb.Model;
using LogParserWithMongoDb.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogParserWithMongoDb
{
    public partial class OfferErrorAnswer : Form
    {
        public List<KnownErrorView> KnowList { get; set; }

        public OfferErrorAnswer(List<KnownErrorView> knowList)
        {
            KnowList = knowList;            
            InitializeComponent();
            InitGrid();
        }

        private void InitGrid()
        {
            var source = new BindingSource();
            source.DataSource = KnowList;
            dataGridView1.DataSource = source;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                var message = row.Cells["Message"].Value.ToString();
                var statusCode = row.Cells["StatusCode"].Value.ToString();
                var statusTitle = row.Cells["StatusTitle"].Value.ToString();
                var answer = row.Cells["Answer"].Value.ToString();
                textBox1.Text = message;
                textBox3.Text = statusCode;
                textBox4.Text = statusTitle;
                textBox2.Text = answer;
            }
        }
    }
}
