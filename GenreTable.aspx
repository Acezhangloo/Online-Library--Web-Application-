<%@ Page Language="C#" MasterPageFile="~/WeBSA.Master" AutoEventWireup="true" CodeBehind="GenreTable.aspx.cs"
    Inherits="WeBSA.GenreTable" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:UpdatePanel ID="updPnlGenreList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvGenreList" runat="server" CellPadding="4" PageSize="10" AutoGenerateColumns="false"
                AllowSorting="true" GridLines="Both" AllowPaging="true" PagerStyle-HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="Center" Font-Size="10pt" OnPageIndexChanging="gvGenreList_PageIndexChanging"
                OnRowDeleting="gvGenreList_RowDeleting" OnSorting="gvGenreList_OnSorting" OnRowEditing="gvGenreList_RowEditing"
                OnRowUpdating="gvGenreList_RowUpdating" OnRowCancelingEdit="gvGenreList_RowCancelingEdit">
                <EmptyDataTemplate>
                    No results found, please refine your search.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Genre ID" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="genre_id">
                        <ItemTemplate>
                            <asp:Label ID="lblGenreID" runat="server" Text='<%#Bind("genre_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Genre Type" Visible="true" ItemStyle-HorizontalAlign="Center"
                        SortExpression="genre_type" ControlStyle-Width="100pt">
                        <ItemTemplate>
                            <asp:Label ID="lblGenreType" Width="150px" Text='<%#Bind("genre_type") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbEditGenreType" Width="150px" runat="server" Text='<%#Bind("genre_type") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" HeaderText="Options" CancelText="Cancel" EditText="Edit"
                        UpdateText="Update" DeleteText="Delete" ShowCancelButton="true" ShowDeleteButton="true"
                        ShowEditButton="true" Visible="true"></asp:CommandField>
                </Columns>
            </asp:GridView>
            <muc:DeleteConfirmation ID="ucDeleteConfirmation" runat="server" Visible="true" OnDeleteConfirmed="DeleteConfirmed">
            </muc:DeleteConfirmation>
            <asp:Label ID="lblGenreOthers" Text="Default Genre Is 'Others' With Genre ID 1000."
                runat="server" Visible="true" />
            <table>
                <tr>
                    <td style='padding: 20pt 0 0 10pt'>
                        <asp:LinkButton ID="btnAddGenre" runat="server" CssClass="submitButton" Text="Add Genre"
                            OnClick="btnAddGenre_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblGenreAdded" Text="New Genre Has Been Added" runat="server" Visible="false" />
            <asp:Panel ID="pnlAddGenre" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Genre ID:
                            <asp:TextBox ID="tbGenreID" runat="server" ReadOnly="false" Width="100px"></asp:TextBox>
                            <asp:RangeValidator ID="NumOnly" runat="server" ControlToValidate="tbGenreID" Type="Integer"
                                ErrorMessage="You Must Enter a Number" MaximumValue="999" MinimumValue="000"
                                ForeColor="red"> </asp:RangeValidator>
                            <br />
                            Genre Type:
                            <asp:TextBox ID="tbGenreType" runat="server" ReadOnly="false" Width="100px"></asp:TextBox>
                            <asp:Button ID="btnGenreAdd" runat="server" Text="Add" OnClick="btnadd_Click" />
                            <asp:Label ID="lblInvalidInput" Text="This input is invalid" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
