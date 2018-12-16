<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.Master" AutoEventWireup="true" CodeBehind="manage-product.aspx.cs" Inherits="PointSaleSystemWeb.manager.manage_product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Point of Sale - View Products
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <script type="text/javascript" src="../assets/widgets/datatable/datatable.js"></script>
    <script type="text/javascript" src="../assets/widgets/datatable/datatable-bootstrap.js"></script>
    <script type="text/javascript" src="../assets/widgets/datatable/datatable-tabletools.js"></script>
    <script type="text/javascript">

        /* Datatables basic */

        $(document).ready(function () {
            $('#datatable-example').dataTable();
        });

        /* Datatables hide columns */
    </script>
    <div id="page-content">
        <div class="container">
            <div id="page-title">
                <h2>Products</h2>
                <p>
                    <asp:Label ID="lblChangePass" Text="View and Manage Products." runat="server" />
                </p>
            </div>
            <div class="panel">
                <div class="panel-body">
                    <div class="example-box-wrapper">
                        <div class="row">
                            <div class="col-sm-12">
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
                                    <h3 class="content-box-header bg-white text-center">Product Details</h3>
                                    <div class="content-box-wrapper">
                                        <table id="datatable-example" class="table table-striped table-bordered font-size-18">
                                            <thead>
                                                <tr>
                                                    <th class="text-center">Product</th>
                                                    <th class="text-center">Category</th>
                                                    <th class="text-center">Quantity</th>
                                                    <th class="text-center">Cost Price</th>
                                                    <th class="text-center">Sellig Price</th>
                                                    <th class="text-center">Action</th>
                                                    <th class="text-center">Status</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tblProduct" runat="server"></tbody>
                                        </table>
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
