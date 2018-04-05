using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPA.UI.Tutorial
{
	public partial class DatagridTest : Form
	{
		public DatagridTest()
		{
			InitializeComponent();
		}

		

		private List<Student> Students { get; set; }

		private void Form1_Load(object sender, EventArgs e)
		{
			Students = new List<Student>();
			Students.Add(new Student(){Name="Nasrul", Age=32, select=true});
			Students.Add(new Student(){Name="Chap", Age=32, select=true});
			Students.Add(new Student(){Name="Redz", Age=30, select=true});
			dgTest.DataSource= Students;
			chkAll.Checked = false;
		}

		//not work
		private void chkAll_CheckedChanged(object sender, EventArgs e)
		{
			var students = (List<Student>)this.dgTest.DataSource;
			if (chkAll.Checked)
			{
				foreach (var student in students)
				{
					student.select = false;
				}
			}
			else
			{
				foreach (var student in students)
				{
					student.select = false;
				}

			}
		
		}

		//working fine -that's what i want!
		private void button1_Click(object sender, EventArgs e)
		{
			var students = (List<Student>)this.dgTest.DataSource;
			MessageBox.Show("Status " + students[1].select.ToString());
		}


		private void ButtonClick(object sender, EventArgs e)
		{
			MessageBox.Show("hahah....click triggered!");
		}



	}

	public class Student
	{
		public bool select { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}
}
