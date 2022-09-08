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
    public partial class Form3 : Form
    {
        CRUD_ConstSoftwareEntities db = new CRUD_ConstSoftwareEntities();

        List<string> Msg = new List<string>();

        int ClientId = 0;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            GetClient();
        }

        private void GetClient()
        {
            var client = (from a in db.ClientTypes
                           select new
                           {
                               a.Id,
                               a.Name,
                               a.Enabled,
                               a.CreatedDate
                           }).ToList();
            dgvClient.DataSource = client;
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

        private void SaveForm()
        {
            var client = new ContactType();
            client.Name = Name.Text;
            client.Description = Description.Text;

            client.Enabled = true;
            client.CreatedDate = DateTime.Now;

            db.ContactTypes.Add(client);

            var clientSaved = db.SaveChanges() > 0;
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

        private void dgvClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ClientId = 0;

            if (!string.IsNullOrEmpty(dgvClient.SelectedRows[0].Cells["Id"].Value.ToString()))
            {
                dgvClient.SelectedRows[0].Cells["Id"].Value.ToString();
                btnUpdate.Visible = true;
                btnDelete.Visible = true;

            }
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void GetClientById(int clientId)
        {
            DefaultControls();
            var client = db.ContactTypes.FirstOrDefault(x => x.Id == clientId);

            if (client != null)
            {
                Name.Text = client.Name;
                Description.Text = client.Description;

            }
        }

        private void DefaultControls()
        {
            Name.Text = string.Empty;
            Description.Text = string.Empty;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetClientById(ClientId);

            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }
    }
    }
