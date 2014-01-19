<%@ Page Language="C#" MasterPageFile="~/WeBSA.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="WeBSA.Search" %>
<%@ Register Src="~/usercontrol/DeleteConfirmation.ascx" TagPrefix="muc" TagName="DeleteConfirmation" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:Panel ID="pnlEverything" runat="server" Visible="true">
        <asp:UpdatePanel ID="updpnlEverything" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td align="center">
                            Book Title:
                        </td>
                        <td>
                            <asp:Textbox ID="tbBookTitle" runat="server" width="100px" ></asp:Textbox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Author:
                        </td>
                        <td>
                            <asp:Textbox ID="tbAuthor" runat="server" width="100px"></asp:Textbox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Publication Year:
                        </td>
                        <td>
                            <asp:Textbox ID="tbPublicationYear" runat="server" Width="100px"></asp:Textbox>
                        </td>
                        <td>
                            <asp:RangeValidator ID="InvalidInput" runat="server" ControlToValidate="tbPublicationYear" Type="Integer" ErrorMessage="You can only enter 4 digit number (i.e. 2013)."
                                MaximumValue="9999" MinimumValue="1000" ForeColor="Red"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Genre:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGenre" runat="server" Width="110px" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Owner Domain:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDomain" Visible="true" runat="server" Width="110px" AppendDataBoundItems="true"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Owner:
                        </td>
                        <td>
                            <asp:TextBox ID="tbOwner" Enabled="false" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnOwnerSearch" runat="server" Visible="true" Text="Select" OnClick="btnOwnerSearch_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnSearch" runat="server" CssClass="submitButton" Text="Search" OnClick="btnSearch_Click"></asp:Button><br />
                <asp:Label ID="UnauthorizedMsg" Visible="false" runat="server" Text="We are sorry! You are not authorized to use this functionality. Please contact our administrators for full access of this site."></asp:Label>
                <br />
                <br />
                <asp:GridView ID="gvSearchList" runat="server" CellPadding="4" PageSize="100" AutoGenerateColumns="false" AllowSorting="true" GridLines="both" AllowPaging="true"
                        OnPageIndexChanging="gvSearchList_PageIndexChanging" PagerStyle-HorizontalAlign="Center" Font-Size="10pt" OnSorting="gvSearchList_OnSorting"
                        OnRowEditing="gvSearchList_RowEditing" OnRowUpdating="gvSearchList_RowUpdating" OnRowDeleting="gvSearchList_RowDeleting"  
                        OnRowCancelingEdit="gvSearchList_RowCancelingEdit" OnRowDataBound="gvSearchList_RowDatabound" OnRowCommand="gvSearchList_RowCommand">
                    <EmptyDataTemplate>
                        No results found, please refine your search.
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Book ID" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_id">
                            <ItemTemplate>
                                <asp:Label ID="lblBookID" runat="server" Text='<%#Bind("book_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Book Title" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_title">
                            <ItemTemplate>
                                <asp:Label ID="lblBookTitle" Text='<%#Bind("book_title") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEditBookTitle" runat="server" Text='<%#Bind("book_title") %>'/>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvEditBookTitle" runat="server" ControlToValidate="tbEditBookTitle" ErrorMessage="Value Required" 
                                 Font-Size="X-Small" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Author" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_author">
                            <ItemTemplate>
                                <asp:Label ID="lbBookAuthor" runat="server" Text='<%#Bind("book_author") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEditBookAuthor" runat="server" Text='<%#Bind("book_author") %>' />
                                <br />
                                <asp:RequiredFieldValidator ID="rfvEditBookAuthor" runat="server" ControlToValidate="tbEditBookAuthor" ErrorMessage="Value Required" 
                                 Font-Size="X-Small" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Publication Year" Visible="true" ItemStyle-HorizontalAlign="Center" SortExpression="publication_year">
                            <ItemTemplate>
                                <asp:Label ID="lblPublicationYear" runat="server" Text='<%#Bind("publication_year") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEditPublicationYear" runat="server" Text='<%#Bind("publication_year") %>' />
                                <br />
                                <asp:RequiredFieldValidator ID="rfvEditPubYear" runat="server" ControlToValidate="tbEditPublicationYear" ErrorMessage="Value Required" 
                                 Font-Size="X-Small" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="InvalidEditInput" runat="server" ControlToValidate="tbEditPublicationYear" Type="Integer" 
                                 ErrorMessage="You can only enter 4 digit number" MaximumValue="9999" MinimumValue="1000" Font-Size="X-Small" Display="Dynamic" ForeColor="Red" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Genre" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_genre">
                            <ItemTemplate>
                                <asp:Label ID="lblBookGenre" runat="server" Text='<%#Bind("book_genre") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblGenre_Description" runat="server" Text='<%#Bind("book_genre") %>' Visible="false" />
                                <asp:DropDownList ID="ddlEditBookGenre" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Owner Domain" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_owner_domain">
                            <ItemTemplate>
                                <asp:Label ID="lblBookOwnerDoamin" runat="server" Text='<%#Bind("book_owner_domain") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Owner" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="book_owner">
                            <ItemTemplate>
                                <asp:Label ID="lblBookOwner" runat="server" Text='<%#Bind("book_owner") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Uploaded" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="date_uploaded">
                            <ItemTemplate>
                                <asp:Label ID="lblDateUploaded" runat="server" Text='<%#Bind("date_uploaded") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Modified" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="date_modified">
                            <ItemTemplate>
                                <asp:Label ID="lblDateModified" runat="server" Text='<%#Bind("date_modified") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Link" HeaderText="Options" CancelText="Cancel" EditText="Edit" UpdateText="Update" DeleteText="Delete"
                             ShowCancelButton="true" ShowDeleteButton="true" ShowEditButton="true" Visible="true">
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="View" Visible="true" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnView" Text="View" CommandName="view" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Visible="true"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Download" Visible="true" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDownload" Text="Download" CommandName="download" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Visible="true"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <muc:OwnerSearch ID="ucOwnerSearch" runat="server" Visible="true" OnOwnerSelected="ucOwnerSearch_OwnerSelected"/>
                <muc:DeleteConfirmation ID="ucDeleteConfirmation" runat="server" Visible="true" OnDeleteConfirmed="DeleteConfirmed"></muc:DeleteConfirmation>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
