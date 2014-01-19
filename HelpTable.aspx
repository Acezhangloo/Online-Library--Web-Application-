<%@ Page Language="C#" MasterPageFile="~/WeBSA.Master" AutoEventWireup="true" CodeBehind="HelpTable.aspx.cs"
    Inherits="WeBSA.HelpTable" ValidateRequest="false"%>

<%@ Register Src="~/usercontrol/DeleteConfirmation.ascx" TagPrefix="muc" TagName="DeleteConfirmation" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:UpdatePanel ID="updPnlHelp" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView ID="gvHelpTable" runat="server" CellPadding="4" PageSize="10" AutoGenerateColumns="false"
                AllowSorting="true" GridLines="Both" AllowPaging="true" PagerStyle-HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="Center" Font-Size="10pt" OnPageIndexChanging="gvHelpTable_PageIndexChanging"
                OnRowDeleting="gvHelpTable_RowDeleting" OnSorting="gvHelpTable_OnSorting" OnRowEditing="gvHelpTable_RowEditing"
                OnRowUpdating="gvHelpTable_RowUpdating" OnRowCancelingEdit="gvHelpTable_RowCancelingEdit">
                <EmptyDataTemplate>
                    No results found, please refine your search.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Help ID" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="help_id">
                        <ItemTemplate>
                            <asp:Label ID="lblHelpID" Text='<%#Bind("help_id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Help Title" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="help_title" ControlStyle-Width="100pt">
                        <ItemTemplate>
                            <asp:Label ID="lblHelpTitle" Text='<%#Bind("help_title") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbEditHelpTitle" Width="150px" runat="server" Text='<%#Bind("help_title") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="description" ControlStyle-Width="500px">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" Width="150px" height="30px" Text='<%# Server.HtmlDecode((string)Eval("help_description")) %> '
                                 runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbEditDescription" Width="300px" height ="30px" runat="server" Text='<%# Server.HtmlDecode((string)Eval("help_description")) %>' TextMode="MultiLine"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" HeaderText="Options" CancelText="Cancel" EditText="Edit"
                        UpdateText="Update" DeleteText="Delete" ShowCancelButton="true" ShowDeleteButton="true"
                        ShowEditButton="true" Visible="true"></asp:CommandField>
                </Columns>
            </asp:GridView>
            <muc:DeleteConfirmation ID="ucDeleteConfirmation" runat="server" Visible="true" OnDeleteConfirmed="DeleteConfirmed">
            </muc:DeleteConfirmation>
            <table>
                <tr>
                    <td style='padding: 20pt 0 0 10pt'>
                        <asp:LinkButton ID="btnAddHelp" runat="server" CssClass="submitButton" Text="Add Help Section"
                            OnClick="btnAddHelp_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblHelpAdded" Text="New Help Section Has Been Added" runat="server"
                Visible="false" />
            <asp:Panel ID="pnlAddHelp" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Help ID:
                            <asp:TextBox ID="tbHelpID" runat="server" ReadOnly="false" Width="100px"></asp:TextBox>
                            <asp:RangeValidator ID="NumOnly" runat="server" ControlToValidate="tbHelpID" Type="Integer"
                                ErrorMessage="You Must Enter a Number" MaximumValue="999" MinimumValue="000"
                                ForeColor="red"> </asp:RangeValidator>
                            &nbsp Help Title:
                            <asp:TextBox ID="tbHelpTitle" runat="server" ReadOnly="false" Width="100px"></asp:TextBox>
                            <br />
                            <br />
                            <br />
                            Description:
                            <br />
                            <asp:TextBox ID="tbDescription" runat="server" ReadOnly="false" Width="500px" height="300px" TextMode="MultiLine"></asp:TextBox>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                            <asp:Label ID="lblInvalidInput" Text="This input is invalid" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
