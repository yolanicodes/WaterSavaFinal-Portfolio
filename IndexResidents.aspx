<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndexResidents.aspx.cs" Inherits="WaterSavaFinal.WebForm1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<h2>Welcome, Yolani</h2>
    <main>
<div id="leftContainer">
    <h3>Track consumption</h3>
    <div id="trackConsumption">
        <label for="activityDD">Activity:</label>
        <asp:DropDownList ID="activityDD" runat="server" DataSourceID="SqlDataSource3" DataTextField="Activity_Description" DataValueField="ActivityID" CssClass="activityDD">
            <asp:ListItem>Select Activity...</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\kwaWaterSava.accdb" ProviderName="System.Data.OleDb" SelectCommand="SELECT DISTINCT [Activity_Description], [ActivityID] FROM [Activity] WHERE Property_ID = @PropertyID">
            <SelectParameters>
        <asp:Parameter Name="PropertyID" Type="Int32" />
    </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label ID="Label1" runat="server" Text="Amount(in litres):"></asp:Label>
        <asp:TextBox ID="txtCapacity" runat="server" TextMode="Number" CssClass="txtCapacity"></asp:TextBox>
        <asp:Button ID="btnSaveCons" runat="server" Text="Save" onclick="btnSaveCons_Click" CssClass="btnSaveCons"/>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label> 
        <asp:Panel ID="panelTipArticles" runat="server"></asp:Panel>
        
    </div>
    
</div>
       <div id="RightContainenr">
           <br />
            <h2>Overview</h2>
           <hr />
            <h3>Water Consumption</h3>
            <div class="dailyTrackers">
            <asp:Chart ID="Chart1" runat="server" Width="600px" Height="400px" BackColor="Transparent">
    <ChartAreas>
        <asp:ChartArea Name="MainChartArea" BackColor="#DAE8ED"></asp:ChartArea>
    </ChartAreas>
    <Series>
        <asp:Series Name="Series1" XValueMember="Activity_Description" YValueMembers="TotalAmount" ChartType="Pie"></asp:Series>
    </Series>
        </asp:Chart>
</div>
       
        </div>

       <div class="left-container">
           <hr />
<%--    <h3>Water Consumption Tips</h3>--%>

 <%-- <div id="websiteCarousel" class="carousel slide" data-ride="carousel">
    <div class="carousel-inner horizontal-carousel">
        <div class="carousel-item active">
            <iframe src="https://sites.psu.edu/marabelleolivia/2021/03/25/why-should-we-turn-off-the-tap-water-when-brushing-our-teeth/" style="width: 100%; height: 400px;" frameborder="0" scrolling="yes"></iframe>
        </div>
        <div class="carousel-item">
            <iframe src="https://www.activesustainability.com/water/learnsustainability-5-minutes-shower/#:~:text=Saving%20water%20in%20the%20domestic,of%20water%20as%20a%20bath%3F" style="width: 100%; height: 400px;" frameborder="0" scrolling="yes"></iframe>
        </div>
        <div class="carousel-item">
            <iframe src="https://www.wwf.org.za/?20281/save-water" style="width: 100%; height: 400px;" frameborder="0" scrolling="yes"></iframe>
        </div>
        <div class="carousel-item">
            <iframe src="https://www.thinkh2onow.com/water_conservation_tips_home.php" style="width: 100%; height: 400px;" frameborder="0" scrolling="yes"></iframe>
        </div>
        <div class="carousel-item">
            <iframe src="https://besigebreintjies.co.za/water-saving-tips/?srsltid=AfmBOophtCXXMFEoxFUYAdp1aBbgDobg9jpl2v8j6OU3hIwTQBGZe2ac" style="width: 100%; height: 400px;" frameborder="0" scrolling="yes"></iframe>
        </div>
    </div>
    <a class="carousel-control-prev" href="#websiteCarousel" role="button" data-slide="prev"> <span class="carousel-control-prev-icon" aria-hidden="true"></span> <span class="sr-only">Previous</span> </a> 
      
      <a class="carousel-control-next" href="#websiteCarousel" role="button" data-slide="next"> <span class="carousel-control-next-icon" aria-hidden="true"></span> <span class="sr-only">Next</span> </a>
</div>--%>
</div>
    </main>

</asp:Content>
