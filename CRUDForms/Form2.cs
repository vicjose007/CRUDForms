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
    public partial class Form2 : Form
    {
        CRUD_ConstSoftwareEntities db = new CRUD_ConstSoftwareEntities();

        List<string> Msg = new List<string>();

        int ContactId = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            GetContact();
        }

        private void GetContact()
        {
            var contact = (from a in db.ContactTypes
                          select new
                          {
                              a.Id,
                              a.Name,
                              a.Enabled,
                              a.CreatedDate
                          }).ToList();
            dgvContact.DataSource = contact;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
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

        private bool ValidateForm()
        {
            Msg = new List<string>();

            bool result = true;

            if (string.IsNullOrEmpty(Name.Text))
            {
                Msg.Add("Name Required. ");
                result = false;
            }
            if (string.IsNullOrEmpty(Description.Text))
            {
                Msg.Add("Description Required. ");
                result = false;
            }

            return result;
        }

        private void SaveForm()
        {
            var contact = new ContactType();
            contact.Name = Name.Text;
            contact.Description = Description.Text;

            contact.Enabled = true;
            contact.CreatedDate = DateTime.Now;

            db.ContactTypes.Add(contact);

            var contactSaved = db.SaveChanges() > 0;
        }

        private void dgvContact_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ContactId = 0;

            if (!string.IsNullOrEmpty(dgvContact.SelectedRows[0].Cells["Id"].Value.ToString()))
            {
                dgvContact.SelectedRows[0].Cells["Id"].Value.ToString();
                btnUpdate.Visible = true;
                btnDelete.Visible = true;

            }
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void GetContactById(int contactId)
        {
            DefaultControls();
            var contact = db.ContactTypes.FirstOrDefault(x => x.Id == contactId);

            if (contact != null)
            {
                Name.Text = contact.Name;
                Description.Text = contact.Description;

            }
        }

        private void DefaultControls()
        {
            Name.Text = string.Empty;
            Description.Text = string.Empty;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetContactById(ContactId);

            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }
    }
}
