var companyTbl
$(document).ready(function () {
    companyTbl = $('#companyTbl').DataTable({
        ajax: {
            url: '/admin/company/getall',
        },
        columns: [
            { data: 'name' },
            { data: 'phoneNumber' },
            { data: 'city' },
            { data: 'state' },
            { data: 'pastalCode' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="row">
                        <div class="col-6">
                            <a href="/Admin/company/AddOrUpdate?id=${data}" class="btn btn-primary form-control">Edit</a>
                        </div>
                        <div class="col-6">
                            <a onclick=DeleteCompany('/Admin/company/Delete?id=${data}') class="btn btn-danger form-control">Delete</a>
                        </div>
                    </div>`
                }
            },
        ]
    });
});

function DeleteCompany(url) {
    
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
                        companyTbl.ajax.reload();
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