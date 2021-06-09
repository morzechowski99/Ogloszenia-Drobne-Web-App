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

    loadAtributes($('#attributeSelect').val())

    $('#attributeSelect').change(function () {
        
        loadAtributes($(this).val())
    })
    
})