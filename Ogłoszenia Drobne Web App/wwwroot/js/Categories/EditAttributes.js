const deleteAtribute = (id) => {
    if (confirm("Czy na pewno chcesz usunąć ten atrybut? Spowoduje to usuniecie go z każdej oferty.")) {
        deleteAttribute(id)
    }
}

const deleteAttribute = (id) => {
    $.ajax({
        method: "DELETE",
        url: "/Categories/DeleteAttribute?id=" + id
    })
        .done(function () {
            window.location.reload()
        })
        .fail(function () {
            alert("Cos poszlo nie tak")
        })
}

const editAttribute = (id,name) => {
    $('#editAtributeForm').show()
    $('#editAtributeForm :input[name="Id"]').val(id)
    $('#editAtributeForm :input[name="Name"]').val(name)
}