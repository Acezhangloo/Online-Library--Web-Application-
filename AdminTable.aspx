<%@ Page Language="C#" MasterPageFile="~/WeBSA.Master" AutoEventWireup="true" CodeBehind="AdminTable.aspx.cs"
    Inherits="WeBSA.AdminTable" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:UpdatePanel ID="updPnlAdminList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvAdminList" runat="server" CellPadding="4" PageSize="10" AutoGenerateColumns="false"
                AllowSorting="true" GridLines="Both" AllowPaging="true" PagerStyle-HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="Center" Font-Size="10pt" OnPageIndexChanging="gvAdminList_PageIndexChanging"
                OnRowDeleting="gvAdminList_RowDeleting" OnSorting="gvAdminList_OnSorting" OnRowEditing="gvAdminList_RowEditing"
                OnRowUpdating="gvAdminList_RowUpdating" OnRowCancelingEdit="gvAdminList_RowCancelingEdit"
                OnRowDataBound="gvAdminList_RowDatabound">
                <EmptyDataTemplate>
                    No results found, please refine your search.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Admin ID" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="admin_id">
                        <ItemTemplate>
                            <asp:Label ID="lblAdminID" runat="server" Text='<%#Bind("admin_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admin Domain" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="admin_domain">
                        <ItemTemplate>
                            <asp:Label ID="lblAdminDomain" Width="150px" Text='<%#Bind("admin_domain") %>' runat="server" />
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admin Name" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="admin_name">
                        <ItemTemplate>
                            <asp:Label ID="lblAdminName" Width="150px" Text='<%#Bind("admin_name") %>' runat="server" />
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admin Role" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="admin_role">
                        <ItemTemplate>
                            <asp:Label ID="lblAdminRole" Width="150px" runat="server" Text='<%#Bind("admin_role") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblRole_Description" runat="server" Text='<%#Bind("admin_role") %>'
                                Visible="false" />
                            <asp:DropDownList ID="ddlEditAdminRole" Width="150px" runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" HeaderText="Options" CancelText="Cancel" EditText="Edit"
                        UpdateText="Update" DeleteText="Delete" ShowCancelButton="true" ShowDeleteButton="true"
                        ShowEditButton='true' Visible="true"></asp:CommandField>
                </Columns>
            </asp:GridView>
            <muc:OwnerSearch ID="ucOwnerSearch" runat="server" Visible="true" OnOwnerSelected="ucOwnerSearch_OwnerSelected" />
            <muc:DeleteConfirmation ID="ucDeleteConfirmation" runat="server" Visible="true" OnDeleteConfirmed="DeleteConfirmed">
            </muc:DeleteConfirmation>
            <table>
                <tr>
                    <td style='padding: 20pt 0 0 10pt'>
                        <asp:LinkButton ID="btnAddAdmin" runat="server" CssClass="submitButton" Text="Add Admin"
                            OnClick="btnAddAdmin_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblAdminAdded" Text="New Admin Has Been Added" runat="server" Visible="false" />
            <asp:Panel ID="pnlAddAdmin" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Admin Domain
                            <asp:TextBox ID="tbAdminDomain" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            Admin Name
                            <asp:TextBox ID="tbAdminName" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                            <asp:Button ID="btnAdminSearch" runat="server" Text="Search Admin" OnClick="btnAdminSearch_Click" />
                        </td>
                        <td>
                            Admin Role
                            <asp:DropDownList ID="ddlAdminRole" runat="server" Width="110px" AppendDataBoundItems="true">
                               
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblInvalidInput" Text="This input is invalid" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
