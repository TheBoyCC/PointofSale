<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change-password.aspx.cs" Inherits="PointSaleSystemWeb.change_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Point of Sale - Change Password</title>

    <meta charset="UTF-8" />
    <!--[if IE]><meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1'><![endif]-->
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <!-- Favicons -->

    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="assets/images/icons/apple-touch-icon-144-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="assets/images/icons/apple-touch-icon-114-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="assets/images/icons/apple-touch-icon-72-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" href="assets/images/icons/apple-touch-icon-57-precomposed.png" />
    <link rel="shortcut icon" href="assets/images/icons/favicon.png" />


    <link rel="stylesheet" type="text/css" href="assets/bootstrap/css/bootstrap.css" />

    <!-- HELPERS -->

    <link rel="stylesheet" type="text/css" href="assets/helpers/animate.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/backgrounds.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/boilerplate.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/grid.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/page-transitions.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/spacing.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/typography.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/utils.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/colors.css" />

    <!-- ELEMENTS -->

    <link rel="stylesheet" type="text/css" href="assets/elements/badges.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/buttons.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/content-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/dashboard-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/forms.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/images.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/info-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/invoice.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/loading-indicators.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/menus.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/panel-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/response-messages.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/responsive-tables.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/ribbon.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/social-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/tables.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/tile-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/elements/timeline.css" />



    <!-- ICONS -->

    <link rel="stylesheet" type="text/css" href="assets/icons/fontawesome/fontawesome.css" />
    <link rel="stylesheet" type="text/css" href="assets/icons/linecons/linecons.css" />
    <link rel="stylesheet" type="text/css" href="assets/icons/spinnericon/spinnericon.css" />


    <!-- WIDGETS -->

    <link rel="stylesheet" type="text/css" href="assets/widgets/accordion-ui/accordion.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/calendar/calendar.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/carousel/carousel.css" />

    <link rel="stylesheet" type="text/css" href="assets/widgets/charts/justgage/justgage.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/charts/morris/morris.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/charts/piegage/piegage.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/charts/xcharts/xcharts.css" />

    <link rel="stylesheet" type="text/css" href="assets/widgets/chosen/chosen.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/colorpicker/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/datatable/datatable.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/datepicker/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/datepicker-ui/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/dialog/dialog.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/dropdown/dropdown.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/dropzone/dropzone.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/file-input/fileinput.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/input-switch/inputswitch.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/input-switch/inputswitch-alt.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/ionrangeslider/ionrangeslider.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/jcrop/jcrop.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/jgrowl-notifications/jgrowl.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/loading-bar/loadingbar.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/maps/vector-maps/vectormaps.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/markdown/markdown.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/modal/modal.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/multi-select/multiselect.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/multi-upload/fileupload.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/nestable/nestable.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/noty-notifications/noty.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/popover/popover.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/pretty-photo/prettyphoto.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/progressbar/progressbar.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/range-slider/rangeslider.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/slidebars/slidebars.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/slider-ui/slider.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/summernote-wysiwyg/summernote-wysiwyg.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/tabs-ui/tabs.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/theme-switcher/themeswitcher.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/timepicker/timepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/tocify/tocify.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/tooltip/tooltip.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/touchspin/touchspin.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/uniform/uniform.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/wizard/wizard.css" />
    <link rel="stylesheet" type="text/css" href="assets/widgets/xeditable/xeditable.css" />

    <!-- SNIPPETS -->

    <link rel="stylesheet" type="text/css" href="assets/snippets/chat.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/files-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/login-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/notification-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/progress-box.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/todo.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/user-profile.css" />
    <link rel="stylesheet" type="text/css" href="assets/snippets/mobile-navigation.css" />

    <!-- APPLICATIONS -->

    <link rel="stylesheet" type="text/css" href="assets/applications/mailbox.css" />

    <!-- Admin theme -->

    <link rel="stylesheet" type="text/css" href="assets/themes/admin/layout.css" />
    <link rel="stylesheet" type="text/css" href="assets/themes/admin/color-schemes/default.css" />

    <!-- Components theme -->

    <link rel="stylesheet" type="text/css" href="assets/themes/components/default.css" />
    <link rel="stylesheet" type="text/css" href="assets/themes/components/border-radius.css" />

    <!-- Admin responsive -->
    <link rel="stylesheet" type="text/css" href="assets/helpers/responsive-elements.css" />
    <link rel="stylesheet" type="text/css" href="assets/helpers/admin-responsive.css" />
    <!-- JS Core -->

    <script type="text/javascript" src="assets/js-core/jquery-core.js"></script>
    <script type="text/javascript" src="assets/js-core/jquery-ui-core.js"></script>
    <script type="text/javascript" src="assets/js-core/jquery-ui-widget.js"></script>
    <script type="text/javascript" src="assets/js-core/jquery-ui-mouse.js"></script>
    <script type="text/javascript" src="assets/js-core/jquery-ui-position.js"></script>
    <!--<script type="text/javascript" src="assets/js-core/transition.js"></script>-->
    <script type="text/javascript" src="assets/js-core/modernizr.js"></script>
    <script type="text/javascript" src="assets/js-core/jquery-cookie.js"></script>

    <!-- Sparklines charts -->

    <script type="text/javascript" src="assets/widgets/charts/sparklines/sparklines.js"></script>
    <script type="text/javascript" src="assets/widgets/charts/sparklines/sparklines-demo.js"></script>

    <script type="text/javascript">
        $(window).load(function () {
            setTimeout(function () {
                $('#loading').fadeOut(400, "linear");
            }, 300);
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="sb-site">
            <div id="page-wrapper">
                <div id="page-header" class="bg-gradient-9">
                    <div id="mobile-navigation">
                        <button id="nav-toggle" class="collapsed" data-toggle="collapse" data-target="#page-sidebar"><span></span></button>
                        <a href="default.aspx" class="logo-content-small" title="POS"></a>
                    </div>
                    <div id="header-logo" class="logo-bg">
                        <a href="default.aspx" class="logo-content-big" title="POS">Point of Sale <i>MGT</i>
                            <span>The perfect solution for user interfaces</span>
                        </a>
                        <a href="default.aspx" class="logo-content-small" title="POS">Point of Sale <i>MGT</i>
                            <span>The perfect solution for user interfaces</span>
                        </a>
                        <a id="close-sidebar" href="#" title="Close sidebar">
                            <i class="glyph-icon icon-angle-left"></i>
                        </a>
                    </div>
                    <div id="header-nav-left">
                        <div class="user-account-btn dropdown">
                            <a href="#" title="My Account" class="user-profile clearfix" data-toggle="dropdown">
                                <asp:Image ID="imgUser" Width="28" alt="" runat="server" ImageUrl="~/assets/image-resources/gravatar.jpg" />
                                <span>
                                    <asp:Label ID="lblUser" Text="Thomas Barnes" runat="server" />
                                </span>
                                <i class="glyph-icon icon-angle-down"></i>
                            </a>
                            <div class="dropdown-menu float-left">
                                <div class="box-sm">
                                    <div class="login-box clearfix">
                                        <div class="user-img">
                                            <a href="#myModal" title="" data-toggle="modal" class="change-img">Change photo</a>
                                            <asp:Image ID="imgUser1" ImageUrl="~/assets/image-resources/gravatar.jpg" alt="" runat="server" />
                                        </div>
                                        <div class="user-info">
                                            <span>
                                                <asp:Label ID="lblUser1" Text="Thomas Barnes" runat="server" />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="pad5A button-pane button-pane-alt text-center">
                                        <a href="#" id="btnSignOut" class="btn display-block font-normal btn-danger" runat="server" onserverclick="btnSignOut_ServerClick" causesvalidation="false">
                                            <i class="glyph-icon icon-power-off"></i>
                                            Logout
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- #header-nav-left -->

                    <div id="header-nav-right">
                        <div class="dropdown" id="notifications-btn">
                            <a data-toggle="dropdown" href="#" title="">
                                <span class="small-badge bg-yellow"></span>
                                <i class="glyph-icon icon-linecons-megaphone"></i>
                            </a>
                            <div class="dropdown-menu box-md float-right">
                                <div class="popover-title display-block clearfix pad10A">
                                    Notifications                                                  
                                    <a class="text-transform-cap font-primary font-normal btn-link float-right" href="#" title="View more options">More options...
                                    </a>
                                </div>
                                <div class="scrollable-content scrollable-slim-box">
                                    <ul class="no-border notifications-box">
                                        <li>
                                            <span class="bg-danger icon-notification glyph-icon icon-bullhorn"></span>
                                            <span class="notification-text">This is an error notification</span>
                                            <div class="notification-time">
                                                a few seconds ago
                                                                          
                                                <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-warning icon-notification glyph-icon icon-users"></span>
                                            <span class="notification-text font-blue">This is a warning notification</span>
                                            <div class="notification-time">
                                                <b>15</b> minutes ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-green icon-notification glyph-icon icon-sitemap"></span>
                                            <span class="notification-text font-green">A success message example.</span>
                                            <div class="notification-time">
                                                <b>2 hours</b> ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-azure icon-notification glyph-icon icon-random"></span>
                                            <span class="notification-text">This is an error notification</span>
                                            <div class="notification-time">
                                                a few seconds ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-warning icon-notification glyph-icon icon-ticket"></span>
                                            <span class="notification-text">This is a warning notification</span>
                                            <div class="notification-time">
                                                <b>15</b> minutes ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-blue icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text font-blue">Alternate notification styling.</span>
                                            <div class="notification-time">
                                                <b>2 hours</b> ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-purple icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text">This is an error notification</span>
                                            <div class="notification-time">
                                                a few seconds ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-warning icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text">This is a warning notification</span>
                                            <div class="notification-time">
                                                <b>15</b> minutes ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-green icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text font-green">A success message example.</span>
                                            <div class="notification-time">
                                                <b>2 hours</b> ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-purple icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text">This is an error notification</span>
                                            <div class="notification-time">
                                                a few seconds ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                        <li>
                                            <span class="bg-warning icon-notification glyph-icon icon-user"></span>
                                            <span class="notification-text">This is a warning notification</span>
                                            <div class="notification-time">
                                                <b>15</b> minutes ago
                               
                                            <span class="glyph-icon icon-clock-o"></span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="pad10A button-pane button-pane-alt text-center">
                                    <a href="#" class="btn btn-primary" title="View all notifications">View all notifications
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="dropdown" id="progress-btn">
                            <a data-toggle="dropdown" href="#" title="">
                                <span class="small-badge bg-azure"></span>
                                <i class="glyph-icon icon-linecons-params"></i>
                            </a>
                            <div class="dropdown-menu pad0A box-sm float-right" id="progress-dropdown">
                                <div class="scrollable-content scrollable-slim-box">
                                    <ul class="no-border progress-box progress-box-links">
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">23%</b>
                                                <div class="progress-title">
                                                    Finishing uploading files
                                    
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="23">
                                                    <div class="progressbar-value bg-blue-alt">
                                                        <div class="progressbar-overlay"></div>
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">91%</b>
                                                <div class="progress-title">
                                                    Roadmap progress
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="91">
                                                    <div class="progressbar-value bg-red">
                                                        <div class="progressbar-overlay"></div>
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">58%</b>
                                                <div class="progress-title">
                                                    Images upload
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="58">
                                                    <div class="progressbar-value bg-green"></div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">74%</b>
                                                <div class="progress-title">
                                                    WordPress migration
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="74">
                                                    <div class="progressbar-value bg-purple"></div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">91%</b>
                                                <div class="progress-title">
                                                    Agile development procedures
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="91">
                                                    <div class="progressbar-value bg-black">
                                                        <div class="progressbar-overlay"></div>
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">58%</b>
                                                <div class="progress-title">
                                                    Systems integration
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="58">
                                                    <div class="progressbar-value bg-azure"></div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="#" title="">
                                                <b class="pull-right">97%</b>
                                                <div class="progress-title">
                                                    Code optimizations
                               
                                                </div>
                                                <div class="progressbar-smaller progressbar" data-value="97">
                                                    <div class="progressbar-value bg-yellow"></div>
                                                </div>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                                <div class="pad5A button-pane button-pane-alt text-center">
                                    <a href="#" class="btn display-block font-normal hover-green" title="View all notifications">View all notifications
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="dropdown" id="dashnav-btn">
                            <a href="#" data-toggle="dropdown" data-placement="bottom" class="popover-button-header tooltip-button" title="Dashboard Quick Menu">
                                <i class="glyph-icon icon-linecons-cog"></i>
                            </a>
                            <div class="dropdown-menu float-right">
                                <div class="box-sm">
                                    <div class="pad5T pad5B pad10L pad10R dashboard-buttons clearfix">
                                        <a href="#" class="btn vertical-button hover-blue-alt" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-dashboard opacity-80 font-size-20"></i>
                                            </span>
                                            Dashboard
                                        </a>
                                        <a href="#" class="btn vertical-button hover-green" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-tags opacity-80 font-size-20"></i>
                                            </span>
                                            Widgets
                                        </a>
                                        <a href="#" class="btn vertical-button hover-orange" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-fire opacity-80 font-size-20"></i>
                                            </span>
                                            Tables
                                        </a>
                                        <a href="#" class="btn vertical-button hover-orange" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-bar-chart-o opacity-80 font-size-20"></i>
                                            </span>
                                            Charts
                                        </a>
                                        <a href="#" class="btn vertical-button hover-purple" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-laptop opacity-80 font-size-20"></i>
                                            </span>
                                            Buttons
                                        </a>
                                        <a href="#" class="btn vertical-button hover-azure" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-code opacity-80 font-size-20"></i>
                                            </span>
                                            Panels
                                        </a>
                                    </div>
                                    <div class="divider"></div>
                                    <div class="pad5T pad5B pad10L pad10R dashboard-buttons clearfix">
                                        <a href="#" class="btn vertical-button remove-border btn-info" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-dashboard opacity-80 font-size-20"></i>
                                            </span>
                                            Dashboard
                                        </a>
                                        <a href="#" class="btn vertical-button remove-border btn-danger" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-tags opacity-80 font-size-20"></i>
                                            </span>
                                            Widgets
                                        </a>
                                        <a href="#" class="btn vertical-button remove-border btn-purple" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-fire opacity-80 font-size-20"></i>
                                            </span>
                                            Tables
                                        </a>
                                        <a href="#" class="btn vertical-button remove-border btn-azure" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-bar-chart-o opacity-80 font-size-20"></i>
                                            </span>
                                            Charts
                                        </a>
                                        <a href="#" class="btn vertical-button remove-border btn-yellow" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-laptop opacity-80 font-size-20"></i>
                                            </span>
                                            Buttons
                                        </a>
                                        <a href="#" class="btn vertical-button remove-border btn-warning" title="">
                                            <span class="glyph-icon icon-separator-vertical pad0A medium">
                                                <i class="glyph-icon icon-code opacity-80 font-size-20"></i>
                                            </span>
                                            Panels
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a class="header-btn" id="logout-btn" href="lockscreen-3.html" title="Lockscreen page example">
                            <i class="glyph-icon icon-linecons-lock"></i>
                        </a>
                    </div>
                    <!-- #header-nav-right -->

                </div>
                <div id="page-content-wrapper">
                    <div id="page-content">
                        <div class="container">
                            <div id="page-title">
                                <h2>Change Password</h2>
                                <p>
                                    <asp:Label ID="lblOldPass" Visible="false" Text="Change Account Password." runat="server" />
                                    <asp:Label ID="lblUrlString" Visible="false" Text="Change Account Password." runat="server" />
                                    <asp:Label ID="lblChangePass" Text="Change Account Password." runat="server" />
                                </p>
                            </div>
                            <div class="panel">
                                <div class="panel-body">
                                    <div class="example-box-wrapper">
                                        <div class="row">
                                            <div class="col-sm-7 col-sm-offset-2">
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
                                                    <h3 class="content-box-header bg-white text-center">Change Password</h3>
                                                    <div class="content-box-wrapper">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="form-horizontal bordered-row">
                                                                    <div class="form-group remove-border">
                                                                        <label class="col-sm-4 control-label">Old Password:</label>
                                                                        <div class="col-sm-6">
                                                                            <asp:TextBox ID="txtOldPassword" TextMode="Password" CssClass="form-control" placeholder="Enter Old Password" runat="server" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFiedValidatorOldPass" ForeColor="Red" runat="server"
                                                                                ControlToValidate="txtOldPassword"
                                                                                ErrorMessage="Enter Old Password"
                                                                                Display="Dynamic">
                                                                            </asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group remove-border">
                                                                        <label class="col-sm-4 control-label">New Password:</label>
                                                                        <div class="col-sm-6">
                                                                            <asp:TextBox ID="txtNewPassword" TextMode="Password" CssClass="form-control" placeholder="Enter New Password" runat="server" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorNewPass" ForeColor="Red" runat="server"
                                                                                ControlToValidate="txtNewPassword"
                                                                                ErrorMessage="Enter New Password"
                                                                                Display="Dynamic">
                                                                            </asp:RequiredFieldValidator>
                                                                            <asp:CustomValidator ID="CustomValidatorNewPass" ForeColor="Red" runat="server"
                                                                                ControlToValidate="txtNewPassword"
                                                                                ErrorMessage="Password should be more than 6 characters"
                                                                                OnServerValidate="CustomValidatorNewPass_ServerValidate"
                                                                                Display="Dynamic">
                                                                            </asp:CustomValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group remove-border">
                                                                        <label class="col-sm-4 control-label">Repeat Password:</label>
                                                                        <div class="col-sm-6">
                                                                            <asp:TextBox ID="txtRepeatPassword" TextMode="Password" CssClass="form-control" placeholder="Repeat Password" runat="server" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRepeatPass" ForeColor="Red" runat="server"
                                                                                ControlToValidate="txtRepeatPassword"
                                                                                ErrorMessage="Repeat Password"
                                                                                Display="Dynamic">
                                                                            </asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="CompareValidatorConPass" ForeColor="Red" runat="server"
                                                                                ErrorMessage="Passwords Do not Match"
                                                                                ControlToValidate="txtRepeatPassword" ControlToCompare="txtNewPassword"
                                                                                Operator="Equal" Type="String" Display="Dynamic">
                                                                            </asp:CompareValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="button-pane content-box text-center pad20A mrg20T">
                                                                        <asp:Button ID="btnChangePassword" CssClass="btn btn-success float-right" Text="Change Password" OnClick="btnChangePassword_Click" runat="server" />
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

            </div>
            <!-- WIDGETS -->

            <script type="text/javascript" src="assets/bootstrap/js/bootstrap.js"></script>
            <!-- JS Interactions -->

            <script type="text/javascript" src="assets/widgets/interactions-ui/resizable.js"></script>
            <script type="text/javascript" src="assets/widgets/interactions-ui/draggable.js"></script>
            <script type="text/javascript" src="assets/widgets/interactions-ui/sortable.js"></script>
            <script type="text/javascript" src="assets/widgets/interactions-ui/selectable.js"></script>

            <!-- jQueryUI Dialog -->

            <!--<link rel="stylesheet" type="text/css" href="assets/widgets/dialog/dialog.css">-->
            <script type="text/javascript" src="assets/widgets/dialog/dialog.js"></script>
            <script type="text/javascript" src="assets/widgets/dialog/dialog-demo.js"></script>

            <!-- Bootstrap Dropdown -->

            <!-- <script type="text/javascript" src="assets/widgets/dropdown/dropdown.js"></script> -->

            <!-- Bootstrap Tooltip -->

            <!-- <script type="text/javascript" src="assets/widgets/tooltip/tooltip.js"></script> -->

            <!-- Bootstrap Popover -->

            <!-- <script type="text/javascript" src="assets/widgets/popover/popover.js"></script> -->

            <!-- Bootstrap Progress Bar -->

            <script type="text/javascript" src="assets/widgets/progressbar/progressbar.js"></script>

            <!-- Bootstrap Buttons -->

            <!-- <script type="text/javascript" src="assets/widgets/button/button.js"></script> -->

            <!-- Bootstrap Collapse -->

            <!-- <script type="text/javascript" src="assets/widgets/collapse/collapse.js"></script> -->

            <!-- Superclick -->

            <script type="text/javascript" src="assets/widgets/superclick/superclick.js"></script>

            <!-- Input switch alternate -->

            <script type="text/javascript" src="assets/widgets/input-switch/inputswitch-alt.js"></script>

            <!-- Slim scroll -->

            <script type="text/javascript" src="assets/widgets/slimscroll/slimscroll.js"></script>

            <!-- Slidebars -->

            <script type="text/javascript" src="assets/widgets/slidebars/slidebars.js"></script>
            <script type="text/javascript" src="assets/widgets/slidebars/slidebars-demo.js"></script>

            <!-- PieGage -->

            <script type="text/javascript" src="assets/widgets/charts/piegage/piegage.js"></script>
            <script type="text/javascript" src="assets/widgets/charts/piegage/piegage-demo.js"></script>

            <!-- Screenfull -->

            <script type="text/javascript" src="assets/widgets/screenfull/screenfull.js"></script>

            <!-- Content box -->

            <script type="text/javascript" src="assets/widgets/content-box/contentbox.js"></script>

            <!-- Overlay -->

            <script type="text/javascript" src="assets/widgets/overlay/overlay.js"></script>

            <!-- Widgets init for demo -->

            <script type="text/javascript" src="assets/js-init/widgets-init.js"></script>

            <!-- Theme layout -->

            <script type="text/javascript" src="assets/themes/admin/layout.js"></script>

            <!-- Theme switcher -->

            <script type="text/javascript" src="assets/widgets/theme-switcher/themeswitcher.js"></script>

        </div>
    </form>
</body>
</html>
