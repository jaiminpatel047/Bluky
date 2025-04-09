var companyTbl
$(document).ready(function () {
    companyTbl = $('#userTbl').DataTable({
        ajax: {
            url: '/admin/usermanagement/getall',
        },
        columns: [
            { data: 'name' },
            { data: 'email' },
            { data: 'city' },
            { data: 'state' },
            { data: 'role' },
            {
                data: { id : "id", lockoutEnd:"lockoutEnd"},
                render: function (data) {
                    var today = new Date().getTime();
                    var lockDate = new Date(data.lockoutEnd).getTime();

                    if (lockDate > today) {
                        return `<div class="row">
                        <div class="col-6">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger form-control">
                            <i class="bi bi-lock-fill"></i> Lock
                            </a>
                        </div>
                        <div class="col-6">
                            <a href="/Admin/UserManagement/RoleManagement?id=${data.id}" class="btn btn-info form-control">Permission</a>
                        </div>
                    </div>`
                    } else {
                        return `<div class="row">
                        <div class="col-6">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success form-control">
                            <i class="bi bi-unlock-fill"></i> Unlock
                            </a>
                        </div>
                        <div class="col-6">
                            <a href="/Admin/UserManagement/RoleManagement?id=${data.id}" class="btn btn-info form-control">Permission</a>
                        </div>
                    </div>`
                    }
                }
            },
        ]
    });
});


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/UserManagement/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: data.message,
                    showConfirmButton: false,
                    timer: 1500
                });
                companyTbl.ajax.reload();
            }
        }
    });
}
