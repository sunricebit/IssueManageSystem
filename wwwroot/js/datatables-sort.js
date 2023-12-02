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

    // Check the column has all number or not
    var isNumeric = isColumnNumeric(table, columnIndex) ? true : false;

    while (switching) {
        switching = false;
        rows = table.rows;

        console.log(isNumeric);

        if (!isNumeric) {
            for (i = 1; i < (rows.length - 1); i++) {
                shouldSwitch = false;
                x = rows[i].getElementsByTagName("td")[columnIndex];
                y = rows[i + 1].getElementsByTagName("td")[columnIndex];

                if (currentDirection === "asc" ? x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase() : x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        else {
            for (i = 1; i < (rows.length - 1); i++) {
                shouldSwitch = false;
                x = rows[i].getElementsByTagName("td")[columnIndex];
                y = rows[i + 1].getElementsByTagName("td")[columnIndex];

                if (currentDirection === "asc" ? Number(x.innerHTML) > Number(y.innerHTML) : Number(x.innerHTML) < Number(y.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}

function isColumnNumeric(table, columnIndex) {
    var rows = table.rows;
    for (var i = 1; i < rows.length; i++) {
        var cell = rows[i].cells[columnIndex];
        var cellValue = cell.textContent || cell.innerText;

        if (isNaN(cellValue)) {
            return false; 
        }
    }
    return true; 
}