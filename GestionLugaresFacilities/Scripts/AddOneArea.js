// Seleccionar todos los td dentro de la tabla que no tengan la clase "follow"
$('td:not(.follow)').each(function () {
    // Agregar función onclick a cada td seleccionado
    $(this).click(function () {
        // Tu función onclick aquí
      

        if (this.classList.contains('follow')) {
            // Ejecutar el código JavaScript si el elemento contiene la clase 'js'
            console.log('El elemento contiene la clase "Follow"');
        } else {
            var response = window.confirm('Agregar nueva area');
            if (response === true)
                SendArea(this);
        }
             
    });
});
// Agregar efecto de bordes al pasar el cursor sobre las celdas
$('#tableDiagram td:not(.follow)').hover(
    function () {
        // Al pasar el cursor sobre la celda
        $(this).addClass('hovered');
    },
    function () {
        // Al salir del cursor de la celda
        $(this).removeClass('hovered');
    }
);

function SendArea(datajson) {
    datajson = datajson.getAttribute('data-coord');
    datajson = JSON.parse(datajson);
    datajson = JSON.stringify(datajson);

    $.ajax({
        url: '/Tbl_Place_Diagram/AddCells', // URL de tu acción en el controlador que devuelve los datos
        type: 'POST',
        data: {
            idDiagram: id_diagram,
            data: datajson
        },
        success: function (data) {
            // Manejar la respuesta del servidor
            //console.log('Respuesta del servidor:', response);
            alert("Area agregada!!");
            location.reload();
        },
        error: function (error) {
            console.error('Error en la petición AJAX: script AddOneArea.js', error);
        }
    });

}

