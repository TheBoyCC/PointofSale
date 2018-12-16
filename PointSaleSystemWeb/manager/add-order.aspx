<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.Master" AutoEventWireup="true" CodeBehind="add-order.aspx.cs" Inherits="PointSaleSystemWeb.manager.add_order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Point of Sale - Order
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <script type="text/javascript" src="../assets/widgets/chosen/chosen.js"></script>
    <script type="text/javascript" src="../assets/widgets/chosen/chosen-demo.js"></script>

    <script type="text/javascript">
        function ShowModal() {
            $("#confirmOrder").modal('show');
        }
    </script>
    <script type="text/javascript">
        function CloseModal() {
            $("#confirmOrder").modal('hide');
        }
    </script>

    <div id="page-content">
        <div class="container">
            <div id="page-title">
                <h2>Order</h2>
                <p>
                    <asp:Label ID="lblChangePass" Text="Add Details of Order." runat="server" />
                    <asp:Label ID="lblModal" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblOrderID" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblSubOrderNum" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblSum" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblCustomerID" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblCustomerName" Visible="false" Text="" runat="server" />
                    <asp:Label ID="lblCustomerPhone" Visible="false" Text="" runat="server" />
                </p>
            </div>
            <div class="panel">
                <div class="panel-body">
                    <div class="example-box-wrapper">
                        <div class="row">
                            <div class="col-sm-5">
                                <div class="content-box">
                                    <asp:Panel ID="alertErrorPanel" Display="Dynamic" Visible="false" Height="60px" runat="server">
                                        <div class="alert alert-close alert-danger">
                                            <a href="#" id="clsAlertError" title="Close" class="glyph-icon alert-close-btn icon-remove" runat="server" onserverclick="clsAlertError_ServerClick" causesvalidation="false"></a>
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
                                    <h3 class="content-box-header bg-white text-center">Enter Details of Order</h3>
                                    <div class="content-box-wrapper">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal bordered-row">
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Customer Name:</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCustomerName" CssClass="form-control" BackColor="White" placeholder="Customer Name" runat="server" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                                ControlToValidate="txtCustomerName"
                                                                ForeColor="Red"
                                                                ValidationExpression="[a-zA-Z / -]*$"
                                                                ErrorMessage="Enter Aplhabets"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Customer Phone:</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCustomerPhone" CssClass="form-control" BackColor="White" placeholder="Customer Phone" runat="server" OnTextChanged="txtCustomerPhone_TextChanged" AutoPostBack="true" />
                                                            <asp:Label ID="lblExists" ForeColor="Red" Visible="false" Text="" runat="server"></asp:Label>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server"
                                                                ControlToValidate="txtCustomerPhone"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 ]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label">Product Name:</label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlProduct" CssClass="chosen-select form-control" runat="server" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="-- Select Product --" Value="0" Selected="True" />
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" runat="server"
                                                                ControlToValidate="ddlProduct"
                                                                ErrorMessage="Select Product"
                                                                InitialValue="0"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Quantity Available:</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtQuantity" CssClass="form-control" placeholder="Quantity Available" BackColor="White" ReadOnly="true" MaxLength="5" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Unit Price:</label>
                                                        <div class="col-sm-7">
                                                            <div class="input-prepend input-group">
                                                                <span class="input-group-addon">GHȻ</span>
                                                                <asp:TextBox ID="txtUnitPrice" CssClass="form-control" placeholder="Unit Price" BackColor="White" ReadOnly="true" MaxLength="5" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Order Quantity:</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtOrderQty" CssClass="form-control" placeholder="Enter Order Quantity" OnTextChanged="txtOrderQty_TextChanged" runat="server" AutoPostBack="true" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtOrderQty"
                                                                ErrorMessage="Enter Order Quantity"
                                                                Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="valOrderQty" ForeColor="Red" runat="server"
                                                                ControlToValidate="txtOrderQty"
                                                                ErrorMessage=""
                                                                Display="Dynamic"
                                                                OnServerValidate="valOrderQty_ServerValidate">
                                                            </asp:CustomValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                ControlToValidate="txtOrderQty"
                                                                ForeColor="Red"
                                                                ValidationExpression="[0-9 ]*$"
                                                                ErrorMessage="Enter Numbers"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Cost:</label>
                                                        <div class="col-sm-7">
                                                            <div class="input-prepend input-group">
                                                                <span class="input-group-addon">GHȻ</span>
                                                                <asp:TextBox ID="txtCost" CssClass="form-control" placeholder="Cost" BackColor="White" ReadOnly="true" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="button-pane text-center pad20A mrg20T">
                                                        <button id="btnAddToCart" class="btn btn-alt btn-hover btn-primary float-right" onserverclick="btnAddToCart_ServerClick" runat="server" causesvalidation="true">
                                                            <span>Add</span>
                                                            <i class="glyph-icon icon-arrow-right"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-7">
                                <div class="content-box">
                                    <h3 class="content-box-header bg-white text-center"><i class="glyph-icon icon-shopping-cart"></i>Cart</h3>
                                    <div class="content-box-wrapper">
                                        <div class="row">
                                            <div id="divOrder" visible="false" class="col-sm-12" runat="server">
                                                <div class="col-sm-4 pad5B font-size-16">
                                                    <asp:Label ID="lblOrderNumber" Text="" runat="server" />
                                                </div>
                                                <div id="divOrderStatus" class="col-sm-5 col-sm-offset-3 pad12TB" runat="server">
                                                </div>
                                            </div>
                                        </div>
                                        <table id="example1" class="table table-bordered font-size-18">
                                            <thead>
                                                <tr>
                                                    <th class="text-center">Product</th>
                                                    <th class="text-center">Quantity</th>
                                                    <th class="text-center">Cost</th>
                                                    <th class="text-center">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tblOrderItems" runat="server"></tbody>
                                        </table>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div id="divCheckOut" visible="false" class="form-horizontal bordered-row" runat="server">
                                                    <div class="button-pane text-center pad20A mrg10T">
                                                        <div class="row">
                                                            <div class="col-sm-4 pad12BT font-size-18 font-bold">
                                                                <asp:Label ID="lblTotalCost" Text="" runat="server" />
                                                            </div>
                                                            <div class="col-sm-5 col-sm-offset-3">
                                                                <button id="btnCancel" class="btn btn-danger mrg5R" onserverclick="btnCancel_ServerClick" runat="server" causesvalidation="false">
                                                                    <span>CANCEL</span>
                                                                </button>
                                                                <button id="btnCheckOut" class="btn btn-success" onserverclick="btnCheckOut_ServerClick" runat="server" causesvalidation="false">
                                                                    <span>CHECKOUT</span>
                                                                </button>
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
        </div>
        <div class="modal fade" id="confirmOrder" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="divModal" class="modal-header bg-danger" runat="server">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" causesvalidation="false">&times;</button>
                        <h4 class="modal-title">
                            <asp:Label ID="lblModTitle" Text="Delete Hall" runat="server"></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12 text-center font-size-14">
                                <asp:Label ID="lblModMessage" Text="Are you sure you want to delete Hall" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal" causesvalidation="false">
                            <i class="glyph-icon icon-times icon-large"></i>
                            Close
                        </button>
                        <button id="btnModYes" type="button" class="btn btn-danger" runat="server" onserverclick="btnModYes_ServerClick" causesvalidation="false">
                            <i class="glyph-icon icon-check icon-large"></i>
                            Yes
                        </button>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
