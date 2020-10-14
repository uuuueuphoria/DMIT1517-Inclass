<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="WebApp.Exercises.Project" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Project Page 01</h1>
     <div class="row">
        <div class="col-md-4 text-right">
            <asp:Label ID="Label1" runat="server" Text="School "></asp:Label>
        </div>
        <div class="col-md-8 text-left">
            <asp:DropDownList ID="List01" runat="server"></asp:DropDownList>&nbsp;&nbsp;
            <asp:Button ID="Fetch01" runat="server" Text="Programs?" 
                 CausesValidation="false" OnClick="Fetch_Click01"/>
            &nbsp;&nbsp;
        <asp:Button ID="ButtonAdd" runat="server" Text="Add" 
             CausesValidation="false" OnClick="Add_Click"/>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 text-right">
            <asp:Label ID="Label2" runat="server" Text="Programs "></asp:Label>
        </div>
        <div class="col-md-8 text-left">
            <asp:DropDownList ID="List02" runat="server"></asp:DropDownList>&nbsp;&nbsp;
            <asp:Button ID="Fetch02" runat="server" Text="Select" 
                 CausesValidation="false" OnClick="Fetch_Click02"/>
        </div>
    </div>
    <div>
        <br /><br />
        <asp:Label ID="MessageLabel" runat="server" ></asp:Label>
        <br />
        <%--<asp:GridView ID="List02" runat="server"></asp:GridView>--%>
    </div>
</asp:Content>
