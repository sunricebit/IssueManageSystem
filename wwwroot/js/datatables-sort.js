function sortTable(columnIndex) {
    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementById("sortable-table");
    switching = true;

    var th = table.getElementsByTagName("th")[columnIndex];
    var currentDirection = th.classList.contains("asc") ? "desc" : "asc";

    // Remove arrow classes from all th elements
    var thElements = table.getElementsByTagName("th");
    for (var j = 0; j < thElements.length; j++) {
        thElements[j].classList.remove("asc", "desc");
    }

    // Add arrow class to the clicked th element
    th.classList.add(currentDirection);

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("td")[columnIndex];
            y = rows[i + 1].getElementsByTagName("td")[columnIndex];

            if (currentDirection === "asc" ? x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase() : x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                shouldSwitch = true;
                break;
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
