﻿$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }
        })
    });

    $('body').on('click', '.btnAddTableToCart', function (e) {

        debugger;
        e.preventDefault();
        var id = $(this).data('id');
        var datetime = $('#datetimepicker').val();
        var quatity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        /*alert(id + " " + quatity);*/
        $.ajax({
            url: '/shoppingcart/addtabletocart',
            type: 'POST',
            data: { id: id, quantity: quatity, time: datetime },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }
        })
    });
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa sản phẩm này không?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            })
        }
    });
    $('body').on('click', '.btnDeleteTable', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa bàn này không?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/DeleteTable',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            })
        }

    });
    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
        alert("Cập nhật số lượng món ăn thành công");
    });

    $('body').on('click', '.btnUpdateTable', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $('#Quantity_' + id).val();
        UpdateTable(id, quantity);
        alert("Cập nhật số lượng bàn thành công");
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm('Bạn có muốn xóa tất cả sản phẩm không?');
        if (conf == true) {
            DeleteAll();
        }

    });
});

function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);        
        }
    });
}


function DeleteAll() {
    $.ajax({
        url: '/shoppingcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}

function UpdateTable(id, quantity) {
    $.ajax({
        url: '/shoppingcart/UpdateTable',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}
function Update(id,quantity) {
    $.ajax({
        url: '/shoppingcart/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}

function LoadCart() {
    $.ajax({
        url: '/shoppingcart/Partial_Item_cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}

