using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBSystem.BLL;
using DBSystem.ENTITIES;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;


namespace WebApp.Exercises
{
    public partial class ProjectCRUD : System.Web.UI.Page
    {
        static string pagenum = "";
        static string pid = "";
        static string add = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pagenum = Request.QueryString["page"];
                pid = Request.QueryString["pid"];
                add = Request.QueryString["add"];
                BindSchoolList();
                if (string.IsNullOrEmpty(pid))
                {
                    Response.Redirect("~/Default.aspx");
                }
                else if (add == "yes")
                {
                    UpdateButton.Enabled = false;
                    DeleteButton.Enabled = false;
                }
                else
                {
                    AddButton.Enabled = false;
                    ProgramController sysmgr = new ProgramController();
                    Programs info = null;
                    info = sysmgr.FindByPKID(int.Parse(pid));
                    if (info == null)
                    {
                        ShowMessage("Record is not in Database.", "alert alert-info");
                        Clear(sender, e);
                    }
                    else
                    {
                        ID.Text = info.ProgramID.ToString(); //NOT NULL in Database
                        ProgramName.Text = info.ProgramName; //NOT NULL in Database
                        if (info.DiplomaName == null) //NULL in Database
                        {
                            DiplomaName.Text = "";
                        }
                        else
                        {
                            DiplomaName.Text = info.DiplomaName;
                        }
                            SchoolList.SelectedValue = info.SchoolCode;
                            Tuition.Text = string.Format("{0:0.00}", info.Tuition.Value);
                            InternationalTuition.Text = string.Format("{0:0.00}", info.InternationalTuition.Value);
                    }
                }
            }
        }
        protected Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
        protected void ShowMessage(string message, string cssclass)
        {
            MessageLabel.Attributes.Add("class", cssclass);
            MessageLabel.InnerHtml = message;
        }
        protected void BindSchoolList()
        {
            try
            {
                SchoolController sysmgr = new SchoolController();
                List<Schools> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.SchoolCode.CompareTo(y.SchoolCode));
                SchoolList.DataSource = info;
                SchoolList.DataTextField = nameof(Schools.SchoolName);
                SchoolList.DataValueField = nameof(Schools.SchoolCode);
                SchoolList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                SchoolList.Items.Insert(0, myitem);
            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }
        protected bool Validation(object sender, EventArgs e)
        {
            double unitprice = 0;
            double international = 0;
            if (string.IsNullOrEmpty(ProgramName.Text))
            {
                ShowMessage("Program Name is required", "alert alert-info");
                return false;
            }
            else if (SchoolList.SelectedValue == "0")
            {
                ShowMessage("School Name is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(Tuition.Text))
            {
                ShowMessage("Tuition is required", "alert alert-info");
                return false;
            }
            else if (double.TryParse(Tuition.Text, out unitprice))
            {
                if (unitprice < 0.00 || unitprice > 7200.00)
                {
                    ShowMessage("Unit Price must be between $0.00 and $7200.00", "alert alert-info");
                    return false;
                }
            }
            else
            {
                ShowMessage("Tuition Price must be a real number", "alert alert-info");
                return false;
            }
            if (string.IsNullOrEmpty(InternationalTuition.Text))
            {
                ShowMessage("International Tuition is required", "alert alert-info");
                return false;
            }
            else if (double.TryParse(InternationalTuition.Text, out international))
            {
                if (international < 0.00 || international > 12000.00)
                {
                    ShowMessage("International Tuition must be between $0.00 and $12000.00", "alert alert-info");
                    return false;
                }
            }
            else
            {
                ShowMessage("International Tuition must be a real number", "alert alert-info");
                return false;
            }
            return true;
        }
        protected void Back_Click(object sender, EventArgs e)
        {
            if (pagenum == "55")
            {
                Response.Redirect("Project.aspx");
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void Clear(object sender, EventArgs e)
        {
            ID.Text = "";
            ProgramName.Text = "";
            DiplomaName.Text = "";
            SchoolList.ClearSelection();
            Tuition.Text = "";
            InternationalTuition.Text = "";
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    ProgramController sysmgr = new ProgramController();
                    Programs item = new Programs();
                    item.ProgramID = int.Parse(ID.Text);
                    item.ProgramName = ProgramName.Text.Trim();
                    if (DiplomaName.Text=="")
                    {
                        item.DiplomaName = null;
                    }
                    else
                    {
                        item.DiplomaName = DiplomaName.Text;
                    }
                    item.SchoolCode = SchoolList.SelectedValue;
                    item.Tuition = decimal.Parse(Tuition.Text);
                    item.InternationalTuition = decimal.Parse(InternationalTuition.Text);
                    int rowsaffected = sysmgr.Update(item);
                    if (rowsaffected > 0)
                    {
                        ShowMessage("Record has been UPDATED", "alert alert-success");
                    }
                    else
                    {
                        ShowMessage("Record was not found", "alert alert-warning");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    ProgramController sysmgr = new ProgramController();
                    Programs item = new Programs();
                    item.ProgramName = ProgramName.Text;
                    if (string.IsNullOrEmpty(DiplomaName.Text))
                    {
                        item.DiplomaName = null;
                    }
                    else
                    {
                        item.DiplomaName = DiplomaName.Text;
                    }
                    item.SchoolCode = SchoolList.SelectedValue;
                    item.Tuition = decimal.Parse(Tuition.Text);
                    item.InternationalTuition = decimal.Parse(InternationalTuition.Text);
                    int newID = sysmgr.Add(item);
                    ID.Text = newID.ToString();
                    ShowMessage("Record has been ADDED", "alert alert-success");
                    AddButton.Enabled = false;
                    UpdateButton.Enabled = true;
                    DeleteButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            var isValid = true;
            if (isValid)
            {
                try
                {
                    ProgramController sysmgr = new ProgramController();
                    int rowsaffected = sysmgr.Delete(int.Parse(ID.Text));
                    if (rowsaffected > 0)
                    {
                        ShowMessage("Record has been DELETED", "alert alert-success");
                        Clear(sender, e);
                    }
                    else
                    {
                        ShowMessage("Record was not found", "alert alert-warning");
                    }
                    UpdateButton.Enabled = false;
                    DeleteButton.Enabled = false;
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
    }
}
