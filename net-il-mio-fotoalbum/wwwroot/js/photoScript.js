// Funzione per visualizzare il modale di conferma per la cancellazione
function showDeleteModal(deleteUrl, categoryTitle) {
    console.log("sto cliccando");
    document.getElementById('deleteForm').action = deleteUrl;
    document.getElementById('photoTitleToDelete').textContent = categoryTitle;
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

function searchPhoto() {
    // Ottenere il valore della barra di ricerca
    var searchString = document.getElementById("searchInput").value.toLowerCase();

    // Se la barra di ricerca è vuota, mostrare tutte le foto
    if (!searchString.trim()) {
        document.querySelectorAll("#photoTable tbody tr").forEach(function (row) {
            row.style.display = "table-row";
        });
        return;
    }

    // Nascondere tutte le foto che non corrispondono alla ricerca
    document.querySelectorAll("#photoTable tbody tr").forEach(function (row) {
        var title = row.querySelector("td:first-child").textContent.toLowerCase();
        if (title.includes(searchString)) {
            row.style.display = "table-row";
        } else {
            row.style.display = "none";
        }
    });
}
