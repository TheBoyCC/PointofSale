<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.Master" AutoEventWireup="true" CodeBehind="delete-order-item.aspx.cs" Inherits="PointSaleSystemWeb.manager.delete_order_item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Point of Sale - Order
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <script type="text/javascript" src="../assets/widgets/chosen/chosen.js"></script>
    <script type="text/javascript" src="../assets/widgets/chosen/chosen-demo.js"></script>

    <script type="text/javascript">
        function ShowModal() {
            $("#deleteOrder").modal('show');
        }
    </script>
    <div id="page-content">
        <div class="container">
            <div id="page-title">
                <h2>Order</h2>
                <p>
                    <asp:Label ID="lblOrderID" Visible="false" Text="Success message title" runat="server"></asp:Label>
                    <asp:Label ID="lblOldQty" Visible="false" Text="Success message title" runat="server"></asp:Label>
                    <asp:Label ID="lblProductName" Visible="false" Text="Success message title" runat="server"></asp:Label>
                    <asp:Label ID="lblItemID" Visible="false" Text="Success message title" runat="server"></asp:Label>
                    <asp:Label ID="lblProductID" Visible="false" Text="Success message title" runat="server"></asp:Label>
                    <asp:Label ID="lblChangePass" Text="Delete Order Item." runat="server" />
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
                                    <h3 class="content-box-header bg-white text-center">Enter Details of Order</h3>
                                    <div class="content-box-wrapper">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal bordered-row">
                                                    <div class="form-group remove-border">
                                                        <label class="col-sm-4 control-label">Product Name:</label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlProduct" CssClass="chosen-select form-control disabled" BackColor="White" runat="server">
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
                                                            <asp:TextBox ID="txtOrderQty" CssClass="form-control" placeholder="Enter Order Quantity" BackColor="White" ReadOnly="true" runat="server" />
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
                                                        <button id="btnAddToCart" class="btn btn-alt btn-hover disabled btn-primary float-right">
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
                                                <div class="col-sm-5 col-sm-offset-3 pad12TB">
                                                    <span id="lblOrderStatus" class="bs-label label-yellow float-right" runat="server">PENDING</span>
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
                                                                <button id="btnCancel" class="btn btn-info mrg5R" runat="server">
                                                                    <span>CANCEL</span>
                                                                </button>
                                                                <button id="btnCheckOut" class="btn btn-success" runat="server">
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
        <div class="modal fade" id="deleteOrder" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="divModal" class="modal-header bg-danger" runat="server">
                        <button id="btnClsModal" type="button" class="close" runat="server" onserverclick="btnModClose_ServerClick" aria-hidden="true" causesvalidation="false">&times;</button>
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
                        <button id="btnModClose" type="button" class="btn btn-default" runat="server" onserverclick="btnModClose_ServerClick" causesvalidation="false">
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
