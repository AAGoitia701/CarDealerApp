document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('OwnerName');
    const suggestionsBox = document.getElementById('floatingSuggestions');

    // Move box
    function moveSuggestionsBox(event) {
        suggestionsBox.style.left = `${event.pageX + 10}px`;
        suggestionsBox.style.top = `${event.pageY + 10}px`;
    }

    // Listens to mouse movement to move box
    input.addEventListener('mousemove', moveSuggestionsBox);

    // Write on input, it searches the owners with coincidence
    input.addEventListener('input', function () {
        const query = input.value;

        if (query.length > 0) {
            // Hacemos la solicitud de búsqueda (fetch)
            fetch(`/Car/GetOwners?term=${query}`)
                .then(response => response.json())
                .then(data => {
                    suggestionsBox.innerHTML = '';  // Erases old suggestions
                    suggestionsBox.style.display = 'block';  // Shows floating box

                    // Crea la lista de sugerencias
                    const ul = document.createElement('ul');
                    data.forEach(item => {
                        const li = document.createElement('li');
                        li.textContent = item.label;
                        li.addEventListener('click', function () {
                            input.value = item.label;
                            document.getElementById('OwnerId').value = item.value;  // Establishes the value
                            suggestionsBox.style.display = 'none';  // Hides box
                        });
                        ul.appendChild(li);
                    });
                    suggestionsBox.appendChild(ul);
                })
                .catch(error => {
                    console.error('Error fetching owners:', error);
                    suggestionsBox.style.display = 'none';  // If error: hides box
                });
        } else {
            suggestionsBox.style.display = 'none';  // Hides box if no input
        }
    });


    // Hide box when click outside box
    document.addEventListener('click', function (event) {
        if (!input.contains(event.target)) {
            suggestionsBox.style.display = 'none';
        }
    });
});