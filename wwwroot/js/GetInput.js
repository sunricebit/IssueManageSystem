function addNonEmptyInputsToForm(formId) {
    console.log("goi ham nay");
    var form = document.getElementById(formId);
    if (form) {
        var inputs = form.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type !== "hidden" && inputs[i].value !== "") {
                var newInput = document.createElement("input");
                newInput.type = "hidden";
                newInput.name = inputs[i].name;
                newInput.value = inputs[i].value;
                form.appendChild(newInput);
            }
        }
    }
}

function changePage(pageNumber) {

    // Thay đổi giá trị của input hidden pageNumber trong form
    document.getElementById("pageNumber").value = pageNumber;
    console.log("goi ham nay");

    // Gọi hàm để thêm tất cả các input có giá trị trong form
    addNonEmptyInputsToForm("FilterAndSearchForm");

    // Gửi form khi một trang được chọn
    document.getElementById("FilterAndSearchForm").submit();
}