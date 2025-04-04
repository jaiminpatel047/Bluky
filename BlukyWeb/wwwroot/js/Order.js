
$(document).ready(function () {
    var url = window.location.search;
    
    if (url.includes("padding")) {
        LoadDataTable("padding");
    }
    else if (url.includes("approved")) {
        LoadDataTable("approved");
    }
    else if (url.includes("processing")) {
        LoadDataTable("processing");
    }
    else if (url.includes("pending")) {
        LoadDataTable("pending");
    }
    else if (url.includes("shipped")) {
        LoadDataTable("shipped");
    }
    else {
        LoadDataTable("all");
    }
    
});

var orderTbl
function LoadDataTable(status) {
    orderTbl = $('#orderManageTbl').DataTable({
        ajax: {
            url: '/admin/Order/getall?status=' + status,
        },
        columns: [
            { data: 'name' },
            { data: 'number' },
            { data: 'applicationUser.email' },
            { data: 'orderStatus' },
            { data: 'orderTotal' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="row">
                        <div class="col-6">
                            <a href="/Admin/Order/Detail?orderId=${data}" class="btn btn-primary form-control">
                               <i class="bi bi-pen"></i>
                            </a>
                        </div>
                    </div>`
                }
            },
        ]
    });
}