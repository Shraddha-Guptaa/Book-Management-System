
    $(document).ready( function () {
        Loaddata();
} );
function Loaddata(){

  datatable =   $('#myTable').DataTable( {
    "ajax":{url:'/Product/GetData'},
    "columns": [
        { data: 'title', "width":"15%",},
        { data: 'isbn',"width":"15%" },
        { data: 'author', "width":"15%"},
        { data: 'lastprice', "width":"15%"},
        { data: 'category.name', "width":"15%"},
        { data: 'productId', 
    "render":function (data){
    return `<div class="w-75 btn-group" role="group"><a href="/Product/Upsert/${data}"  class="btn btn-primary"><i class="bi bi-pencil-square"></i>Edit</a>&nbsp;&nbsp;<a onClick=deletedata('/Product/Delete/${data}') class="btn btn-danger"><i class="bi bi-pencil-square"></i>Delete</a></div>`},
 "width":"15%"  }
    ]
} );
}
function deletedata(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        debugger;
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {

                    $('#myTable').DataTable().ajax.reload();

                    Swal.fire(
                        "Deleted!",
                        "Your record has been deleted.",
                        "success"
                    );
                },
                error: function () {
                    toastr.error("Something went wrong");
                }
            });
        }
    });
}

