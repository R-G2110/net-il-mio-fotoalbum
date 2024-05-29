// Function to send the message
function sendMessage(evt) {
    evt.preventDefault();
    const mail = document.getElementById('email').value;
    const content = document.getElementById('messageContent').value;
    const photoId = document.getElementById('photoId').value;

    axios.post('/api/PhotoWebApi/SendMessage', {
        Email: mail,
        Content: content,
        PhotoId: photoId
    }).then(res => {
        console.log(res);
        // Close the modal after successful message send
        var sendMessageModal = new bootstrap.Modal(document.getElementById('sendMessageModal'));
        sendMessageModal.hide();
        // Show success alert
        document.getElementById('successAlert').classList.remove('d-none');
        setTimeout(function () {
            document.getElementById('successAlert').classList.add('d-none');
        }, 3000); // Hide the alert after 3 seconds
    }).catch(error => {
        console.error('Error sending message:', error);
    });
}

// Function to load photos and display them as cards
function loadPhotos(searchString = "") {
    const url = searchString === "" ? '/api/PhotoWebApi/GetAllPhotos' : `/api/PhotoWebApi/GetAllPhotos/${searchString}`;
    axios.get(url)
        .then(response => {
            const photos = response.data;
            const photoCardsContainer = document.getElementById('photoCardsContainer');
            photoCardsContainer.innerHTML = '';

            photos.forEach(photo => {
                const card = document.createElement('div');
                card.classList.add('photo-card', 'card');
                card.innerHTML = `
                                    <img src="${photo.imageUrl}" class="card-img-top" alt="${photo.title} image">
                                    <div class="card-body">
                                        <h5 class="card-title">${photo.title}</h5>
                                        <button class="btn btn-primary" onclick='openSendMessageModal(${JSON.stringify(photo)})'>Send Message</button>
                                    </div>
                                `;
                photoCardsContainer.appendChild(card);
            });
        })
        .catch(error => {
            console.error('Error loading photos:', error);
        });
}

function searchPhoto() {
    const searchString = this.value.trim();
    loadPhotos(searchString);
}

// Load photos on page load
loadPhotos();

document.querySelector('#searchInput').addEventListener('keyup', searchPhoto);
document.getElementById('searchButton').addEventListener('click', function () {
    const searchString = document.getElementById('searchInput').value.trim();
    loadPhotos(searchString);
});

// Function to open the modal for sending a message
function openSendMessageModal(photo) {
    document.getElementById('sendMessageModalLabel').innerText = "Send Message about: " + photo.title;
    document.getElementById('photoId').value = photo.id;
    var sendMessageModal = new bootstrap.Modal(document.getElementById('sendMessageModal'));
    sendMessageModal.show();
}

// Attach event listener to form submit event
document.getElementById('sendMessageForm').addEventListener('submit', sendMessage);

// Hide modal when it's closed
document.getElementById('sendMessageModal').addEventListener('hidden.bs.modal', function () {
    document.getElementById('sendMessageForm').reset(); // Reset form fields
});