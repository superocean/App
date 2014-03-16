//
//
function allFunction() {
    deleteItem();
    var href = window.location.pathname.toLowerCase();
    switch (href) {
        case "/admin":
        case "/admin/category":
        case "/admin/category/index":
            updatePosition();
            break;
        case "/admin/attribute/add":
            addOptionValue();
            break;
        case "/admin/product":
        case "/admin/product/index":
            updateProductView();
            categoryFilterChange();
            searchProduct_Enter();
            searchProduct_Button();
            addSearchTextToTextBox();
            sortByName();
            sortByPrice();
        case "/admin/product/add":
            categoryChange();
            SetAttributeSaved();
            optionSelectChange();
        case "/admin/newscategory":
        case "/admin/newscategory/index":
            updateNewsCategoryPosition();
            break;
        case "/admin/managenews":
        case "/admin/managenews/index":
            categoryNewsFilterChange();
            searchNews_Enter();
            searchNews_Button();
            addSearchTextToTextBox();
            break;
        default:
    }
    if (href.indexOf("/admin/attribute/edit") != -1) {
        addOptionValue();
        showOptionAttributeEdit();
    }
    if (href.indexOf("/admin/product/edit") != -1) {
        categoryChange();
        SetAttributeSaved();
        optionSelectChange();
    }
};

//
//-------------------------------For all----------------------------------------------------
//Get queryString by name
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}
//
//--------------------------------For category admin----------------------------------
//

function deleteItem() {
    $(".delete").click(function (e) {
        e.preventDefault();
        deleteConfirm(this);
        $("#dialog-confirm").dialog("open");
    });
}
function setHideNotification(t) {
    setTimeout("hideNotification()", t);
}
function hideNotification() {
    $(".notification").hide(500);
}
function deleteConfirm(o) {
    $("#dialog:ui-dialog").dialog("destroy");

    $("#dialog-confirm").dialog({
        resizable: false,
        autoOpen: false,
        height: 140,
        modal: true,
        buttons: {
            "Delete": function () {
                $(this).dialog("close");
                $.ajax({
                    url: $(o).attr("ref"),
                    dataType: "json",
                    data: { id: $(o).attr("href") },
                    type: "Post",
                    success: function (result) {
                        $(o).parent().parent().hide();
                        $("#message_notification").html(result);
                        $(".notification").show(500);
                        setHideNotification(5000);
                    },
                    error: function (err) {
                        $("#message_notification").html("Delete category error");
                    }
                    ,
                    statusCode: {
                        404: function () {
                            $("#message_notification").html("Page not found");
                        },
                        500: function () {
                            $("#message_notification").html("Delete category error");
                        }
                    }
                });
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}
function updatePosition() {
    $(".updatePosition").click(function (e) {
        e.preventDefault();
        var sortorder = $(this).parent().parent().find(".input_sortorder").val();
        $.ajax({
            url: "/Admin/Category/UpdatePosition",
            dataType: "json",
            data: { id: $(this).attr("href"), order: sortorder },
            type: "Post",
            success: function (result) {
                window.location.href = "/Admin/Category";
            }
        });
    });
}
//
//=====================================================================================
//-------------------------------For product admin-------------------------------------
//
function AttributeSaved(id, name, value) {
    this.Id = id;
    this.Name = name;
    this.Value = value;
}
function SetAttributeSaved() {
    var arrAttr = new Array();
    $(".attribute_Product").each(function (index, item) {
        var attrSaved = new AttributeSaved($(item).find("input").first().val(), $(item).find("label").first().text(), $(item).find("select").first().val());
        arrAttr.push(attrSaved);
    });
    var strSaved = JSON.stringify(arrAttr);
    $("#attribute_Product_saved").val(strSaved);
}
function optionSelectChange() {
    $(".selectAttribute_option").live("change", function () {
        SetAttributeSaved();
    });
}
function categoryChange() {
    $("#CategoryId").change(function () {
        var changeValue = $("#CategoryId").val();
        $.ajax({
            url: "/Admin/Product/CategoryChange",
            type: "Post",
            dataType: "json",
            data: { id: changeValue },
            success: function (result) {
                $("#tab3").children().remove();
                var sthtml = "";
                for (var i = 0; i < result.length; i++) {
                    sthtml = sthtml + "<p class='attribute_Product'><input type='hidden' value='" + result[i].Id + "' /><label>" + result[i].Name + "</label><select class='dropdownlist selectAttribute_option'>";
                    for (var j = 0; j < result[i].Attributes.length; j++) {
                        sthtml = sthtml + "<option value=' " + result[i].Attributes[j].Value + "'>" + result[i].Attributes[j].Value + "</option>";
                    }
                    sthtml = sthtml + "</select></p>";
                }
                sthtml = sthtml + "<input type='hidden' id='attribute_Product_saved' name='Attributes' />";
                $("#tab3").html(sthtml);
                SetAttributeSaved();
            }
        });
    });
}
function browserServerCKE() {
    var finder = new CKFinder();
    finder.basePath = '/Content/ckfinder';
    finder.popup();
}
function updateProductView() {
    $(".updateProductView").click(function (e) {
        e.preventDefault();
        var productId = $(this).attr("href");
        var isShowHomePage = $(this).parent().parent().find("input.setShowHomePage").first().is(":checked");
        var isShowBanner = $(this).parent().parent().find("input.setShowBanner").first().is(":checked");
        $.ajax({
            url: "/Admin/Product/UpdateProductView",
            type: "post",
            dataType: "json",
            data: { id: productId, isShowHomePage: isShowHomePage, isShowBanner: isShowBanner },
            success: function (result) {
                $("#message_notification").html(result);
                $(".notification").show(500);
                setHideNotification(5000);
            }
        });
    });
}
function categoryFilterChange() {
    $("#categorysearch").change(function () {
        var catId = $(this).val();
        var currentUrl = window.location.href;
        $.ajax({
            url: "/Admin/Product/ResetQueryString",
            dataType: "json",
            type: "Post",
            data: { url: currentUrl, queryString: "catid" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?catid=" + catId + "" : "&catid=" + catId + "");
            }
        });
        //currentUrl.indexOf("?") == -1 ? "?catid=" + catId + "" : "&catid=" + catId + "";
    });
}
function searchProduct_Enter() {
    var textbox = $("#txtSearchbox");
    var code = null;
    textbox.keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            searchProduct();
            e.preventDefault();
        }
    });
}
function searchProduct_Button() {
    $("#button_search_product").click(function () {
        searchProduct();
    });
}
function searchProduct() {
    var textbox = $("#txtSearchbox");
    var key = textbox.val();
    if (key != '') {
        var currentUrl = window.location.href;
        $.ajax({
            url: "/Admin/Product/ResetQueryString",
            dataType: "json",
            type: "Post",
            data: { url: currentUrl, queryString: "q" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?q=" + key + "" : "&q=" + key + "");
            }
        });
    }
}

function addSearchTextToTextBox() {
    var text = getParameterByName("q");
    if (text != "") {
        $("#txtSearchbox").val(text);
    }
}
function sortByName() {
    $("#sort_by_name").click(function () {
        var currentUrl = window.location.href;
        var sortorder = getParameterByName("sortorder");
        var isDesc = sortorder == "1" ? false : true;
        $.ajax({
            url: "/Admin/Product/ResetQueryString",
            dataType: "json",
            cache: false,
            type: "Post",
            data: { url: currentUrl, queryString: "orderby;sortorder" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?orderby=name" : "&orderby=name") + (isDesc ? "&sortorder=1" : "");
            }
        });
    });
}
function sortByPrice() {
    $("#sort_by_price").click(function () {
        var currentUrl = window.location.href;
        var sortorder = getParameterByName("sortorder");
        var isDesc = sortorder == "1" ? false : true;
        $.ajax({
            url: "/Admin/Product/ResetQueryString",
            dataType: "json",
            cache: false,
            type: "Post",
            data: { url: currentUrl, queryString: "orderby;sortorder" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?orderby=price" : "&orderby=price") + (isDesc ? "&sortorder=1" : "");
            }
        });
    });
}
//
//=====================================================================================
//-------------------------------For attribute admin-------------------------------------
//
function addOptionValue() {
    $("#addoption").click(function () {
        var value = $("#optionvalue").val();
        if (value != "") {
            var html = $("#listOption");
            html.append('<tr><td><input class="value_option_result" type="hidden" value="' + value + '"/><span>' + value + '</span></td><td><a class="edit_option" href="#" title="Edit"><img src="/Content/images/admin/icons/pencil.png" alt="Edit" /></a>&nbsp;<a class="delete_option" href="#" title="Delete"><img src="/Content/images/admin/icons/cross.png" alt="Delete" /></a>&nbsp;<a style="display:none" class="update_option" href="#" title="Update"><img src="/Content/images/admin/icons/tick_circle.png" alt="Update" /></a></td></tr>');
            changeOption();
            $("#optionvalue").val("");
        }
    });
    $(".edit_option").live("click", function (e) {
        e.preventDefault();
        $(this).parent().find(".update_option").show();
        var value = $(this).parent().parent().find("span").first().text();
        $(this).parent().parent().find("td").first().html("<input class='text-input large-input small-width' type='text' value='" + value + "'/>");
    });
    $(".update_option").live("click", function (e) {
        e.preventDefault();
        var obj = $(this).parent().parent().find("td").first().find("input.small-width").first();
        if (obj.length) {
            var value = obj.val();
            if (value != "") {
                $(this).parent().parent().find("td").first().html('<input class="value_option_result" type="hidden" value="' + value + '"/><span>' + value + '</span>');
                changeOption();
                $(this).hide();
            }
        }
    });
    $(".delete_option").live("click", function (e) {
        e.preventDefault();
        $(this).parent().parent().remove();
        changeOption();
    });
}
function changeOption() {
    var result = "";
    $(".value_option_result").each(function (index, item) {
        var val = $(item).val();
        result = result + val + ";";
    });
    $("#resultOption").val(result);
    var selectedvalue = $("#Value").val();
    clearSetDefaultDdl();
    setDefaultDdl(selectedvalue);
}

function setDefaultDdl(selectedValue) {
    var result = $("#resultOption").val();
    var arr = result.split(";");

    $.each(arr, function (index, item) {
        if (index != arr.length - 1) {
            if (item == selectedValue) {
                $("#Value").append("<option selected='selected' value='" + item + "'>" + item + "</option>");
            }
            else {
                $("#Value").append("<option value='" + item + "'>" + item + "</option>");
            }
        }
    });
    if (arr.length == 1) {
        $("#Value").append("<option value='Not have any option'>--Not have any option--</option>");
    }
}
function clearSetDefaultDdl() {
    $("#Value").find("option").remove();
}
function showOptionAttributeEdit() {
    $("#show_add_option").click(function () {
        $("#show_option_attribute_edit").show();
    });
    changeOption();
}

//
//=====================================================================================
//-------------------------------For news category admin-------------------------------------
//
function updateNewsCategoryPosition() {
    $(".updatePosition").click(function (e) {
        e.preventDefault();
        var sortorder = $(this).parent().parent().find(".input_sortorder").val();
        $.ajax({
            url: "/Admin/NewsCategory/UpdatePosition",
            dataType: "json",
            data: { id: $(this).attr("href"), order: sortorder },
            type: "Post",
            success: function (result) {
                window.location.href = "/Admin/NewsCategory";
            }
        });
    });
}

//
//=====================================================================================
//-------------------------------For news  admin-------------------------------------
//
function categoryNewsFilterChange() {
    $("#categorysearch").change(function () {
        var catId = $(this).val();
        var currentUrl = window.location.href;
        $.ajax({
            url: "/Admin/ManageNews/ResetQueryString",
            dataType: "json",
            type: "Post",
            data: { url: currentUrl, queryString: "catid" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?catid=" + catId + "" : "&catid=" + catId + "");
            }
        });
    });
}
function searchNews_Enter() {
    var textbox = $("#txtSearchbox");
    var code = null;
    textbox.keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            searchNews();
            e.preventDefault();
        }
    });
}
function searchNews_Button() {
    $("#button_search_product").click(function () {
        searchNews();
    });
}
function searchNews() {
    var textbox = $("#txtSearchbox");
    var key = textbox.val();
    if (key != '') {
        var currentUrl = window.location.href;
        $.ajax({
            url: "/Admin/ManageNews/ResetQueryString",
            dataType: "json",
            type: "Post",
            data: { url: currentUrl, queryString: "q" },
            success: function (result) {
                var indexQ = result.indexOf("?");
                window.location.href = result + (indexQ == -1 ? "?q=" + key + "" : "&q=" + key + "");
            }
        });
    }
}

function addSearchTextToTextBox() {
    var text = getParameterByName("q");
    if (text != "") {
        $("#txtSearchbox").val(text);
    }
}