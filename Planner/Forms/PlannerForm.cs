using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Planner.Model;

namespace Planner.Forms
{
    public partial class PlannerForm : Form
    {

        private readonly PlannerDbContext _context;

        public PlannerForm()
        {
            InitializeComponent();
            _context = new PlannerDbContext();

            var statuses = _context.Status.ToList();

            foreach (Status status in statuses)
            {
                StatusComboBox.Items.Add(status);
            }

        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
