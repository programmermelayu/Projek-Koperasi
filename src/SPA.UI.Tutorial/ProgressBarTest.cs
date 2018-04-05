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
	public partial class ProgressBarTest : Form
	{
		public ProgressBarTest()
		{
			InitializeComponent();
			//Shown += new EventHandler(ProgressBarTest_Shown);

			// To report progress from the background worker we need to set this property
			backgroundWorker1.WorkerReportsProgress = true;
			// This event will be raised on the worker thread when the worker starts
			backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
			// This event will be raised when we call ReportProgress
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		}

		void ProgressBarTest_Shown(object sender, EventArgs e)
		{
			// Start the background worker
			//backgroundWorker1.RunWorkerAsync();
		}
		// On worker thread so do our thing!
		void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			// Your background task goes here
			for (int i = 0; i <= 100; i++)
			{
				// Report progress to 'UI' thread
				backgroundWorker1.ReportProgress(i);
				// Simulate long task
				SetText("This is loop " + i.ToString() + Environment.NewLine);
				this.CreatePerson(i);
				SetDatagrid(Persons);
				System.Threading.Thread.Sleep(10);
			
			}
		}


		delegate void SetTextCallback(string text);
		/// <summary>
		/// <param name="text"></param>
		private void SetText(string text)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.textBox1.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
			}
		}


		// Back on the 'UI' thread so we can update the progress bar
		void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// The progress percentage is a property of e
			progressBar1.Value = e.ProgressPercentage;
		}

		void ProgressBarTest_Load(object sender, EventArgs e)
		{
			// Start the background worker

		}

		private void progressBar1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			backgroundWorker1.RunWorkerAsync();
		}

		delegate void SetDatagridCallback(List<Person> persons);
		private void SetDatagrid(List<Person> persons)
		{
			if (this.dataGridView1.InvokeRequired)
			{
				SetDatagridCallback d = new SetDatagridCallback(SetDatagrid);
				//this.Invoke(d, new object[] { persons });
			}
			//else
			//{
			//	this.dataGridView1.DataSource = persons;
			//	if (persons.Count  > 4)
			//	{
			//		return;
			//	}
			//}
		}

		private List<Person> Persons = new List<Person>();
		private void CreatePerson(int i)
		{
			Person person = new Person();
			person.Name = "Nasrul" + i.ToString();
			person.Age = 32 + i;
			Persons.Add(person);
		}

		public class Person
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

	}



}
