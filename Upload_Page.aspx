<%@ Page Title="Upload Page" Language="C#" MasterPageFile="~/WeBSA.Master" AutoEventWireup="true"
    CodeBehind="Upload_Page.aspx.cs" Inherits="WeBSA.Upload_Page" %>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolderWeBSA">
    <asp:Panel ID="pnlAuthorized" runat="server" Visible="true">
<h2>Upload</h2> 

 Please locate the book from your local computer<br />
<asp:FileUpload ID="FileUpload" runat="server" Height="19px" 
        Width="300px" style="margin-left: 0px"/>
<asp:Label ID="wordOrPdfOnly" Text="Only Word or PDF file can be uploaded." runat="server" Visible="false" ForeColor="red"/>
<br />
<asp:RequiredFieldValidator ID="Nofile" runat="server" ErrorMessage="Please select a file." ControlToValidate="FileUpload" ForeColor="red">
</asp:RequiredFieldValidator>
<br />
 Please locate the cover page of the book from your local computer<br />
 <asp:FileUpload ID="coverPage" runat="server" Height="19px"
        Width="300px" style="margin-left: 0px"/>
<asp:Label ID="jpgPngOnly" Text="Only jpg and png file can be uploaded." runat="server" Visible="false" ForeColor="red"/>
<br />
<br />
        <table>
            <tr>
                <td align="center">
                    Book Title*:
                </td>
                <td>
                    <asp:Textbox ID="BookTitle" runat="server" width="100px"></asp:Textbox>
                    <asp:RequiredFieldValidator ID="Book_title_input" runat="server" ErrorMessage="Please input your Book Title." ControlToValidate="BookTitle" ForeColor="red">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    Author:
                </td>
                <td>
                    <asp:Textbox ID="Author" runat="server" width="100px"></asp:Textbox>
                    <asp:RangeValidator ID="Right_Name" runat="server" ControlToValidate="Author" Type="string" ErrorMessage="Please input right name which only contain letters"
                    MaximumValue="z" MinimumValue="A" ForeColor="red"> </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    Publication Year:
                </td>
                <td>
                    <asp:Textbox ID="PublicationYear" runat="server" Width="100px"></asp:Textbox>
                    <asp:RangeValidator ID="NumOnly" runat="server" ControlToValidate="PublicationYear" Type="Integer" ErrorMessage="You mush enter 4 digit number in this box. Example: 2013"
                    MaximumValue="9999" MinimumValue="1000" ForeColor="red"> </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    Genre:
                </td>
                <td>
                    <asp:DropDownList ID="GenreList" runat="server" Width="110px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <caption>
                
            </caption>
        </table>
       
         Must enter fields with *<br />
        <asp:Button ID="Upload_Button" runat="server" onclick="Upload_Click" Text="Upload" visible="true"/> 
        <asp:Label ID="successfulInfo" Text="The file is uploaded successfully" runat="server" Visible="false" ForeColor="red" />
        <asp:Label ID="authenticationInfor" Text="We are sorry! You are not authorized to use this functionality. Please contact our administrators for full access of this site." runat="server" Visible="false" ForeColor="red"/> 
        <br />
         <asp:Label ID="rightPath" Text="" runat="server" Visible="false" ForeColor="red" />
         <br />
         <asp:Label ID="imagePath" Text="" runat="server" Visible="false" ForeColor="red" />
        </asp:Panel>
  </asp:Content>











