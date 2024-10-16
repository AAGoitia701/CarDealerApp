/*function confirmDelete(url) {
    var confirmed = confirm("Are you sure you want to delete this?");
    if (confirmed) {
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-CSRF-TOKEN': '@Html.AntiForgeryToken()'
            }
        }
        ).then(response => {
            if (response.ok) {
                location.reload();
            } else {
                alert('Error on delete');
            }
        }).catch(error => {
            console.error('Error:', error);
            alert('Error in request');
        });
    }
}*/

function confirmDelete(url){
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
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'X-CSRF-TOKEN': '@Html.AntiForgeryToken()'
                }
            }).then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    alert('Error on delete');
                }
            }).catch(error => {
                console.error('Error:', error);
                alert('Error in request');
            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}