// Funzione per visualizzare il modale di conferma per la cancellazione
function showDeleteModal(deleteUrl, photoTitle) {
    document.getElementById('deleteForm').action = deleteUrl;
    document.getElementById('photoTitleToDelete').textContent = photoTitle;
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Funzione per chiudere l'alert di successo dopo 2 secondi
function startAlertCountdown(duration, display, closeButton) {
    var timer = duration, seconds;
    var countdownInterval = setInterval(function () {
        seconds = parseInt(timer % 60, 10);

        display.textContent = seconds;

        if (--timer < 0) {
            clearInterval(countdownInterval);
            var alert = document.getElementById('successAlert');
            alert.classList.remove('show');
            closeButton.click();
        }
    }, 1000);
}

document.addEventListener('DOMContentLoaded', function () {
    var alert = document.getElementById('successAlert');
    if (alert) {
        var closeButton = document.getElementById('closeAlertButton');
        var countdown = document.getElementById('countdown');
        startAlertCountdown(2, countdown, closeButton);
    }
});
