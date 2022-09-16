namespace RoblotV2
{
    public partial class Form1 : Form
    {
        private Form activeForm = null;
        private void OpenChildForms(Form form)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panel2.Controls.Add(form);
            panel2.Tag = form;
            form.BringToFront();
            form.Show();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForms(new Main());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForms(new Movement());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForms(new Executor());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}