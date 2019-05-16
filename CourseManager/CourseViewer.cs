// ----------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CourseManager
{
    public partial class CourseViewer : Form
    {
        //Create an ObjectContext instance based on SchoolEntity
        private SchoolEntities schoolContext;

        public CourseViewer()
        {
            InitializeComponent();
        }

        private void closeForm_Click(object sender, EventArgs e)
        {
            //Dispose the object context.
            schoolContext.Dispose();

            //Close the form
            this.Close();
        }

        private void CourseViewer_Load(object sender, EventArgs e)
        {
            //Initialize the ObjectContext
            schoolContext = new SchoolEntities();

            //Define a query that returns all Department objects and related
            // Course objects, ordered by name.
            ObjectQuery<Department> departmentQuery = schoolContext.Department.Include("Course").OrderBy("it.Name");

            try
            {
                //Bind the ComboBox control to the query, which is
                // executed during data binding.
                this.departmentList.DisplayMember = "Name";
                this.departmentList.DataSource = departmentQuery;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void departmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Get the object for the selected department.
                Department department = (Department)this.departmentList.SelectedItem;

                //Bind the grid view to the collection of Course objects
                // that are related to the selected Department object.
                courseGridView.DataSource = department.Course;

                // Hide the columns that are bound to the navigation properties on Course.
                courseGridView.Columns["Department"].Visible = false;
                courseGridView.Columns["CourseGrade"].Visible = false;
                courseGridView.Columns["OnlineCourse"].Visible = false;
                courseGridView.Columns["OnsiteCourse"].Visible = false;
                courseGridView.Columns["Person"].Visible = false;

                courseGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                //Save object changes to the database, display a message,
                // and refresh the form.
                schoolContext.SaveChanges();
                MessageBox.Show("Changes saved to the database.");
                this.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
