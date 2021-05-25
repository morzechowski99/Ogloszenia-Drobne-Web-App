const types = {
    0 : 'Data',
    1:'Zmiennoprzecinkowy',
    2:'Checkbox',
    3:'Numer',
    4:'Tekstowy'
}


$(document).ready(function () {
    let categories;
    const atributes = [];

    $.get("/Categories/GetCategoriesWithParent", function () {
        
    })
    .done(function (data) {
        categories = data
    })

    const wirteTree = function () {
        const id = $(".parentCategory").val()
        let tree;
        if (isNaN(id)) {
            tree = $(".categoryInput").val()
            $(".category-tree").empty()
            $(".category-tree").append(tree)
            return
        }
        const parent = categories.find((category) => category.id == id)
        const newCategoryName = $(".categoryInput").val()
        if (newCategoryName.length === 0) return
        tree = `${parent.categoryName}/${newCategoryName}`
        if (parent) {
            let parentCategory = parent.parentCategory
            while (parentCategory) {
                tree = parentCategory.categoryName + '/' + tree
                parentCategory = parentCategory.parentCategory
            }
        }
        $(".category-tree").empty()
        $(".category-tree").append(tree)
    }

    const printTable = function () {
        const tableBody = $(".atributeTable > tbody")
        tableBody.empty()
        atributes.forEach((atribute,idx) => {
            tableBody.append(`<tr><td>${atribute.Name}</td><td>${types[atribute.Type]}</td></tr>`)
            const btn = $(`<button type="button"class="m-0 btn btn-link">Usuń</button>`)
            btn.click(function () {
                removeAtribute(idx)
            })
            const td = $("<td></td>")
            td.append(btn)
            $(".atributeTable tr:last").append(td)
        })
    }

    const removeAtribute = function (idx) {
        atributes.splice(idx, 1)
        printTable()
    }

    $(".parentCategory").change(function () {
        wirteTree()
    })

    $(".categoryInput").blur(function () {
        wirteTree()
    })

    $(".addAtributeButton").click(function () {
        $(".addAtributeDiv").hide()
        $(".addAtributeForm").show()
    })

    $(".addAtributeSubmitButton").click(function () {
        const name = $("input[name='AtributeName']").val();
        const type = $("select[name='AtributeType']").val();
        if (name.length === 0) {
            $("#AtributeNameValidation").text("Nazwa nie moze byc pusta")
            return
        }
        else if (verifyNameUsed(name)) {
            $("#AtributeNameValidation").text("Nazwa zajęta")
            return
        }
        atributes.push({ Name: name, Type: Number.parseInt(type) })
        $(".addAtributeDiv").show()
        $(".addAtributeForm").hide()
        $("input[name='AtributeName']").val("");
        printTable()
    })

    $("input[name='AtributeName']").blur(function () {
        if ($(this).val().length === 0) {
            $("#AtributeNameValidation").text("Nazwa nie moze byc pusta")
        }
        else if (verifyNameUsed($(this).val())) {
            $("#AtributeNameValidation").text("Nazwa zajęta")
        }
        else {
            $("#AtributeNameValidation").text("")
        }
    })

    const verifyNameUsed = function (name) {
        return atributes.findIndex(elem => elem.Name === name) === -1 ? false : true
    }

    $("#form").submit(function () {
        atributes.forEach((atribute, idx) => {
            $(this).append(`<input type="hidden" name="Attributes[${idx}].Name" value="${atribute.Name}"/>`)
            $(this).append(`<input type="hidden" name="Attributes[${idx}].Type" value="${atribute.Type}"/>`)
        }
        )
        
            
        return true;
    })
});





