var productTbl
$(document).ready(function () {
   productTbl = $('#productTbl').DataTable({
        ajax: {
            url: '/admin/product/getall',
        },
        columns: [
            { data: 'title' },
            { data: 'isbn' },
            { data: 'price' },
            { data: 'author' },
            { data: 'category.name' },
            {
                data: 'id',
                render: function (data) {
                    return  `<div class="row">
                        <div class="col-6">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary form-control">Edit</a>
                        </div>
                        <div class="col-6">
                            <a onclick=Delete('/Admin/Product/Detele?id=${data}') class="btn btn-danger form-control">Delete</a>
                        </div>
                    </div>`
                }
            },
        ]
    });
});

function Delete(url) {
    debugger
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success == true) {
                        productTbl.ajax.reload();
                        Swal.fire({
                            title: "Deleted!",
                            text: data.message,
                            icon: "success"
                        })
                    }
                }
                
            })
            
        }
    });
}