var table = document.getElementById("ForDynamic_adding_tr_td");
var itemName = document.getElementById("itemName");
var itemPrice = document.getElementById('itemPrice');
var itemQuantity = document.getElementById('itemQuantity')
var total_price = document.getElementById('totalPrice');
var total_quantity = document.getElementById('totalItem');
var CustomerName = document.getElementById('customerName');
var CustomerPhone = document.getElementById('customerPhoneNumber');
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

    CustomerName.innerText = name.value;
    CustomerPhone.innerText = phone.value;
    RecivedDate.innerText = today;


    name.value = "";
    phone.value = "";
}

//SaveItem...........
function SaveItem() {
    var isEmail = document.getElementById('isEmail');
    var responsediv = document.getElementById('response-div');
    var Sucmsg = "Entry Save SuccessFully";
    var Errormsg = "Failed";
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
        "Reciveddate": RecivedDate.innerText,
    }
    var payload = {
        "billObjectDto": billObjectDto,
        "listObjectDto": Obj,
        "isEmail": isEmail.checked ? true : false
    }
    $.ajax({
        type: "POST",
        url: '/Bill/SendBill',
        data: payload,
        success: function (data) {
            responsediv.innerHTML =
                `
<div class="alert alert-warning alert-dismissible fade show" role="alert">
  <strong>Success</strong> ${data}
  <button type="button" class="close right-0" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>
`;
        },
        error: function (error) {
            responsediv.innerHTML = `
                      <div class="alert alert-danger alert-dismissible fade show" role = "alert" ><strong>Error</strong> Some server error Occured
                            < button type = "button" class="btn-close" data - bs - dismiss="alert" aria - label="Close" ></button >
                       </div >`
        }
    });
}

//Print Function
function Print()
{
    var printContent = document.getElementById("print_section");
    var a = window.open('', '');
    a.document.write('<html>');
    a.document.write('<head>')
    a.document.write('<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">');
    a.document.write('<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css">');
    a.document.write('</head>')
    a.document.write('<body>');
    a.document.write(printContent.innerHTML);
    a.document.write('</body></html>');

    a.print();
}

//Back To Bill Page
function BackTomainPage() {
    window.location = "/Bill"
}