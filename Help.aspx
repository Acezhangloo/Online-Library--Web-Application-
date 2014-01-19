<%@ Page Language="C#" MasterPageFile="WeBSA.master" AutoEventWireup="true" CodeBehind="Help.aspx.cs"
    Inherits="WeBSA.Help" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:GridView ID="gvHelpList" runat="server" CellPadding="4" PageSize="10" AutoGenerateColumns="false"
         GridLines="Both" AllowPaging="true" PagerStyle-HorizontalAlign="Center"
        HeaderStyle-HorizontalAlign="Center" Font-Size="10pt" OnPageIndexChanging="gvHelpList_PageIndexChanging">
        <EmptyDataTemplate>
            No results found, please refine your search.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Help ID" Visible="true" ItemStyle-HorizontalAlign="Center"
                SortExpression="help_id">
                <ItemTemplate>
                    <asp:Label ID="lblHelpID" runat="server" Text='<%#Bind("help_ID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Help Title" Visible="true" ItemStyle-HorizontalAlign="Center"
                SortExpression="help_title">
                <ItemTemplate>
                    <asp:Label ID="lblHelpTitle" Width="300px" Text='<%#Bind("help_title") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" Visible="true" ItemStyle-HorizontalAlign="Center"
                SortExpression="description">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" Width="600px" Text='<%#Bind("help_description") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
