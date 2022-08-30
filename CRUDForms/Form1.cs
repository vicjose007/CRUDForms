using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDForms
{
    public partial class CRUDForm : Form
    {
        CRUD_ConstSoftwareEntities db = new CRUD_ConstSoftwareEntities();

        List<string> Msg = new List<string>();

        string PeopleId = string.Empty;

        public CRUDForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetContactTypes();
            GetClientTypes();
            GetPeople();
        }

        private void GetPeople()
        {
            var people = (from a in db.People
                         select new
                         {
                             a.Id,
                             Fullname = a.FirstName + " " + a.LastName,
                             a.PhoneNumber,
                             a.EmailAddress,
                             a.Enabled,
                             a.CreatedDate
                         }).ToList();
            dgvPeople.DataSource = people;

        }

        private void GetClientTypes()
        {
            var clientTypes = db.ClientTypes.ToList();
            cbClientType.DataSource = clientTypes;
            cbClientType.DisplayMember = "Name";
            cbClientType.ValueMember = "Id";
        }

        private void GetContactTypes()
        {
            var contactTypes = db.ContactTypes.ToList();
            cbContactType.DataSource = contactTypes;   
            cbContactType.DisplayMember = "Name";
            cbContactType.ValueMember = "Id";
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void SaveForm()
        {
            var people = new Person();
            people.Id = Guid.NewGuid().ToString();
            people.FirstName = FirstName.Text;
            people.MiddleName = MiddleName.Text;
            people.LastName = LastName.Text;
            people.ClientTypeId = Convert.ToInt32(cbClientType.SelectedValue);

            if(cbContactType.SelectedIndex != 0)
            {
                people.ContactTypeId = Convert.ToInt32(cbContactType.SelectedValue);
            }

            people.SupportStaff = PhoneNumber.Text;
            people.PhoneNumber = PhoneNumber.Text;
            people.EmailAddress = EmailAddress.Text;
            people.Enabled = true;
            people.CreatedDate = DateTime.Now;

            db.People.Add(people);

            var peopleSaved = db.SaveChanges() > 0;
        }

        private bool ValidateForm()
        {
            Msg = new List<string>();

            bool result = true;

            if (string.IsNullOrEmpty(FirstName.Text))
            {
                Msg.Add("First Name Required. ");
                result = false;
            }
            if (string.IsNullOrEmpty(LastName.Text))
            {
                Msg.Add("Last Name Required. ");
                result = false;
            }
            if (string.IsNullOrEmpty(PhoneNumber.Text))
            {
                Msg.Add("Last Name Required. ");
                result = false;
            }

            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveForm();
            }
            else
            {
                string errors = string.Empty;
                int errorIndex = 1;
                foreach (var item in Msg)
                {
                    errors += $"{errorIndex}. -{item.ToString()}\n";
                    errorIndex++;
                }
                MessageBox.Show(errors, "ERRORS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void dgvPeople_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PeopleId = String.Empty;

            if (!string.IsNullOrEmpty(dgvPeople.SelectedRows[0].Cells["Id"].Value.ToString()))
            {
                PeopleId = dgvPeople.SelectedRows[0].Cells["Id"].Value.ToString();
                btnUpdate.Visible = true;
                btnDelete.Visible = true;

            }
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void GetPeopleById(string peopleId)
        {
            DefaultControls();
            var people = db.People.FirstOrDefault(x => x.Id == peopleId);

            if(people != null)
            {
                FirstName.Text = people.FirstName;
                MiddleName.Text = people.MiddleName;
                LastName.Text = people.LastName;    
                PhoneNumber.Text = people.PhoneNumber;
                EmailAddress.Text = people.EmailAddress;

            }
        }

        private void DefaultControls()
        {
            FirstName.Text = string.Empty;
            MiddleName.Text = string.Empty;
            LastName.Text = string.Empty;
            cbClientType.SelectedIndex = 0;
            cbContactType.SelectedIndex = 0;
            CheckSupportStaff.Checked = false;
            PhoneNumber.Text = string.Empty;
            EmailAddress.Text = string.Empty;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetPeopleById(PeopleId);

            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }
    }
}
