$(function () {
    const loadAtributes = (categoryId) => {
        if (categoryId >= 0) {
            $('#loader').show()
            
            $('#attributesDiv').load('/Offers/AttributesPartial?categoryId=' + categoryId, null, function () {
                $('#loader').hide()

            });
        } else {
            $('#attributesDiv').empty()
        }
    }

    $('#attributeSelect').change(function () {
        
        loadAtributes($(this).val())
    })
    
})