<%@  Language="C#" MasterPageFile="WeBSA.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="WeBSA._Default" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:Panel ID="pnlUnauthorized" runat="server" Visible="false">
        <asp:Label ID="lblUnauthorized" runat="server" Text="We are sorry! You are not one of our registered users for this site. You may experience limited functionalities on the other pages. Please contact our administrators for full access of this site."
            Visible="true"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlAuthorized" runat="server">
        Welcome back,
        <asp:Label ID="lblLogonUser" runat="server" Visible="true"></asp:Label>
    </asp:Panel>
    
    <table>
        <tr>
            <td width="50">
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <img src="/images/W.jpg" alt="Pulpit rock" width="1000" height="500"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
