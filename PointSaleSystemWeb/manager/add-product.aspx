<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.Master" AutoEventWireup="true" CodeBehind="add-product.aspx.cs" Inherits="PointSaleSystemWeb.manager.add_product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Point of Sale - Product Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="page-content">
        <div class="container">
            <div id="page-title">
                <h2>Product</h2>
                <p>
                    <asp:Label ID="lblChangePass" Text="Add Product Details." runat="server" />
                </p>
            </div>
            <div class="panel">
                <div class="panel-body">
                    <div class="example-box-wrapper">
                        <div class="row">
                            <div class="col-sm-8 col-sm-offset-2">
                                <div class="content-box">
                                    <asp:Panel ID="alertErrorPanel" Display="Dynamic" Visible="false" Height="60px" runat="server">
                                        <div class="alert alert-close alert-danger">
                                            <a href="#" id="clsAlertError" title="Close" class="glyph-icon alert-close-btn icon-remove" runat="server" onserverclick="clsAlertSuccess_ServerClick" causesvalidation="false"></a>
                                            <div class="bg-red alert-icon">
                                                <i class="glyph-icon icon-times"></i>
                                            </div>
                                            <div class="alert-content">
                                                <h4 class="alert-title">
                                                    <asp:Label ID="alertErrorTitle" Text="Success message title" runat="server"></asp:Label>
                                                </h4>
                                                <p>
                                                    <asp:Label ID="alertErrorMessage" Text="Information message box using the" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="alertSuccessPanel" Display="Dynamic" Visible="false" Height="60px" runat="server">
                                        <div class="alert alert-close alert-success">
                                            <a href="#" id="clsAlertSuccess" title="Close" class="glyph-icon alert-close-btn icon-remove" runat="server" onserverclick="clsAlertSuccess_ServerClick" causesvalidation="false"></a>
                                            <div class="bg-green alert-icon">
                                                <i class="glyph-icon icon-check"></i>
                                            </div>
                                            <div class="alert-content">
                                                <h4 class="alert-title">
                                                    <asp:Label ID="alertSuccessTitle" Text="Success message title" runat="server"></asp:Label>
                                                </h4>
                                                <p>
                                                    <asp:Label ID="alertSuccessMessage" Text="Information message box using the" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <h3 class="content-box-header bg-white text-center">Enter Details of Product</h3>
                                    <div class="content-box-wrapper">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal bordered-row">
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Product Name:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtProductName" CssClass="form-control" placeholder="Enter Product Name" runat="server" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtProductName"
                                                                ErrorMessage="Enter Product Name"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                                ControlToValidate="txtProductName"
                                                                ForeColor="Red"
                                                                ValidationExpression="[a-zA-Z / -]*$"
                                                                ErrorMessage="Enter Aplhabets"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Category:</label>
                                                        <div class="col-sm-6 ">
                                                            <div>
                                                                <asp:DropDownList ID="ddlCategory" CssClass="custom-select form-control" runat="server">
                                                                    <asp:ListItem Text="-- Select Category --" Value="0" Selected="True" />
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" runat="server"
                                                                    ControlToValidate="ddlCategory"
                                                                    ErrorMessage="Select Category"
                                                                    InitialValue="0"
                                                                    Display="Dynamic">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Product Quantity:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtQuantity" CssClass="form-control" placeholder="Enter Product Quantity" MaxLength="3" runat="server" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtQuantity"
                                                                ErrorMessage="Enter Product Quantity"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server"
                                                                ControlToValidate="txtQuantity"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 ]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Cost Price:</label>
                                                        <div class="col-sm-6">
                                                            <div class="input-prepend input-group">
                                                                <span class="input-group-addon">GHȻ</span>
                                                                <asp:TextBox ID="txtCostPrice" CssClass="form-control" placeholder="Enter Cost Price" MaxLength="10" runat="server" />
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtCostPrice"
                                                                ErrorMessage="Enter Cost Price"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                ControlToValidate="txtCostPrice"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 .]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Selling Price:</label>
                                                        <div class="col-sm-6">
                                                            <div class="input-prepend input-group">
                                                                <span class="input-group-addon">GHȻ</span>
                                                                <asp:TextBox ID="txtSellingPrice" CssClass="form-control" placeholder="Enter Selling Price" MaxLength="10" runat="server" />
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtSellingPrice"
                                                                ErrorMessage="Enter Selling Price"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                                ControlToValidate="txtSellingPrice"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 .]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Min Product Level:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtMinLevel" CssClass="form-control" placeholder="Enter Min Product Level" runat="server" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtMinLevel"
                                                                ErrorMessage="Enter Min Product Level"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                ControlToValidate="txtMinLevel"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 ]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="button-pane content-box text-center pad20A mrg20T">
                                                        <asp:Button ID="btnSaveProduct" CssClass="btn btn-success float-right" Text="Save Details" OnClick="btnSaveProduct_Click" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

