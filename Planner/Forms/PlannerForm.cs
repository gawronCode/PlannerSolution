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
            _context = new PlannerDbContext();
            InitializeComponent();
            AddItemsToStatusComboBox();
        }

        private void AddItemsToStatusComboBox()
        {
            var statuses = _context.Status.ToList();
            foreach (var status in statuses)
            {
                StatusComboBox.Items.Add(status);
            }

            StatusComboBox.SelectedIndex = 0;
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            AddNewTaskToDb();
        }

        private void AddNewTaskToDb()
        {
            if (StatusComboBox.SelectedItem is null || TaskTextBox.Text == String.Empty)
            {
                MessageBox.Show("Not enough data to create task!!!");
                return;
            }

            var task = new Planner.Model.Task()
            {
                Name = TaskTextBox.Text,
                DueDate = DueDateTimePicker.Value,
                StatusId = (StatusComboBox.SelectedItem as Model.Status).Id
            };

            _context.Task.Add(task);
            _context.SaveChanges();
        }
    }
}
