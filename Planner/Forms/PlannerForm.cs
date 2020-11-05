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
            RefreshData();
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

        private void RefreshData()
        {
            BindingSource bindingSource = new BindingSource();

            var query = from t in _context.Task
                orderby t.DueDate
                select new {t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.DueDate};

            bindingSource.DataSource = query.ToList();
            TasksDataGridView.DataSource = bindingSource;
            TasksDataGridView.Refresh();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            AddNewTaskToDb();
            RefreshData();
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

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteTaskFromDb();
            RefreshData();
        }

        private void DeleteTaskFromDb()
        {
            var task = _context.Task.Find((int)TasksDataGridView.SelectedCells[0].Value);
            _context.Task.Remove(task);
            _context.SaveChanges();
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateTaskInDb();
        }

        private void UpdateTaskInDb()
        {
            if (UpdateBtn.Text == "Update")
            {
                try
                {
                    TaskTextBox.Text = TasksDataGridView.SelectedCells[1].Value.ToString();
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Please select row");
                    return;
                }

                DueDateTimePicker.Value = (DateTime)TasksDataGridView.SelectedCells[3].Value;
                foreach (Status s in StatusComboBox.Items)
                {
                    if (s.Name == TasksDataGridView.SelectedCells[2].ToString())
                    {
                        StatusComboBox.SelectedItem = s;
                    }
                }
                UpdateBtn.Text = "Save";
                CreateBtn.Enabled = false;
                DeleteBtn.Enabled = false;
            }
            else if (UpdateBtn.Text == "Save")
            {
                var updatedTask = _context.Task.Find((int)TasksDataGridView.SelectedCells[0].Value);
                updatedTask.Name = TaskTextBox.Text;
                updatedTask.StatusId = (StatusComboBox.SelectedItem as Model.Status).Id;
                updatedTask.DueDate = DueDateTimePicker.Value;

                _context.SaveChanges();
                RefreshData();
                UpdateBtn.Text = "Update";
                TaskTextBox.Text = String.Empty;
                DueDateTimePicker.Value = DateTime.Now;
                CreateBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
        }

    }
}
