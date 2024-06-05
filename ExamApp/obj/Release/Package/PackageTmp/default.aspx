<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ExamApp._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Hoşgeldiniz, <%= User.Identity.Name.ToString() %>!</h2>
</asp:Content>
