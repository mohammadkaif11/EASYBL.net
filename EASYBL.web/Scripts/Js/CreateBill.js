var table = document.getElementById("ForDynamic_adding_tr_td");
var itemName = document.getElementById("itemName");
var itemPrice = document.getElementById('itemPrice');
var itemQuantity = document.getElementById('itemQuantity')
var total_price = document.getElementById('totalPrice');
var total_quantity = document.getElementById('totalItem');
var CustomerName = document.getElementById('customerName');
var CustomerPhone = document.getElementById('customerPhoneNumber');
var deliveryDate = document.getElementById('customerDeliveryDate');
var RecivedDate = document.getElementById('customerRecivedDate');


//SomeGlobalVariable..........
var delete_index = 0;
var golbal_Id = 1;
var TotalPrice = 0;
var TotalQuanity = 0;

//addItem
function addItem() {
    if (table.children.length == 0) {
        delete_index = 0;
        golbal_Id = 1;
    }

    var p = document.createElement("tr");
    p.id = golbal_Id

    var th = document.createElement("th");
    if (delete_index != 0) {
        th.innerText = delete_index + 1;
        delete_index++;
    } else {
        th.innerText = golbal_Id;
    }
    th.scope = "row";
    p.appendChild(th);

    var td1 = document.createElement("td");
    td1.innerText = itemName.value;
    itemName.value = '';
    p.appendChild(td1);

    var td2 = document.createElement("td");
    td2.innerText = itemQuantity.value;

    if (itemQuantity.value != '') {
        var _itemquantity = parseInt(itemQuantity.value);
        TotalQuanity = TotalQuanity + parseInt(itemQuantity.value);
    }
    itemQuantity.value = '';
    p.appendChild(td2);

    var td3 = document.createElement("td");
    td3.innerText = itemPrice.value;
    if (itemPrice.value != '') {
        TotalPrice = TotalPrice + parseInt(itemPrice.value) * _itemquantity;
    }
    itemPrice.value = '';
    p.appendChild(td3);

    var td4 = document.createElement("td");
    td4.innerHTML = '<i class="bi bi-trash3 mx-2 my-2"></i>';
    p.appendChild(td4);
    td4.id = golbal_Id;
    td4.onclick = DeleteCell;
    DeleteCell.bind(this.id);
    p.appendChild(td4);


    table.append(p);
    golbal_Id++;

    updateTotalItemPriceValue();
}

//DeleteCell.................
function DeleteCell() {
    console.log(this.id);
    if (this.id == undefined || this.id == null) {
        return;
    }
    var tr_ = document.getElementById(this.id);
    if (tr_ == undefined || tr_ == null) {
        return;
    }
    if (TotalPrice != 0 || TotalQuanity != 0) {
        if (tr_.children[2].innerText != "" && tr_.children[3].innerText != "") {
            TotalQuanity = TotalQuanity - parseInt(tr_.children[2].innerText);
            TotalPrice = TotalPrice - parseInt(tr_.children[3].innerText) * parseInt(tr_.children[2].innerText);
        }
    }
    table.removeChild(tr_);
    arrangeItem();
    updateTotalItemPriceValue();
}

//arrangeItem
function arrangeItem() {
    var table_child = table.children;
    delete_index = table_child.length;
    var count = 1;
    for (var i = 0; i < table_child.length; i++) {
        var table_inner_child = table_child[i].children;
        table_inner_child[0].innerText = count;
        count++;
    }

}

//UpdateTotalPriceValue.....................
function updateTotalItemPriceValue() {
    if (TotalPrice != 0 || TotalQuanity != 0) {
        total_price.innerText = TotalPrice;
        total_quantity.innerText = TotalQuanity;
    } else {
        total_price.innerText = "0";
        total_quantity.innerText = "0";
    }
}

//ChangetheValue of Select input filed;
function myFunction(e) {
    var value = event.target.value.split(" ");
    console.log(value);
    itemName.value = value[0];
    itemPrice.value = value[1];
}

//Add Customer Details
function addCustomerDetails() {
    let today = new Date().toLocaleDateString()
    var name = document.getElementById('c-name');
    var phone = document.getElementById('c-phoneNumber');
    var delivery_date = document.getElementById('c-deliveryDate');

    CustomerName.innerText = name.value;
    CustomerPhone.innerText = phone.value;
    deliveryDate.innerText = delivery_date.value;
    RecivedDate.innerText = today;


    name.value = "";
    phone.value = "";
    delivery_date.value = "";

}

//SaveItem...........
function SaveItem() {
    var Sucmsg = "Entry Save SuccessFully";
    var Errormsg = "Failed";
    var responsediv = document.getElementById('response-div');
    var Obj = [];
    var table_child = table.children;
    for (var i = 0; i < table_child.length; i++) {
        var table_inner_child = table_child[i].children;
        let billObj = {
            "itemName": table_inner_child[1].innerText,
            "itemPrice": parseFloat(table_inner_child[2].innerText),
            "itemQuantity": parseInt(table_inner_child[3].innerText)
        }
        Obj.push(billObj);
    }
    var billObjectDto = {
        "customername": CustomerName.innerText,
        "customerPhoneNumber": CustomerPhone.innerText,
        "deliverydate": deliveryDate.innerText,
        "Reciveddate": RecivedDate.innerText,
    }
    var payload = {
        "billObjectDto": billObjectDto,
        "listObjectDto": Obj,
    }
    $.ajax({
        type: "POST",
        url: '/Bill/SendBill',
        data: payload,
        success: function (data) {
            responsediv.innerHTML = `<div class="alert alert-success" role="alert">
  ${data}
</div>
 `;

        },
        error: function (error) {
            responsediv.innerHTML =`< div class="alert alert-danger alert-dismissible fade show" role = "alert" >
                                  <strong>Error</strong> Some Server Error
                    < button type = "button" class="btn-close" data - bs - dismiss="alert" aria - label="Close" ></button >
                       </div >`

        }
    });
    console.log(payload)
}
