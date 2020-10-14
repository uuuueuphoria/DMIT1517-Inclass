using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBSystem.BLL;
using DBSystem.ENTITIES;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

namespace WebApp.Exercises
{
    public partial class Exercise08CRUD : System.Web.UI.Page
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
                BindTeamList();
                BindGuardianList();
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
                    PlayerController sysmgr = new PlayerController();
                    Player info = null;
                    info = sysmgr.FindByPKID(int.Parse(pid));
                    if (info == null)
                    {
                        ShowMessage("Player Record is not in Database.", "alert alert-info");
                        Clear(sender, e);
                    }
                    else
                    {
                        TextBox1.Text = info.PlayerID.ToString(); //NOT NULL in Database
                        TextBox2.Text = info.FirstName; //NOT NULL in Database
                        TextBox3.Text = info.LastName;
                        GuardianList.SelectedValue = info.GuardianID.ToString();
                        TeamList.SelectedValue = info.TeamID.ToString();
                        TextBox6.Text = info.Age.ToString();
                        TextBox7.Text = info.Gender;
                        TextBox8.Text = info.AlbertaHealthCareNumber;
                        if (string.IsNullOrEmpty(info.MedicalAlertDetails)) //NULL in Database
                        {
                            TextBox9.Text = "";
                        }
                        else
                        {
                            TextBox9.Text = info.MedicalAlertDetails;
                        }
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

        protected void BindGuardianList()
        {
            try
            {
                GuardianController sysmgr = new GuardianController();
                List<Guardian> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.Name.CompareTo(y.Name));
                GuardianList.DataSource = info;
                GuardianList.DataTextField = nameof(Guardian.Name);
                GuardianList.DataValueField = nameof(Guardian.GuardianID);
                GuardianList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                GuardianList.Items.Insert(0, myitem);
            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }
        protected void BindTeamList()
        {
            try
            {
                TeamController sysmgr = new TeamController();
                List<Team> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.TeamName.CompareTo(y.TeamName));
                TeamList.DataSource = info;
                TeamList.DataTextField = nameof(Team.TeamName);
                TeamList.DataValueField = nameof(Team.TeamID);
                TeamList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                TeamList.Items.Insert(0, myitem);
            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }


        }
        protected void Back_Click(object sender, EventArgs e)
        {
            if (pagenum == "50")
            {
                Response.Redirect("50ASPControlsMultiRecordDropdownToSingleRecord.aspx");
            }
            else if (pagenum == "60")
            {
                Response.Redirect("60ASPControlsMultiRecDropToCustGridViewToSingleRec.aspx");
            }
            else if (pagenum == "70")
            {
                Response.Redirect("70ASPControlsMultiRecDropToDropToSingleRec.aspx");
            }
            else if (pagenum == "80")
            {
                Response.Redirect("80ASPControlsPartialStringSearchToCustGridViewToSingleRec.aspx");
            }
            else if (pagenum == "90")
            {
                Response.Redirect("90ASPControlsPartialStringSearchToDropToSingleRec.aspx");
            }
            else if (pagenum == "08")
            {
                Response.Redirect("Exercise08.aspx");
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void Clear(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            GuardianList.ClearSelection();
            TeamList.ClearSelection();
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
        }

        protected bool Validation(object sender, EventArgs e)
        {
            int Age = 0;
            char gender = '0';
            if (string.IsNullOrEmpty(TextBox2.Text))
            {
                ShowMessage("First Name is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(TextBox3.Text))
            {
                ShowMessage("Last Name is required", "alert alert-info");
                return false;
            }
            else if (GuardianList.SelectedValue == "0")
            {
                ShowMessage("Guardian is required", "alert alert-info");
                return false;
            }
            else if (TeamList.SelectedValue == "0")
            {
                ShowMessage("Team is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(TextBox6.Text))
            {
                ShowMessage("Age is required", "alert alert-info");
                return false;
            }
            else if (int.TryParse(TextBox6.Text, out Age))
            {
                if (Age < 6 || Age > 14)
                {
                    ShowMessage("Player Age must be between 6 and 14", "alert alert-info");
                    return false;
                }
            }
            else
            {
                ShowMessage("Age must be an integer", "alert alert-info");
                return false;
            }
             if (string.IsNullOrEmpty(TextBox7.Text))
            {
                ShowMessage("Gender is required", "alert alert-info");
                return false;
            }
            else if (char.TryParse(TextBox7.Text, out gender))
            {
                if (char.ToUpper(gender)!='F' && char.ToUpper(gender) != 'M')
                {
                    ShowMessage("Player Gender must be either M or F", "alert alert-info");
                    return false;
                }
            }
             else
            {
                ShowMessage("Gender must be a letter", "alert alert-info");
                return false;
            }
            if (string.IsNullOrEmpty(TextBox8.Text))
            {
                ShowMessage("Alberta Health Care Number is required", "alert alert-info");
                return false;
            }
            else
            {
                Match match1 = Regex.Match(TextBox8.Text, @"^[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$");
                if (!match1.Success)
                {
                    ShowMessage("Alberta Health Care Number must be like 10 digits", "alert alert-info");
                    return false;
                }
            }
            return true;
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    Player person = new Player();
                    person.FirstName = TextBox2.Text.Trim(); //NOT NULL in Database
                    person.LastName = TextBox3.Text.Trim();
                    person.GuardianID = int.Parse(GuardianList.SelectedValue);
                    person.TeamID = int.Parse(TeamList.SelectedValue);
                    person.Age = int.Parse(TextBox6.Text);
                    person.Gender = TextBox7.Text;
                    person.AlbertaHealthCareNumber = TextBox8.Text;
                    if (string.IsNullOrEmpty(TextBox9.Text))
                    {
                        person.MedicalAlertDetails = null;
                    }
                    else
                    {
                        person.MedicalAlertDetails = TextBox9.Text;
                    }
                    int newID = sysmgr.Add(person);
                    TextBox1.Text = newID.ToString();
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
        protected void Update_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    Player person = new Player();
                    person.PlayerID = int.Parse(TextBox1.Text);
                    person.FirstName = TextBox2.Text.Trim();
                    person.LastName = TextBox3.Text.Trim();
                    person.GuardianID = int.Parse(GuardianList.SelectedValue);
                    person.TeamID = int.Parse(TeamList.SelectedValue);
                    person.Age = int.Parse(TextBox6.Text);
                    person.Gender = TextBox7.Text;
                    person.AlbertaHealthCareNumber = TextBox8.Text;
                    if (string.IsNullOrEmpty(TextBox9.Text))
                    {
                        person.MedicalAlertDetails = null;
                    }
                    else
                    {
                        person.MedicalAlertDetails = TextBox9.Text;
                    }
                    int rowsaffected = sysmgr.Update(person);
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
        protected void Delete_Click(object sender, EventArgs e)
        {
            var isValid = true;
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    int rowsaffected = sysmgr.Delete(int.Parse(TextBox1.Text));
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
                    AddButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }

        }
    }
}
