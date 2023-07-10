function DealFormInitialice(getCategoriesUrl) {
    $("#OperationTypeId").change(async function () {
        const operationTypeId = $(this).val();

        const response = await fetch(getCategoriesUrl, {
            method: 'POST',
            body: operationTypeId,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await response.json();
        const options = json.map(category => `<option value=${category.value}>${category.text}</option>`);
        $("#CategoryId").html(options);
    })
}