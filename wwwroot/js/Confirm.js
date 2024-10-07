function confirmDelete(url) {
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
                window.location.href = '/Car/Index'; // redirect to index
            } else {
                alert('Error on delete');
            }
        }).catch(error => {
            console.error('Error:', error);
            alert('Error in request');
        });
    }
}