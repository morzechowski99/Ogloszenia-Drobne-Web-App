

$(function () {
    let categories;

    const writeTree =  () => {
        const id = parentCategory
        let tree;
        if (isNaN(id)) {
            tree = $(".categoryInput").val()
            $(".tree").empty()
            $(".tree").append(tree)
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
        $(".tree").empty()
        $(".tree").append(tree)
    }
    
    $.get("/Categories/GetCategoriesWithParent", function (data) {
        categories = data
        writeTree()
    })
 
    
    $(".categoryInput").blur(function () {
        writeTree()
    })

})